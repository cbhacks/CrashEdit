using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class OldCameraBox : UserControl
    {
        private OldCameraController controller;
        private OldCamera camera;

        private bool positiondirty;
        private int positionindex;

        public OldCameraBox(OldCameraController controller)
        {
            this.controller = controller;
            camera = controller.Camera;
            InitializeComponent();
            UpdatePosition();
            UpdateGeneral();
            UpdateNeighbor();
            UpdateDir();
            positionindex = 0;
        }

        private void InvalidateNodes()
        {
            controller.InvalidateNode();
        }

        private void UpdatePosition()
        {
            positiondirty = true;
            if (positionindex >= camera.Positions.Count)
            {
                positionindex = camera.Positions.Count - 1;
            }
            // Do not make this else if,
            // sometimes both will run.
            // (this is intentional)
            if (positionindex < 0)
            {
                positionindex = 0;
            }
            // Do not remove this either
            if (positionindex >= camera.Positions.Count)
            {
                lblPositionIndex.Text = "-- / --";
                cmdPreviousPosition.Enabled = false;
                cmdNextPosition.Enabled = false;
                cmdLastPosition.Enabled = false;
                cmdFirstPosition.Enabled = false;
                cmdInsertPosition.Enabled = false;
                cmdRemovePosition.Enabled = false;
                lblX.Enabled = false;
                lblY.Enabled = false;
                lblZ.Enabled = false;
                numX.Enabled = false;
                numY.Enabled = false;
                numZ.Enabled = false;
                lblXRot.Enabled = false;
                lblYRot.Enabled = false;
                lblZRot.Enabled = false;
                numXRot.Enabled = false;
                numYRot.Enabled = false;
                numZRot.Enabled = false;
            }
            else
            {
                lblPositionIndex.Text = string.Format("{0} / {1}",positionindex + 1,camera.Positions.Count);
                cmdPreviousPosition.Enabled = (positionindex > 0);
                cmdFirstPosition.Enabled = (positionindex > 0);
                cmdNextPosition.Enabled = (positionindex < camera.Positions.Count - 1);
                cmdLastPosition.Enabled = (positionindex < camera.Positions.Count - 1);
                cmdInsertPosition.Enabled = true;
                cmdRemovePosition.Enabled = (camera.Positions.Count > 1);
                lblX.Enabled = true;
                lblY.Enabled = true;
                lblZ.Enabled = true;
                numX.Enabled = true;
                numY.Enabled = true;
                numZ.Enabled = true;
                numX.Value = camera.Positions[positionindex].X;
                numY.Value = camera.Positions[positionindex].Y;
                numZ.Value = camera.Positions[positionindex].Z;
                lblXRot.Enabled = true;
                lblYRot.Enabled = true;
                lblZRot.Enabled = true;
                numXRot.Enabled = true;
                numYRot.Enabled = true;
                numZRot.Enabled = true;
                numXRot.Value = camera.Positions[positionindex].XRot;
                numYRot.Value = camera.Positions[positionindex].YRot;
                numZRot.Value = camera.Positions[positionindex].ZRot;
            }
            positiondirty = false;
        }

        private void cmdPreviousPosition_Click(object sender,EventArgs e)
        {
            positionindex--;
            UpdatePosition();
        }

        private void cmdNextPosition_Click(object sender,EventArgs e)
        {
            positionindex++;
            UpdatePosition();
        }

        private void cmdLastPosition_Click(object sender,EventArgs e)
        {
            positionindex = camera.Positions.Count - 1;
            UpdatePosition();
        }

        private void cmdNextAndRemovePosition_Click(object sender,EventArgs e)
        {
            positionindex++;
            camera.Positions.RemoveAt(positionindex);
            UpdatePosition();
        }

        private void cmdInsertPosition_Click(object sender,EventArgs e)
        {
            camera.Positions.Insert(positionindex,camera.Positions[positionindex]);
            UpdatePosition();
        }

        private void cmdRemovePosition_Click(object sender,EventArgs e)
        {
            camera.Positions.RemoveAt(positionindex);
            UpdatePosition();
        }

        private void cmdAppendPosition_Click(object sender,EventArgs e)
        {
            positionindex = camera.Positions.Count;
            if (camera.Positions.Count > 0)
            {
                camera.Positions.Add(camera.Positions[positionindex - 1]);
            }
            else
            {
                camera.Positions.Add(new OldCameraPosition(0,0,0,0,0,0));
            }
            UpdatePosition();
        }

        private void numX_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                OldCameraPosition pos = camera.Positions[positionindex];
                camera.Positions[positionindex] = new OldCameraPosition((short)numX.Value,pos.Y,pos.Z,pos.XRot,pos.YRot,pos.ZRot);
            }
        }

        private void numY_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                OldCameraPosition pos = camera.Positions[positionindex];
                camera.Positions[positionindex] = new OldCameraPosition(pos.X,(short)numY.Value,pos.Z,pos.XRot,pos.YRot,pos.ZRot);
            }
        }

        private void numZ_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                OldCameraPosition pos = camera.Positions[positionindex];
                camera.Positions[positionindex] = new OldCameraPosition(pos.X,pos.Y,(short)numZ.Value,pos.XRot,pos.YRot,pos.ZRot);
            }
        }

        private void numXRot_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                OldCameraPosition pos = camera.Positions[positionindex];
                camera.Positions[positionindex] = new OldCameraPosition(pos.X,pos.Y,pos.Z,(short)numXRot.Value,pos.YRot,pos.ZRot);
            }
        }

        private void numYRot_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                OldCameraPosition pos = camera.Positions[positionindex];
                camera.Positions[positionindex] = new OldCameraPosition(pos.X,pos.Y,pos.Z,pos.XRot,(short)numYRot.Value,pos.ZRot);
            }
        }

        private void numZRot_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                OldCameraPosition pos = camera.Positions[positionindex];
                camera.Positions[positionindex] = new OldCameraPosition(pos.X,pos.Y,pos.Z,pos.XRot,pos.YRot,(short)numYRot.Value);
            }
        }

        private void UpdateGeneral()
        {
            numAvgDist.Value = camera.AvgDist;
            numMode.Value = camera.Mode;
            numZoom.Value = camera.Zoom;
        }

        private void numMode_ValueChanged(object sender,EventArgs e)
        {
            camera.Mode = (short)numMode.Value;
        }

        private void numZoom_ValueChanged(object sender,EventArgs e)
        {
            camera.Zoom = (short)numZoom.Value;
        }

        private void UpdateNeighbor()
        {
            numNeighborCount.Value = camera.NeighborCount;
            numRelative1.Value = camera.Neighbors[0].LinkType;
            numRelative2.Value = camera.Neighbors[1].LinkType;
            numRelative3.Value = camera.Neighbors[2].LinkType;
            numRelative4.Value = camera.Neighbors[3].LinkType;
            numParentZone1.Value = camera.Neighbors[0].ZoneIndex;
            numParentZone2.Value = camera.Neighbors[1].ZoneIndex;
            numParentZone3.Value = camera.Neighbors[2].ZoneIndex;
            numParentZone4.Value = camera.Neighbors[3].ZoneIndex;
            numPathItem1.Value = camera.Neighbors[0].CameraIndex;
            numPathItem2.Value = camera.Neighbors[1].CameraIndex;
            numPathItem3.Value = camera.Neighbors[2].CameraIndex;
            numPathItem4.Value = camera.Neighbors[3].CameraIndex;
            numRelativeFlag1.Value = camera.Neighbors[0].Flag;
            numRelativeFlag2.Value = camera.Neighbors[1].Flag;
            numRelativeFlag3.Value = camera.Neighbors[2].Flag;
            numRelativeFlag4.Value = camera.Neighbors[3].Flag;
        }

        private void numNeighborCount_ValueChanged(object sender,EventArgs e)
        {
            camera.NeighborCount = (int)numNeighborCount.Value;
            fraNeighbor0.Enabled = camera.NeighborCount >= 1;
            fraNeighbor1.Enabled = camera.NeighborCount >= 2;
            fraNeighbor2.Enabled = camera.NeighborCount >= 3;
            fraNeighbor3.Enabled = camera.NeighborCount >= 4;
        }

        private void numRelative1_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[0].LinkType = (byte)numRelative1.Value;
        }

        private void numRelative2_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[1].LinkType = (byte)numRelative2.Value;
        }

        private void numRelative3_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[2].LinkType = (byte)numRelative3.Value;
        }

        private void numRelative4_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[3].LinkType = (byte)numRelative4.Value;
        }

        private void numParentZone1_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[0].ZoneIndex = (byte)numParentZone1.Value;
        }

        private void numParentZone2_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[1].ZoneIndex = (byte)numParentZone2.Value;
        }

        private void numParentZone3_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[2].ZoneIndex = (byte)numParentZone3.Value;
        }

        private void numParentZone4_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[3].ZoneIndex = (byte)numParentZone4.Value;
        }

        private void numPathItem1_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[0].CameraIndex = (byte)numPathItem1.Value;
        }

        private void numPathItem2_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[1].CameraIndex = (byte)numPathItem2.Value;
        }

        private void numPathItem3_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[2].CameraIndex = (byte)numPathItem3.Value;
        }

        private void numPathItem4_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[3].CameraIndex = (byte)numPathItem4.Value;
        }

        private void numRelativeFlag1_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[0].Flag = (byte)numRelativeFlag1.Value;
        }

        private void numRelativeFlag2_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[1].Flag = (byte)numRelativeFlag2.Value;
        }

        private void numRelativeFlag3_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[2].Flag = (byte)numRelativeFlag3.Value;
        }

        private void numRelativeFlag4_ValueChanged(object sender,EventArgs e)
        {
            camera.Neighbors[3].Flag = (byte)numRelativeFlag4.Value;
        }

        private void cmdFirstPosition_Click(object sender,EventArgs e)
        {
            positionindex = 0;
            UpdatePosition();
        }

        private void UpdateDir()
        {
            numXDir.Value = camera.XDir / 4096;
            numYDir.Value = camera.YDir / 4096;
            numZDir.Value = camera.ZDir / 4096;
        }

        private void numXDir_ValueChanged(object sender,EventArgs e)
        {
            camera.XDir = (short)(numXDir.Value * 4096);
        }

        private void numYDir_ValueChanged(object sender,EventArgs e)
        {
            camera.YDir = (short)(numYDir.Value * 4096);
        }

        private void numZDir_ValueChanged(object sender,EventArgs e)
        {
            camera.ZDir = (short)(numZDir.Value * 4096);
        }

        private void numAvgDist_ValueChanged(object sender,EventArgs e)
        {
            camera.AvgDist = (short)numAvgDist.Value;
        }

        private void cmdAvgDistCalc_Click(object sender, EventArgs e)
        {
            if (camera.Positions.Count <= 1)
            {
                numAvgDist.Value = 0;
                return;
            }

            double dist = 0;

            for (int i = 0; i < camera.Positions.Count; ++i)
            {
                if (i == 0) continue;
                var p1 = camera.Positions[i-1];
                var p2 = camera.Positions[i];
                dist += Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.Z - p1.Z, 2));
            }

            numAvgDist.Value = (short)(dist / (camera.Positions.Count-1));
        }
    }
}
