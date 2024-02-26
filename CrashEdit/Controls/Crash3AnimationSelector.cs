using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public partial class Crash3AnimationSelector : UserControl
    {
        private NSF nsf;
        private AnimationEntry anim;
        private Frame frame;
        private Control rewardcontrol;

        public Crash3AnimationSelector(NSF nsf, AnimationEntry anim, Frame frame = null)
        {
            this.nsf = nsf;
            this.anim = anim;
            this.frame = frame;
            rewardcontrol = null;

            Dock = DockStyle.Fill;
            InitializeComponent();

            if (Properties.Settings.Default.UseAnimLinks)
            {
                Program.LoadC3AnimLinks();
                if (Program.C3AnimLinks.ContainsKey(anim.EName))
                {
                    txtEName.Text = Program.C3AnimLinks[anim.EName];
                    OnKeyDown_Func(null, new KeyEventArgs(Keys.Enter));
                }
            }

            txtEName.KeyDown += new KeyEventHandler(OnKeyDown_Func);

            lblDesc.Text = Properties.Resources.Crash3AnimationSelector_Desc;

            // try to automatically find model by doing Bc10V -> Bc10G
            if (rewardcontrol == null)
            {
                txtEName.Text = anim.EName.Substring(0, 4) + 'G';
                OnKeyDown_Func(this, new KeyEventArgs(Keys.Enter));
            }
        }

        private void OnKeyDown_Func(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && rewardcontrol == null)
            {
                lblEIDErr.Text = Entry.CheckEIDErrors(txtEName.Text, false);
                if (lblEIDErr.Text == string.Empty)
                {
                    ModelEntry modelentry = nsf.GetEntry<ModelEntry>(txtEName.Text);
                    if (modelentry != null)
                    {
                        List<Frame> frames = frame == null ? new(anim.Frames) : new() { frame };
                        foreach (Frame f in frames)
                        {
                            if (f.Vertices.Count != modelentry.VertexCount)
                            {
                                lblEIDErr.Visible = true;
                                lblEIDErr.Text = "Invalid Model: wrong vertex count";
                                return;
                            }
                            else if (f.Temporals.Length < modelentry.GetFrameBitCount() + f.SpecialVertexCount * 8 * 3)
                            {
                                lblEIDErr.Visible = true;
                                lblEIDErr.Text = "Invalid Model: not enough data for decompression";
                                return;
                            }
                        }
                    }
                    else
                    {
                        lblEIDErr.Visible = true;
                        lblEIDErr.Text = "Model does not exist.";
                        return;
                    }
                    foreach (Control control in Controls)
                    {
                        control.Visible = control.Enabled = false;
                    }
                    if (modelentry != null)
                    {
                        TextureChunk[] texturechunks = new TextureChunk[modelentry.TPAGCount];
                        for (int i = 0; i < texturechunks.Length; ++i)
                        {
                            texturechunks[i] = nsf.GetEntry<TextureChunk>(BitConv.FromInt32(modelentry.Info, 0xC + i * 4));
                        }
                        if (frame == null)
                            rewardcontrol = new AnimationEntryViewer(nsf, anim.EID, -1, modelentry.EID);
                        else
                            rewardcontrol = new AnimationEntryViewer(nsf, anim.EID, anim.Frames.IndexOf(frame), modelentry.EID);
                        if (sender != null)
                        {
                            if (Program.C3AnimLinks.ContainsKey(anim.EName))
                            {
                                Program.C3AnimLinks[anim.EName] = modelentry.EName;
                            }
                            else
                            {
                                Program.C3AnimLinks.Add(anim.EName, modelentry.EName);
                            }
                            Program.SaveC3AnimLinks();
                        }
                    }
                    else
                    {
                        if (frame == null)
                            rewardcontrol = new AnimationEntryViewer(nsf, anim.EID, -1, modelentry.EID);
                        else
                            rewardcontrol = new AnimationEntryViewer(nsf, anim.EID, anim.Frames.IndexOf(frame), modelentry.EID);
                    }
                    rewardcontrol.Dock = DockStyle.Fill;
                    Controls.Add(rewardcontrol);
                }
                else
                {
                    lblEIDErr.Visible = true;
                }
            }
            else if (rewardcontrol != null && e.KeyCode == Keys.Enter)
            {
                Controls.Remove(rewardcontrol);
                rewardcontrol.Dispose();
                rewardcontrol = null;
                foreach (Control control in Controls)
                {
                    control.Visible = control.Enabled = true;
                }
                lblEIDErr.Text = Entry.CheckEIDErrors(txtEName.Text, false);
                lblEIDErr.Visible = lblEIDErr.Text != string.Empty;
            }
        }

        private void txtEName_TextChanged(object sender, EventArgs e)
        {
            lblEIDErr.Text = Entry.CheckEIDErrors(txtEName.Text, false);
            lblEIDErr.Visible = lblEIDErr.Text != string.Empty;
        }
    }
}
