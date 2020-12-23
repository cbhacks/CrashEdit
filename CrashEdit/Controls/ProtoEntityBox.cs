using Crash;
using System;
using System.Collections.Generic;
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
            UpdateStartPosition();
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

        private void UpdateStartPosition()
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
            numModeA.Value = entity.VecX;
            numModeB.Value = entity.VecY;
            numModeC.Value = entity.VecZ;
        }

        private void numA_ValueChanged(object sender, EventArgs e)
        {
            entity.Flags = (short)numFlags.Value;
        }

        private void numB_ValueChanged(object sender, EventArgs e)
        {
            entity.VecX = (short)numModeA.Value;
        }

        private void numC_ValueChanged(object sender, EventArgs e)
        {
            entity.VecY = (short)numModeB.Value;
        }

        private void numD_ValueChanged(object sender, EventArgs e)
        {
            entity.VecZ = (short)numModeC.Value;
        }

        private void UpdateCodeString()
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

                case 0x3C: lblCodeName.Text = "AsciC"; break;
                case 0x3D: lblCodeName.Text = "WinGC"; break;

                default:
                    lblCodeName.Text = "(Unknown)"; break;
            }
        }
    }
}
