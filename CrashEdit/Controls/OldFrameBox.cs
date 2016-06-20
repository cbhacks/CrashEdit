using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class OldFrameBox : UserControl
    {
        private OldFrameController controller;
        private OldFrame frame;

        private bool vertexdirty;
        private int vertexindex;

        public OldFrameBox(OldFrameController controller)
        {
            this.controller = controller;
            frame = controller.OldFrame;
            InitializeComponent();
            UpdateVertice();
            UpdateUnknown();
            UpdateFactor1();
            UpdateFactor2();
            UpdateFactorG();
            UpdateOffset();
            vertexindex = 0;
        }

        private void InvalidateNodes()
        {
            controller.InvalidateNode();
        }

        private void UpdateVertice()
        {
            vertexdirty = true;
            if (vertexindex >= frame.Vertices.Count)
            {
                vertexindex = frame.Vertices.Count - 1;
            }
            // Do not make this else if,
            // sometimes both will run.
            // (this is intentional)
            if (vertexindex < 0)
            {
                vertexindex = 0;
            }
            // Do not remove this either
            if (vertexindex >= frame.Vertices.Count)
            {
                lblVerticeIndex.Text = "-- / --";
                cmdPreviousVertice.Enabled = false;
                cmdNextVertice.Enabled = false;
                cmdInsertVertice.Enabled = false;
                cmdRemoveVertice.Enabled = false;
                lblX.Enabled = false;
                lblY.Enabled = false;
                lblZ.Enabled = false;
                numX.Enabled = false;
                numY.Enabled = false;
                numZ.Enabled = false;
                lblNX.Enabled = false;
                lblNY.Enabled = false;
                lblNZ.Enabled = false;
                numNX.Enabled = false;
                numNY.Enabled = false;
                numNZ.Enabled = false;
            }
            else
            {
                lblVerticeIndex.Text = string.Format("{0} / {1}",vertexindex + 1,frame.Vertices.Count);
                cmdPreviousVertice.Enabled = (vertexindex > 0);
                cmdNextVertice.Enabled = (vertexindex < frame.Vertices.Count - 1);
                cmdInsertVertice.Enabled = true;
                cmdRemoveVertice.Enabled = (frame.Vertices.Count > 1);
                lblX.Enabled = true;
                lblY.Enabled = true;
                lblZ.Enabled = true;
                numX.Enabled = true;
                numY.Enabled = true;
                numZ.Enabled = true;
                lblNX.Enabled = true;
                lblNY.Enabled = true;
                lblNZ.Enabled = true;
                numNX.Enabled = true;
                numNY.Enabled = true;
                numNZ.Enabled = true;
                numX.Value = frame.Vertices[vertexindex].X;
                numY.Value = frame.Vertices[vertexindex].Y;
                numZ.Value = frame.Vertices[vertexindex].Z;
                numNX.Value = frame.Vertices[vertexindex].NormalX;
                numNY.Value = frame.Vertices[vertexindex].NormalY;
                numNZ.Value = frame.Vertices[vertexindex].NormalZ;
            }
            vertexdirty = false;
        }

        private void cmdPreviousVertice_Click(object sender,EventArgs e)
        {
            vertexindex--;
            UpdateVertice();
        }

        private void cmdNextVertice_Click(object sender,EventArgs e)
        {
            vertexindex++;
            UpdateVertice();
        }

        private void cmdNextAndRemoveVertice_Click(object sender,EventArgs e)
        {
            vertexindex++;
            frame.Vertices.RemoveAt(vertexindex);
            InvalidateNodes();
            UpdateVertice();
        }

        private void cmdInsertVertice_Click(object sender,EventArgs e)
        {
            frame.Vertices.Insert(vertexindex, frame.Vertices[vertexindex]);
            InvalidateNodes();
            UpdateVertice();
        }

        private void cmdRemoveVertice_Click(object sender,EventArgs e)
        {
            frame.Vertices.RemoveAt(vertexindex);
            InvalidateNodes();
            UpdateVertice();
        }

        private void cmdAppendVertice_Click(object sender,EventArgs e)
        {
            vertexindex = frame.Vertices.Count;
            if (frame.Vertices.Count > 0)
            {
                frame.Vertices.Add(frame.Vertices[vertexindex - 1]);
            }
            else
            {
                frame.Vertices.Add(new OldFrameVertex(0,0,0,0,0,0));
            }
            InvalidateNodes();
            UpdateVertice();
        }

        private void numX_ValueChanged(object sender,EventArgs e)
        {
            if (!vertexdirty)
            {
                OldFrameVertex pos = frame.Vertices[vertexindex];
                frame.Vertices[vertexindex] = new OldFrameVertex((byte)numX.Value,pos.Y,pos.Z,pos.NormalX,pos.NormalY,pos.NormalZ);
            }
        }

        private void numY_ValueChanged(object sender,EventArgs e)
        {
            if (!vertexdirty)
            {
                OldFrameVertex pos = frame.Vertices[vertexindex];
                frame.Vertices[vertexindex] = new OldFrameVertex(pos.X, (byte)numY.Value, pos.Z, pos.NormalX, pos.NormalY, pos.NormalZ);
            }
        }

        private void numZ_ValueChanged(object sender,EventArgs e)
        {
            if (!vertexdirty)
            {
                OldFrameVertex pos = frame.Vertices[vertexindex];
                frame.Vertices[vertexindex] = new OldFrameVertex(pos.X, pos.Y, (byte)numZ.Value, pos.NormalX, pos.NormalY, pos.NormalZ);
            }
        }

        private void numXNormal_ValueChanged(object sender, EventArgs e)
        {
            if (!vertexdirty)
            {
                OldFrameVertex pos = frame.Vertices[vertexindex];
                frame.Vertices[vertexindex] = new OldFrameVertex(pos.X, pos.Y, pos.Z, (byte)numNX.Value, pos.NormalY, pos.NormalZ);
            }
        }

        private void numYNormal_ValueChanged(object sender, EventArgs e)
        {
            if (!vertexdirty)
            {
                OldFrameVertex pos = frame.Vertices[vertexindex];
                frame.Vertices[vertexindex] = new OldFrameVertex(pos.X, pos.Y, pos.Z, pos.NormalX, (byte)numNY.Value, pos.NormalZ);
            }
        }

        private void numZNormal_ValueChanged(object sender, EventArgs e)
        {
            if (!vertexdirty)
            {
                OldFrameVertex pos = frame.Vertices[vertexindex];
                frame.Vertices[vertexindex] = new OldFrameVertex(pos.X, pos.Y, pos.Z, pos.NormalX, pos.NormalY, (byte)numNZ.Value);
            }
        }

        private void UpdateUnknown()
        {
            numUnknown.Value = frame.Unknown;
        }

        private void numUnknown_ValueChanged(object sender,EventArgs e)
        {
            frame.Unknown = (short)numUnknown.Value;
        }

        private void UpdateFactor1()
        {
            numX1.Value = frame.X1;
            numY1.Value = frame.Y1;
            numZ1.Value = frame.Z1;
        }

        private void UpdateFactor2()
        {
            numX2.Value = frame.X2;
            numY2.Value = frame.Y2;
            numZ2.Value = frame.Z2;
        }

        private void UpdateFactorG()
        {
            numXG.Value = frame.XGlobal;
            numYG.Value = frame.YGlobal;
            numZG.Value = frame.ZGlobal;
        }

        private void numXOffset_ValueChanged(object sender, EventArgs e)
        {
            frame.XOffset = (int)numXOffset.Value;
        }

        private void numYOffset_ValueChanged(object sender, EventArgs e)
        {
            frame.YOffset = (int)numYOffset.Value;
        }

        private void numZOffset_ValueChanged(object sender, EventArgs e)
        {
            frame.ZOffset = (int)numZOffset.Value;
        }

        private void numX1_ValueChanged(object sender, EventArgs e)
        {
            frame.X1 = (int)numX1.Value;
        }

        private void numY1_ValueChanged(object sender, EventArgs e)
        {
            frame.Y1 = (int)numY1.Value;
        }

        private void numZ1_ValueChanged(object sender, EventArgs e)
        {
            frame.Z1 = (int)numZ1.Value;
        }

        private void numX2_ValueChanged(object sender, EventArgs e)
        {
            frame.X2 = (int)numX2.Value;
        }

        private void numY2_ValueChanged(object sender, EventArgs e)
        {
            frame.Y2 = (int)numY2.Value;
        }

        private void numZ2_ValueChanged(object sender, EventArgs e)
        {
            frame.Z2 = (int)numZ2.Value;
        }

        private void numXGlobal_ValueChanged(object sender, EventArgs e)
        {
            frame.XGlobal = (int)numXG.Value;
        }

        private void numYGlobal_ValueChanged(object sender, EventArgs e)
        {
            frame.YGlobal = (int)numYG.Value;
        }

        private void numZGlobal_ValueChanged(object sender, EventArgs e)
        {
            frame.ZGlobal = (int)numZG.Value;
        }

        private void UpdateOffset()
        {
            numXOffset.Value = frame.XOffset;
            numYOffset.Value = frame.YOffset;
            numZOffset.Value = frame.ZOffset;
        }
    }
}
