using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class ProtoEntityBox : UserControl
    {
        private ProtoEntityController controller;
        private ProtoEntity entity;

        public ProtoEntityBox(ProtoEntityController controller)
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
        }

        private void InvalidateNodes()
        {
            controller.InvalidateNode();
        }

        private void UpdatePosition()
        {
            numX.Value = entity.StartX;
            numY.Value = entity.StartY;
            numZ.Value = entity.StartZ;
        }

        private void numX_ValueChanged(object sender,EventArgs e)
        {
            entity.StartX = (short)numX.Value;
        }

        private void numY_ValueChanged(object sender,EventArgs e)
        {
            entity.StartY = (short)numY.Value;
        }

        private void numZ_ValueChanged(object sender,EventArgs e)
        {
            entity.StartZ = (short)numZ.Value;
        }

        private void UpdateID()
        {
            numID.Value = entity.ID;
        }

        private void numID_ValueChanged(object sender,EventArgs e)
        {
            if (entity.ID > -32768 && entity.ID < 32767)
                entity.ID = (short)numID.Value;
            else
                throw new ArgumentOutOfRangeException("id");
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
            numA.Value = entity.SettingA;
            numB.Value = entity.SettingB;
            numC.Value = entity.SettingC;
            numD.Value = entity.SettingD;
        }

        private void numA_ValueChanged(object sender, EventArgs e)
        {
            if (numA.Value < numA.Maximum && numA.Value > numA.Minimum)
                entity.SettingA = (short)numA.Value;
            else
                entity.SettingA = 0;
        }

        private void numB_ValueChanged(object sender, EventArgs e)
        {
            if (numB.Value < numB.Maximum && numB.Value > numB.Minimum)
                entity.SettingB = (short)numB.Value;
            else
                entity.SettingB = 0;
        }

        private void numC_ValueChanged(object sender, EventArgs e)
        {
            if (numC.Value < numC.Maximum && numC.Value > numC.Minimum)
                entity.SettingC = (short)numC.Value;
            else
                entity.SettingC = 0;
        }

        private void numD_ValueChanged(object sender, EventArgs e)
        {
            if (numD.Value < numD.Maximum && numD.Value > numD.Minimum)
                entity.SettingD = (short)numD.Value;
            else
                entity.SettingD = 0;
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
                    lblCodeName.Text = string.Format("Unavailable");
                    break;
            }
        }
    }
}
