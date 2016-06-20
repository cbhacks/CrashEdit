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
            entity = controller.Entity;
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
            if (positionindex >= entity.Index.Count)
            {
                positionindex = entity.Index.Count - 1;
            }
            // Do not make this else if,
            // sometimes both will run.
            // (this is intentional)
            if (positionindex < 0)
            {
                positionindex = 0;
            }
            // Do not remove this either
            if (positionindex >= entity.Index.Count)
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
                lblPositionIndex.Text = string.Format("{0} / {1}",positionindex + 1,entity.Index.Count);
                cmdPreviousPosition.Enabled = (positionindex > 0);
                cmdNextPosition.Enabled = (positionindex < entity.Index.Count - 1);
                cmdInsertPosition.Enabled = true;
                cmdRemovePosition.Enabled = (entity.Index.Count > 1);
                lblX.Enabled = true;
                lblY.Enabled = true;
                lblZ.Enabled = true;
                numX.Enabled = true;
                numY.Enabled = true;
                numZ.Enabled = true;
                numX.Value = entity.Index[positionindex].X;
                numY.Value = entity.Index[positionindex].Y;
                numZ.Value = entity.Index[positionindex].Z;
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

        private void cmdNextAndRemovePosition_Click(object sender,EventArgs e)
        {
            positionindex++;
            entity.Index.RemoveAt(positionindex);
            InvalidateNodes();
            UpdatePosition();
        }

        private void cmdInsertPosition_Click(object sender,EventArgs e)
        {
            entity.Index.Insert(positionindex, entity.Index[positionindex]);
            InvalidateNodes();
            UpdatePosition();
        }

        private void cmdRemovePosition_Click(object sender,EventArgs e)
        {
            entity.Index.RemoveAt(positionindex);
            InvalidateNodes();
            UpdatePosition();
        }

        private void cmdAppendPosition_Click(object sender,EventArgs e)
        {
            positionindex = entity.Index.Count;
            if (entity.Index.Count > 0)
            {
                entity.Index.Add(entity.Index[positionindex - 1]);
            }
            else
            {
                entity.Index.Add(new EntityPosition(0,0,0));
            }
            InvalidateNodes();
            UpdatePosition();
        }

        private void numX_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                EntityPosition pos = entity.Index[positionindex];
                entity.Index[positionindex] = new EntityPosition((short)numX.Value,pos.Y,pos.Z);
            }
        }

        private void numY_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                EntityPosition pos = entity.Index[positionindex];
                entity.Index[positionindex] = new EntityPosition(pos.X,(short)numY.Value,pos.Z);
            }
        }

        private void numZ_ValueChanged(object sender,EventArgs e)
        {
            if (!positiondirty)
            {
                EntityPosition pos = entity.Index[positionindex];
                entity.Index[positionindex] = new EntityPosition(pos.X,pos.Y,(short)numZ.Value);
            }
        }

        private void UpdateID()
        {
            numID.Value = entity.ID.Value;
        }

        private void numID_ValueChanged(object sender,EventArgs e)
        {
            if (entity.ID.Value > -32768 && entity.ID.Value < 32767)
                entity.ID = (short)numID.Value;
            else
                throw new ArgumentOutOfRangeException("id");
        }

        private void UpdateType()
        {
            numType.Value = entity.Type.Value;
        }

        private void numType_ValueChanged(object sender,EventArgs e)
        {
            entity.Type = (byte)numType.Value;
            UpdateCodeString();
        }

        private void UpdateSubtype()
        {
            numSubtype.Value = entity.Subtype.Value;
        }

        private void numSubtype_ValueChanged(object sender,EventArgs e)
        {
            entity.Subtype = (byte)numSubtype.Value;
        }

        private void UpdateSettings()
        {
            numA.Value = entity.Unknown1.Value;
            numB.Value = entity.SettingA.Value;
            numC.Value = entity.SettingB.Value;
            numD.Value = entity.LinkID.Value;
        }

        private void numA_ValueChanged(object sender,EventArgs e)
        {
            if (numA.Value < numA.Maximum && numA.Value > numA.Minimum)
                entity.Unknown1 = (short)numA.Value;
            else
                entity.Unknown1 = 0;
        }

        private void numB_ValueChanged(object sender,EventArgs e)
        {
            if (numB.Value < numB.Maximum && numB.Value > numB.Minimum)
                entity.SettingA = (short)numB.Value;
            else
                entity.SettingA = 0;
        }

        private void numC_ValueChanged(object sender,EventArgs e)
        {
            if (numC.Value < numC.Maximum && numC.Value > numC.Minimum)
                entity.SettingB = (short)numC.Value;
            else
                entity.SettingB = 0;
        }

        private void numD_ValueChanged(object sender,EventArgs e)
        {
            if (numD.Value < numD.Maximum && numD.Value > numD.Minimum)
                entity.LinkID = (short)numD.Value;
            else
                entity.LinkID = 0;
        }

        private void UpdateCodeString()
        {
            switch (entity.Type)
            {
                case 0x0:
                    lblCodeName.Text = string.Format("WillC");
                    break;
                case 0x1:
                    lblCodeName.Text = string.Format("MonkC");
                    break;
                case 0x2:
                    lblCodeName.Text = string.Format("SkunC");
                    break;
                case 0x3:
                    lblCodeName.Text = string.Format("FruiC");
                    break;
                case 0x4:
                    lblCodeName.Text = string.Format("DispC");
                    break;
                case 0x5:
                    lblCodeName.Text = string.Format("DoctC");
                    break;
                case 0x6:
                    lblCodeName.Text = string.Format("SnakC");
                    break;
                case 0x7:
                    lblCodeName.Text = string.Format("WartC");
                    break;
                case 0x8:
                    lblCodeName.Text = string.Format("PoDoC");
                    break;
                case 0x9:
                    lblCodeName.Text = string.Format("PoRoC");
                    break;
                case 0xA:
                    lblCodeName.Text = string.Format("PoREC");
                    break;
                case 0xB:
                    lblCodeName.Text = string.Format("PoPlC");
                    break;
                case 0xC:
                    lblCodeName.Text = string.Format("MafiC");
                    break;
                case 0xD:
                    lblCodeName.Text = string.Format("Dog_C/PoCoC");
                    break;
                case 0xE:
                    lblCodeName.Text = string.Format("PoObC");
                    break;
                case 0xF:
                    lblCodeName.Text = string.Format("PinsC");
                    break;
                case 0x10:
                    lblCodeName.Text = string.Format("BaraC");
                    break;
                case 0x11:
                    lblCodeName.Text = string.Format("FatsC");
                    break;
                case 0x12:
                    lblCodeName.Text = string.Format("PinOC");
                    break;
                case 0x13:
                    lblCodeName.Text = string.Format("TurtC");
                    break;
                case 0x14:
                    lblCodeName.Text = string.Format("ChefC");
                    break;
                case 0x15:
                    lblCodeName.Text = string.Format("CheOC");
                    break;
                case 0x16:
                    lblCodeName.Text = string.Format("JunOC");
                    break;
                case 0x17:
                    lblCodeName.Text = string.Format("BridC");
                    break;
                case 0x18:
                    lblCodeName.Text = string.Format("HyeaC");
                    break;
                case 0x19:
                    lblCodeName.Text = string.Format("PlanC");
                    break;
                case 0x1A:
                    lblCodeName.Text = string.Format("CliOC");
                    break;
                case 0x1B:
                    lblCodeName.Text = string.Format("BeaOC");
                    break;
                case 0x1C:
                    lblCodeName.Text = string.Format("RivOC");
                    break;
                case 0x1D:
                    lblCodeName.Text = string.Format("ShadC");
                    break;
                case 0x1E:
                    lblCodeName.Text = string.Format("KonOC");
                    break;
                case 0x1F:
                    lblCodeName.Text = string.Format("CrabC");
                    break;
                case 0x20:
                    lblCodeName.Text = string.Format("WarpC");
                    break;
                case 0x21:
                    lblCodeName.Text = string.Format("WalOC");
                    break;
                case 0x22:
                    lblCodeName.Text = string.Format("BoxsC");
                    break;
                case 0x23:
                    lblCodeName.Text = string.Format("PillC");
                    break;
                case 0x24:
                    lblCodeName.Text = string.Format("frogC");
                    break;
                case 0x25:
                    lblCodeName.Text = string.Format("RRooC");
                    break;
                case 0x26:
                    lblCodeName.Text = string.Format("SheNC");
                    break;
                case 0x27:
                    lblCodeName.Text = string.Format("RooOC");
                    break;
                case 0x28:
                    lblCodeName.Text = string.Format("BrioC");
                    break;
                case 0x29:
                    lblCodeName.Text = string.Format("BriOC");
                    break;
                case 0x2A:
                    lblCodeName.Text = string.Format("RuiOC");
                    break;
                case 0x2B:
                    lblCodeName.Text = string.Format("SpidC");
                    break;
                case 0x2C:
                    lblCodeName.Text = string.Format("MapOC");
                    break;
                case 0x2D:
                    lblCodeName.Text = string.Format("KongC");
                    break;
                case 0x2E:

                    lblCodeName.Text = string.Format("RWaOC");
                    break;
                case 0x2F:
                    lblCodeName.Text = string.Format("LizaC");
                    break;
                case 0x30:
                    lblCodeName.Text = string.Format("Opt0C");
                    break;
                case 0x31:
                    lblCodeName.Text = string.Format("CortC");
                    break;
                case 0x32:
                    lblCodeName.Text = string.Format("CorOC");
                    break;
                case 0x33:
                    lblCodeName.Text = string.Format("VilOC");
                    break;
                case 0x34:
                    lblCodeName.Text = string.Format("GamOC");
                    break;
                case 0x35:
                    lblCodeName.Text = string.Format("CasOC");
                    break;
                case 0x36:
                    lblCodeName.Text = string.Format("LabAC");
                    break;
                case 0x37:
                    lblCodeName.Text = string.Format("WateC");
                    break;
                case 0x38:
                    lblCodeName.Text = string.Format("BonoC");
                    break;
                case 0x39:
                    lblCodeName.Text = string.Format("CardC");
                    break;
                case 0x3A:
                    lblCodeName.Text = string.Format("GemsC");
                    break;
                case 0x3C:
                    lblCodeName.Text = string.Format("AsciC");
                    break;
                case 0x3D:
                    lblCodeName.Text = string.Format("WinGC");
                    break;
                default:
                    lblCodeName.Text = string.Format("TBD");
                    break;
            }
        }
    }
}
