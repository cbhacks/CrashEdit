using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class OldEntityBox : UserControl
    {
        private OldEntityController controller;
        private OldEntity entity;

        private bool positiondirty;
        private int positionindex;

        public OldEntityBox(OldEntityController controller)
        {
            this.controller = controller;
            entity = controller.OldEntity;
            InitializeComponent();
            UpdatePosition();
            UpdateID();
            UpdateType();
            UpdateSubtype();
            UpdateSettings();
            UpdateCodeString();
            positionindex = 0;
        }

        private void InvalidateNodes()
        {
            controller.InvalidateNode();
        }

        private void UpdatePosition()
        {
            positiondirty = true;
            if (positionindex >= entity.Positions.Count)
            {
                positionindex = entity.Positions.Count - 1;
            }
            // Do not make this else if,
            // sometimes both will run.
            // (this is intentional)
            if (positionindex < 0)
            {
                positionindex = 0;
            }
            // Do not remove this either
            if (positionindex >= entity.Positions.Count)
            {
                lblPositionIndex.Text = "-- / --";
                cmdPreviousPosition.Enabled = false;
                cmdNextPosition.Enabled = false;
                cmdInsertPosition.Enabled = false;
                cmdRemovePosition.Enabled = false;
                lblX.Enabled = false;
                lblY.Enabled = false;
                lblZ.Enabled = false;
                numX.Enabled = false;
                numY.Enabled = false;
                numZ.Enabled = false;
            }
            else
            {
                lblPositionIndex.Text = string.Format("{0} / {1}",positionindex + 1,entity.Positions.Count);
                cmdPreviousPosition.Enabled = (positionindex > 0);
                cmdNextPosition.Enabled = (positionindex < entity.Positions.Count - 1);
                cmdInsertPosition.Enabled = true;
                cmdRemovePosition.Enabled = (entity.Positions.Count > 1);
                lblX.Enabled = true;
                lblY.Enabled = true;
                lblZ.Enabled = true;
                numX.Enabled = true;
                numY.Enabled = true;
                numZ.Enabled = true;
                numX.Value = entity.Positions[positionindex].X;
                numY.Value = entity.Positions[positionindex].Y;
                numZ.Value = entity.Positions[positionindex].Z;
            }
            cmdInterpolate.Enabled = entity.Positions.Count >= 2;
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

        private void cmdNextAndRemovePosition_Click(object sender,EventArgs e)
        {
            positionindex++;
            entity.Positions.RemoveAt(positionindex);
            InvalidateNodes();
            UpdatePosition();
        }

        private void cmdInsertPosition_Click(object sender,EventArgs e)
        {
            entity.Positions.Insert(positionindex, entity.Positions[positionindex]);
            InvalidateNodes();
            UpdatePosition();
        }

        private void cmdRemovePosition_Click(object sender,EventArgs e)
        {
            entity.Positions.RemoveAt(positionindex);
            InvalidateNodes();
            UpdatePosition();
        }

        private void cmdAppendPosition_Click(object sender,EventArgs e)
        {
            positionindex = entity.Positions.Count;
            if (entity.Positions.Count > 0)
            {
                entity.Positions.Add(entity.Positions[positionindex - 1]);
            }
            else
            {
                entity.Positions.Add(new EntityPosition(0,0,0));
            }
            InvalidateNodes();
            UpdatePosition();
        }

        private void numX_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                EntityPosition pos = entity.Positions[positionindex];
                entity.Positions[positionindex] = new EntityPosition((short)numX.Value,pos.Y,pos.Z);
            }
        }

        private void numY_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                EntityPosition pos = entity.Positions[positionindex];
                entity.Positions[positionindex] = new EntityPosition(pos.X,(short)numY.Value,pos.Z);
            }
        }

        private void numZ_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                EntityPosition pos = entity.Positions[positionindex];
                entity.Positions[positionindex] = new EntityPosition(pos.X,pos.Y,(short)numZ.Value);
            }
        }

        private void UpdateID()
        {
            numID.Value = entity.ID;
        }

        private void numID_ValueChanged(object sender,EventArgs e)
        {
            entity.ID = (short)numID.Value;
        }

        private void UpdateType()
        {
            numType.Value = entity.Type;
        }

        private void numType_ValueChanged(object sender,EventArgs e)
        {
            entity.Type = (byte)numType.Value;
            UpdateCodeString();
        }

        private void UpdateSubtype()
        {
            numSubtype.Value = entity.Subtype;
        }

        private void numSubtype_ValueChanged(object sender,EventArgs e)
        {
            entity.Subtype = (byte)numSubtype.Value;
        }

        private void UpdateSettings()
        {
            numFlags.Value = entity.Flags;
            numA.Value = entity.VecX;
            numB.Value = entity.VecY;
            numC.Value = entity.VecZ;
            numSpawn.Value = entity.Spawn;
        }

        private void numUnknown_ValueChanged(object sender,EventArgs e)
        {
            entity.Flags = (short)numFlags.Value;
        }

        private void numA_ValueChanged(object sender,EventArgs e)
        {
            entity.VecX = (short)numA.Value;
        }

        private void numB_ValueChanged(object sender,EventArgs e)
        {
            entity.VecY = (short)numB.Value;
        }

        private void numC_ValueChanged(object sender,EventArgs e)
        {
            entity.VecZ = (short)numC.Value;
        }

        private void numSpawn_ValueChanged(object sender, EventArgs e)
        {
            entity.Spawn = (byte)numSpawn.Value;
        }

        private void UpdateCodeString() // TODO : use NSD gool map
        {
            switch (entity.Type)
            {
                case 0x0: lblCodeName.Text = "WillC"; break;
                case 0x1: lblCodeName.Text = "MonkC"; break;
                case 0x2: lblCodeName.Text = "SkunC"; break;
                case 0x3: lblCodeName.Text = "FruiC"; break;
                case 0x4: lblCodeName.Text = "DispC"; break;
                case 0x5: lblCodeName.Text = "DoctC"; break;
                case 0x6: lblCodeName.Text = "SnakC"; break;
                case 0x7: lblCodeName.Text = "WartC"; break;
                case 0x8: lblCodeName.Text = "PoDoC"; break;
                case 0x9: lblCodeName.Text = "PoRoC"; break;
                case 0xA: lblCodeName.Text = "PoREC"; break;
                case 0xB: lblCodeName.Text = "PoPlC"; break;
                case 0xC: lblCodeName.Text = "MafiC"; break;
                case 0xD: lblCodeName.Text = "Dog_C/PoCoC"; break;
                case 0xE: lblCodeName.Text = "PoObC"; break;
                case 0xF: lblCodeName.Text = "PinsC"; break;
                case 0x10: lblCodeName.Text = "BaraC"; break;
                case 0x11: lblCodeName.Text = "FatsC"; break;
                case 0x12: lblCodeName.Text = "PinOC"; break;
                case 0x13: lblCodeName.Text = "TurtC"; break;
                case 0x14: lblCodeName.Text = "ChefC"; break;
                case 0x15: lblCodeName.Text = "CheOC"; break;
                case 0x16: lblCodeName.Text = "JunOC"; break;
                case 0x17: lblCodeName.Text = "BridC"; break;
                case 0x18: lblCodeName.Text = "HyeaC"; break;
                case 0x19: lblCodeName.Text = "PlanC"; break;
                case 0x1A: lblCodeName.Text = "CliOC"; break;
                case 0x1B: lblCodeName.Text = "BeaOC"; break;
                case 0x1C: lblCodeName.Text = "RivOC"; break;
                case 0x1D: lblCodeName.Text = "ShadC"; break;
                case 0x1E: lblCodeName.Text = "KonOC"; break;
                case 0x1F: lblCodeName.Text = "CrabC"; break;
                case 0x20: lblCodeName.Text = "WarpC"; break;
                case 0x21: lblCodeName.Text = "WalOC"; break;
                case 0x22: lblCodeName.Text = "BoxsC"; break;
                case 0x23: lblCodeName.Text = "PillC"; break;
                case 0x24: lblCodeName.Text = "frogC"; break;
                case 0x25: lblCodeName.Text = "RRooC"; break;
                case 0x26: lblCodeName.Text = "SheNC"; break;
                case 0x27: lblCodeName.Text = "RooOC"; break;
                case 0x28: lblCodeName.Text = "BrioC"; break;
                case 0x29: lblCodeName.Text = "BriOC"; break;
                case 0x2A: lblCodeName.Text = "RuiOC"; break;
                case 0x2B: lblCodeName.Text = "SpidC"; break;
                case 0x2C: lblCodeName.Text = "MapOC"; break;
                case 0x2D: lblCodeName.Text = "KongC"; break;
                case 0x2E: lblCodeName.Text = "RWaOC"; break;
                case 0x2F: lblCodeName.Text = "LizaC"; break;
                case 0x30: lblCodeName.Text = "Opt0C"; break;
                case 0x31: lblCodeName.Text = "CortC"; break;
                case 0x32: lblCodeName.Text = "CorOC"; break;
                case 0x33: lblCodeName.Text = "VilOC"; break;
                case 0x34: lblCodeName.Text = "GamOC"; break;
                case 0x35: lblCodeName.Text = "CasOC"; break;
                case 0x36: lblCodeName.Text = "LabAC"; break;
                case 0x37: lblCodeName.Text = "WateC"; break;
                case 0x38: lblCodeName.Text = "BonoC"; break;
                case 0x39: lblCodeName.Text = "CardC"; break;
                case 0x3A: lblCodeName.Text = "GemsC"; break;
                case 0x3B: lblCodeName.Text = "IsldC"; break;
                case 0x3C: lblCodeName.Text = "AsciC"; break;
                case 0x3D: lblCodeName.Text = "WinGC"; break;

                default:
                    lblCodeName.Text = "(Unknown)"; break;
            }
        }

        private void chkHexUnknown_CheckedChanged(object sender, EventArgs e)
        {
            numFlags.Hexadecimal = chkHexFlags.Checked;
        }

        private void chkHexA_CheckedChanged(object sender, EventArgs e)
        {
            numA.Hexadecimal = chkHexA.Checked;
        }

        private void chkHexB_CheckedChanged(object sender, EventArgs e)
        {
            numB.Hexadecimal = chkHexB.Checked;
        }

        private void chkHexC_CheckedChanged(object sender, EventArgs e)
        {
            numC.Hexadecimal = chkHexC.Checked;
        }

        private void cmdInterpolate_Click(object sender, EventArgs e)
        {
            Position[] pos = new Position[entity.Positions.Count];
            for (int i = 0; i < entity.Positions.Count; ++i)
            {
                pos[i] = new Position(entity.Positions[i].X, entity.Positions[i].Y, entity.Positions[i].Z);
            }
            using (InterpolatorForm interpolator = new InterpolatorForm(pos))
            {
                if (interpolator.ShowDialog() == DialogResult.OK)
                {
                    for (int m = interpolator.Start-1, i = interpolator.End-2; i > m; --i)
                    {
                        entity.Positions.RemoveAt(i);
                    }
                    for (int i = 0; i < interpolator.Amount; ++i)
                    {
                        entity.Positions.Insert(i+interpolator.Start,new EntityPosition(interpolator.NewPositions[i+1]));
                    }
                    UpdatePosition();
                    InvalidateNodes();
                }
            }
        }
    }
}
