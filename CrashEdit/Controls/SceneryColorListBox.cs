using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class SceneryColorListBox : UserControl
    {
        private SceneryColorListController controller;
        private SceneryColorList colorlist;

        private bool colordirty;
        private int colorindex;

        public SceneryColorListBox(SceneryColorListController controller)
        {
            this.controller = controller;
            colorlist = controller.ColorList;
            InitializeComponent();
            UpdateColors();
            colorindex = 0;
        }

        private void InvalidateNodes()
        {
            controller.InvalidateNode();
        }

        private void UpdateColors()
        {
            colordirty = true;
            if (colorindex >= colorlist.Colors.Count)
            {
                colorindex = colorlist.Colors.Count - 1;
            }
            // Do not make this else if,
            // sometimes both will run.
            // (this is intentional)
            if (colorindex < 0)
            {
                colorindex = 0;
            }
            // Do not remove this either
            if (colorindex >= colorlist.Colors.Count)
            {
                lblColorIndex.Text = "-- / --";
                cmdPreviousColor.Enabled = false;
                cmdNextColor.Enabled = false;
                cmdInsertColor.Enabled = false;
                cmdRemoveColor.Enabled = false;
                lblRed.Enabled = false;
                lblGreen.Enabled = false;
                lblBlue.Enabled = false;
                numRed.Enabled = false;
                numGreen.Enabled = false;
                numBlue.Enabled = false;
            }
            else
            {
                lblColorIndex.Text = string.Format("{0} / {1}",colorindex + 1,colorlist.Colors.Count);
                cmdPreviousColor.Enabled = (colorindex > 0);
                cmdNextColor.Enabled = (colorindex < colorlist.Colors.Count - 1);
                cmdInsertColor.Enabled = true;
                cmdRemoveColor.Enabled = true;
                lblRed.Enabled = true;
                lblGreen.Enabled = true;
                lblBlue.Enabled = true;
                numRed.Enabled = true;
                numGreen.Enabled = true;
                numBlue.Enabled = true;
                numRed.Value = colorlist.Colors[colorindex].Red;
                numGreen.Value = colorlist.Colors[colorindex].Green;
                numBlue.Value = colorlist.Colors[colorindex].Blue;
            }
            colordirty = false;
        }

        private void cmdPreviousColor_Click(object sender,EventArgs e)
        {
            colorindex--;
            UpdateColors();
        }

        private void cmdNextColor_Click(object sender, EventArgs e)
        {
            colorindex++;
            UpdateColors();
        }

        private void cmdInsertColor_Click(object sender,EventArgs e)
        {
            colorlist.Colors.Insert(colorindex,colorlist.Colors[colorindex]);
            UpdateColors();
        }

        private void cmdRemoveColor_Click(object sender,EventArgs e)
        {
            colorlist.Colors.RemoveAt(colorindex);
            UpdateColors();
        }

        private void cmdAppendColor_Click(object sender,EventArgs e)
        {
            colorindex = colorlist.Colors.Count;
            if (colorlist.Colors.Count > 0)
            {
                colorlist.Colors.Add(colorlist.Colors[colorindex - 1]);
            }
            else
            {
                colorlist.Colors.Add(new SceneryColor(0,0,0,0));
            }
            UpdateColors();
        }

        private void numRed_ValueChanged(object sender,EventArgs e)
        {
            if (!colordirty)
            {
                SceneryColor pos = colorlist.Colors[colorindex];
                colorlist.Colors[colorindex] = new SceneryColor((byte)numRed.Value,pos.Green,pos.Blue);
            }
        }

        private void numGreen_ValueChanged(object sender,EventArgs e)
        {
            if (!colordirty)
            {
                SceneryColor pos = colorlist.Colors[colorindex];
                colorlist.Colors[colorindex] = new SceneryColor(pos.Red,(byte)numGreen.Value,pos.Blue);
            }
        }

        private void numBlue_ValueChanged(object sender,EventArgs e)
        {
            if (!colordirty)
            {
                SceneryColor pos = colorlist.Colors[colorindex];
                colorlist.Colors[colorindex] = new SceneryColor(pos.Red,pos.Green,(byte)numBlue.Value);
            }
        }
    }
}
