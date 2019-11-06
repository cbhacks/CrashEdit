using Crash;
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class MainControl : UserControl
    {
        private FileInfo fileinfo;
        private GameVersion gameversion;
        private Dictionary<Crash.UI.Controller,ControllerData> controllers;
        private Crash.UI.NSFController nsfc;
        private ControllerData nsfcd;
        private ControllerData activecd;
        private CommandManager commandmanager;

        public event EventHandler SyncMasterUI;

        public MainControl(FileInfo fileinfo,GameVersion gameversion)
        {
            InitializeComponent();
            this.fileinfo = fileinfo;
            this.gameversion = gameversion;
            this.controllers = new Dictionary<Crash.UI.Controller,ControllerData>();
            byte[] data = File.ReadAllBytes(fileinfo.FullName);
            NSF nsf = NSF.LoadAndProcess(data,gameversion);
            nsfc = new Crash.UI.NSFController(nsf);
            nsfcd = new ControllerData(this,nsfc);
            controllers.Add(nsfc,nsfcd);
            uxTree.Nodes.Add(nsfcd.Node);
            nsfc.DeepItemAdded += nsfc_DeepItemAdded;
            nsfc.DeepItemRemoved += nsfc_DeepItemRemoved;
            nsfc.DeepPopulate(nsfc_DeepItemAdded);
            this.activecd = null;
            this.commandmanager = new CommandManager();
            commandmanager.CommandExecuted += new EventHandler(commandmanager_CommandExecuted);
            uxImageList.Images.Add("NSFController",Properties.Resources.Computer_File_053);
            uxImageList.Images.Add("NormalChunkController",Properties.Resources.People_014);
            uxImageList.Images.Add("TextureChunkController",Properties.Resources.Computer_File_068);
            uxImageList.Images.Add("OldSoundChunkController",Properties.Resources.People_020_cyan);
            uxImageList.Images.Add("SoundChunkController",Properties.Resources.People_020);
            uxImageList.Images.Add("WavebankChunkController",Properties.Resources.People_020_red);
            uxImageList.Images.Add("SpeechChunkController",Properties.Resources.People_017);
            uxImageList.Images.Add("UnprocessedChunkController",Properties.Resources.People_020_code);
            uxImageList.Images.Add("OldAnimationEntryController",Properties.Resources.objects_012_crimson);
            uxImageList.Images.Add("T1EntryController",Properties.Resources.objects_012_gray);
            uxImageList.Images.Add("OldModelEntryController",Properties.Resources.objects_012_violet);
            uxImageList.Images.Add("ModelEntryController",Properties.Resources.objects_012_violet);
            uxImageList.Images.Add("OldSceneryEntryController",Properties.Resources.objects_012_lime);
            uxImageList.Images.Add("SceneryEntryController",Properties.Resources.objects_012_lime);
            uxImageList.Images.Add("SLSTEntryController",Properties.Resources.objects_012_gray);
            uxImageList.Images.Add("T6EntryController",Properties.Resources.objects_012_gray);
            uxImageList.Images.Add("OldZoneEntryController",Properties.Resources.objects_012_blue);
            uxImageList.Images.Add("ZoneEntryController",Properties.Resources.objects_012_blue);
            uxImageList.Images.Add("T11EntryController",Properties.Resources.objects_012_gray);
            uxImageList.Images.Add("SoundEntryController",Properties.Resources.objects_053_blue);
            uxImageList.Images.Add("OldMusicEntryController",Properties.Resources.objects_006_yellow);
            uxImageList.Images.Add("MusicEntryController",Properties.Resources.objects_006);
            uxImageList.Images.Add("WavebankEntryController",Properties.Resources.objects_006_red);
            uxImageList.Images.Add("OldT15EntryController",Properties.Resources.objects_012_gray);
            uxImageList.Images.Add("T15EntryController",Properties.Resources.objects_012_gray);
            uxImageList.Images.Add("OldT17EntryController",Properties.Resources.objects_012_gray);
            uxImageList.Images.Add("T17EntryController",Properties.Resources.objects_012_gray);
            uxImageList.Images.Add("PaletteEntryController",Properties.Resources.objects_012_gray);
            uxImageList.Images.Add("DemoEntryController",Properties.Resources.objects_012);
            uxImageList.Images.Add("T20EntryController",Properties.Resources.objects_012_gray);
            uxImageList.Images.Add("SpeechEntryController",Properties.Resources.objects_053_white);
            uxImageList.Images.Add("T21EntryController",Properties.Resources.objects_012_gray);
            uxImageList.Images.Add("UnprocessedEntryController",Properties.Resources.objects_012_code);
        }

        public FileInfo FileInfo
        {
            get { return fileinfo; }
            set { fileinfo = value; }
        }

        public Crash.UI.NSFController NSFController
        {
            get { return nsfc; }
        }

        public Crash.UI.Controller SelectedController
        {
            get
            {
                if (uxTree.SelectedNode == null)
                    return null;
                return ((ControllerData)uxTree.SelectedNode.Tag).Controller;
            }
        }

        public CommandManager CommandManager
        {
            get { return commandmanager; }
        }

        private void SyncUI()
        {
            if (SyncMasterUI != null)
            {
                SyncMasterUI(this,EventArgs.Empty);
            }
        }

        private class ControllerData : IDisposable
        {
            private MainControl maincontrol;
            private Crash.UI.Controller controller;
            private TreeNode node;
            private Control control;

            public ControllerData(MainControl maincontrol,Crash.UI.Controller controller)
            {
                this.maincontrol = maincontrol;
                this.controller = controller;
                this.node = new TreeNode();
                this.control = null;
                controller.Invalidated += controller_Invalidated;
                controller_Invalidated(null,null);
                node.Tag = this;
            }

            public Crash.UI.Controller Controller
            {
                get { return controller; }
            }

            public TreeNode Node
            {
                get { return node; }
            }

            public Control Control
            {
                get
                {
                    if (control == null)
                    {
                        try
                        {
                            control = controller.CreateControl();
                            control.Dock = DockStyle.Fill;
                        }
                        catch (Exception ex)
                        {
                            maincontrol.txtException.Text = ex.ToString();
                            return maincontrol.pnError;
                        }
                    }
                    return control;
                }
            }

            public void Show()
            {
                maincontrol.uxSplit.Panel2.Controls.Add(Control);
            }

            public void Hide()
            {
                maincontrol.uxSplit.Panel2.Controls.Remove(Control);
            }

            public void Dispose()
            {
                if (control != null)
                {
                    control.Dispose();
                    control = null;
                }
            }

            private void controller_Invalidated(object sender,EventArgs e)
            {
                node.Text = controller.ToString();
                node.ImageKey = controller.ImageKey;
                node.SelectedImageKey = controller.ImageKey;
                node.ForeColor = controller.ForeColor;
                node.BackColor = controller.BackColor;
            }
        }

        private void nsfc_DeepItemAdded(object sender,EvListEventArgs<Crash.UI.Controller> e)
        {
            TreeNode parentnode = controllers[(Crash.UI.Controller)sender].Node;
            ControllerData newdata = new ControllerData(this,e.Item);
            controllers.Add(e.Item,newdata);
            parentnode.Nodes.Insert(e.Index,newdata.Node);
        }

        private void nsfc_DeepItemRemoved(object sender,EvListEventArgs<Crash.UI.Controller> e)
        {
            TreeNode parentnode = controllers[(Crash.UI.Controller)sender].Node;
            controllers[e.Item].Dispose();
            controllers.Remove(e.Item);
            parentnode.Nodes.RemoveAt(e.Index);
        }

        private void uxTree_AfterSelect(object sender,TreeViewEventArgs e)
        {
            if (activecd != null)
            {
                activecd.Hide();
                activecd = null;
            }
            if (uxTree.SelectedNode != null)
            {
                activecd = (ControllerData)uxTree.SelectedNode.Tag;
                if (activecd != null)
                {
                    activecd.Show();
                }
            }
            SyncUI();
        }

        private void commandmanager_CommandExecuted(object sender,EventArgs e)
        {
            SyncUI();
        }
    }
}
