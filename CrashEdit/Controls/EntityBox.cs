using Crash;
using CrashEdit.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class EntityBox : UserControl
    {
        private Controller controller;
        private Entity entity;

        private int positionindex;
        private int settingindex;
        private int victimindex;
        private int loadlistarowindex;
        private int loadlistaeidindex;
        private int loadlistbrowindex;
        private int loadlistbeidindex;
        private int drawlistarowindex;
        private int drawlistaentityindex;
        private int drawlistbrowindex;
        private int drawlistbentityindex;
        private int neighborindex;
        private int neighborsettingindex;
        private int fovframeindex;
        private int fovindex;

        private Timer argtexttimer;

        internal Stack<bool> dirty = new Stack<bool>();
        internal bool Dirty => dirty.Count > 0 && dirty.Peek();

        internal void MainInit()
        {
            InitializeComponent();
            UpdateName();
            UpdatePosition();
            UpdateType();
            UpdateSubtype();
            UpdateSettings();
            UpdateID();
            positionindex = 0;
            victimindex = 0;
            loadlistarowindex = 0;
            loadlistaeidindex = 0;
            loadlistbrowindex = 0;
            loadlistbeidindex = 0;
            drawlistarowindex = 0;
            drawlistaentityindex = 0;
            drawlistbrowindex = 0;
            drawlistbentityindex = 0;
            neighborindex = 0;
            neighborsettingindex = 0;
            fovframeindex = 0;
            fovindex = 0;
            tabGeneral.Text = Resources.EntityBox_TabGeneral;
            tabSpecial.Text = Resources.EntityBox_TabSpecial;
            tabCamera.Text = Resources.EntityBox_TabCamera;
            tabLoadLists.Text = Resources.EntityBox_TabLoadList;
            tabDrawLists.Text = Resources.EntityBox_TabDrawList;
            fraName.Text = Resources.EntityBox_FraName;
            foreach (CheckBox chk in this.GetAll(typeof(CheckBox)))
            {
                if (chk.Text == "Enabled")
                {
                    chk.Text = Resources.EntityBox_ChkEnabled;
                }
            }
            chkBonusBoxCount.Text = Resources.EntityBox_ChkBonusBoxCount;
            fraID.Text = Resources.EntityBox_FraID;
            fraType.Text = Resources.EntityBox_FraType;
            fraPosition.Text = Resources.EntityBox_FraPosition;
            fraSettings.Text = Resources.EntityBox_FraSettings;
            foreach (Button cmd in this.GetAll(typeof(Button)))
            {
                if (cmd.Text == "Previous")
                {
                    cmd.Text = Resources.EntityBox_CmdPrevious;
                }
                else if (cmd.Text == "Next")
                {
                    cmd.Text = Resources.EntityBox_CmdNext;
                }
                else if (cmd.Text == "Remove")
                {
                    cmd.Text = Resources.EntityBox_CmdRemove;
                }
                else if (cmd.Text == "Insert")
                {
                    cmd.Text = Resources.EntityBox_CmdInsert;
                }
                else if (cmd.Text == "Append")
                {
                    cmd.Text = Resources.EntityBox_CmdAppend;
                }
                else if (cmd.Text == "Add")
                {
                    cmd.Text = Resources.EntityBox_CmdAdd;
                }
            }
            chkSettingHex.Text = Resources.EntityBox_ChkHex;
            cmdInterpolate.Text = Resources.EntityBox_CmdInterpolate;
            fraVictims.Text = Resources.EntityBox_FraVictims;
            fraBoxCount.Text = Resources.EntityBox_FraBoxCount;
            fraDDASection.Text = Resources.EntityBox_FraDDASection;
            fraDDASettings.Text = Resources.EntityBox_FraDDASettings;
            fraOtherSettings.Text = Resources.EntityBox_FraOtherSettings;
            fraZMod.Text = Resources.EntityBox_FraZMod;
            fraScaling.Text = Resources.EntityBox_FraScaling;
            fraTTReward.Text = Resources.EntityBox_FraTTReward;
            fraSLST.Text = Resources.EntityBox_FraSLST;
            cmdClearAllVictims.Text = Resources.EntityBox_cmdClearAllVictims;
            fraMode.Text = Resources.EntityBox_FraMode;
            fraAvgDist.Text = Resources.EntityBox_fraAvgDist;
            fraCameraIndex.Text = Resources.EntityBox_fraCameraIndex;
            fraCameraSubIndex.Text = Resources.EntityBox_fraCameraSubIndex;
            fraNeighbor.Text = Resources.EntityBox_fraNeighbor;
            lblNeighborPosition.Text = Resources.EntityBox_lblNeighborPosition;
            fraNeighborSetting.Text = Resources.EntityBox_fraNeighborSetting;
            lblNeighborCamera.Text = Resources.EntityBox_lblNeighborCamera;
            lblNeighborFlag.Text = Resources.EntityBox_lblNeighborFlag;
            lblNeighborLink.Text = Resources.EntityBox_lblNeighborLink;
            lblNeighborZone.Text = Resources.EntityBox_lblNeighborZone;
            fraFOV.Text = Resources.EntityBox_fraFOV;
            lblFOVPosition.Text = Resources.EntityBox_lblFOVPosition;
            fraFOVFrame.Text = Resources.EntityBox_fraFOVFrame;
            fraLoadListA.Text = Resources.EntityBox_fraLoadListA;
            fraLoadListB.Text = Resources.EntityBox_fraLoadListB;
            lblMetavalueLoadA.Text = Resources.EntityBox_lblMetavalueLoadA;
            lblMetavalueLoadB.Text = Resources.EntityBox_lblMetavalueLoadB;
            fraEIDA.Text = Resources.EntityBox_fraEIDA;
            fraEIDB.Text = Resources.EntityBox_fraEIDB;
            fraLoadListPayload.Text = Resources.EntityBox_fraLoadListPayload;
            cmdLoadListVerify.Text = Resources.EntityBox_cmdLoadListVerify;
            lblPayloadPosition.Text = Resources.EntityBox_lblPayloadPosition;
            cmdPayload.Text = Resources.EntityBox_cmdPayload;
            fraDrawListA.Text = Resources.EntityBox_fraDrawListA;
            fraDrawListB.Text = Resources.EntityBox_fraDrawListB;
            lblMetavalueDrawA.Text = Resources.EntityBox_lblMetavalueDrawA;
            lblMetavalueDrawB.Text = Resources.EntityBox_lblMetavalueDrawB;
            fraEntityA.Text = Resources.EntityBox_fraEntityA;
            fraEntityB.Text = Resources.EntityBox_fraEntityB;
            lblArgAs.Text = MakeArgAsText();
            chkSettingHex_CheckedChanged(null, null);

            // use a Timer because of PAL switch
            argtexttimer = new Timer()
            {
                Enabled = true,
                Interval = 40
            };
            argtexttimer.Tick += (object sender, EventArgs e) =>
            {
                lblArgAs.Text = MakeArgAsText();
            };
        }

        public EntityBox(NewEntityController controller)
        {
            this.controller = controller;
            entity = controller.Entity;
            MainInit();
        }

        public EntityBox(EntityController controller)
        {
            this.controller = controller;
            entity = controller.Entity;
            MainInit();
        }

        private void InvalidateNodes()
        {
            controller.InvalidateNode();
        }

        internal string MakeArgAsText()
        {
            int arg = entity.Settings.Count > 0 ? entity.Settings[settingindex].Value : 0;
            return string.Format(Resources.EntityBox_lblArgAs,
                arg / 256F,
                arg / (float)0x1000 * 360,
                arg / (OldMainForm.PAL ? 25F : 30F),
                arg / (256F*400));
        }

        private void UpdateName()
        {
            if (entity.Name != null)
            {
                txtName.Text = entity.Name;
                chkName.Checked = true;
            }
            else
            {
                txtName.Enabled = false;
                chkName.Checked = false;
            }
        }

        private void chkName_CheckedChanged(object sender,EventArgs e)
        {
            txtName.Enabled = chkName.Checked;
            entity.Name = chkName.Checked ? txtName.Text : null;
            InvalidateNodes();
        }

        private void txtName_TextChanged(object sender,EventArgs e)
        {
            entity.Name = txtName.Text;
            InvalidateNodes();
        }

        private void UpdatePosition()
        {
            dirty.Push(true);
            if (positionindex >= entity.Positions.Count)
            {
                positionindex = entity.Positions.Count-1;
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
                cmdPreviousPosition.Enabled =
                cmdNextPosition.Enabled =
                cmdInsertPosition.Enabled =
                cmdRemovePosition.Enabled =
                cmdInterpolate.Enabled = false;
                lblX.Enabled = lblY.Enabled = lblZ.Enabled = numX.Enabled = numY.Enabled = numZ.Enabled = false;
            }
            else
            {
                lblPositionIndex.Text = $"{positionindex+1} / {entity.Positions.Count}";
                cmdPreviousPosition.Enabled = positionindex > 0;
                cmdNextPosition.Enabled = positionindex < entity.Positions.Count-1;
                cmdInsertPosition.Enabled = true;
                cmdRemovePosition.Enabled = true;
                lblX.Enabled = lblY.Enabled = lblZ.Enabled = numX.Enabled = numY.Enabled = numZ.Enabled = true;
                numX.Value = entity.Positions[positionindex].X;
                numY.Value = entity.Positions[positionindex].Y;
                numZ.Value = entity.Positions[positionindex].Z;
                cmdInterpolate.Enabled = entity.Positions.Count >= 2;
            }
            dirty.Pop();
        }

        private void cmdPreviousPosition_Click(object sender,EventArgs e)
        {
            --positionindex;
            UpdatePosition();
        }

        private void cmdNextPosition_Click(object sender,EventArgs e)
        {
            ++positionindex;
            UpdatePosition();
        }

        private void cmdInsertPosition_Click(object sender,EventArgs e)
        {
            entity.Positions.Insert(positionindex,entity.Positions[positionindex]);
            UpdatePosition();
        }

        private void cmdRemovePosition_Click(object sender,EventArgs e)
        {
            entity.Positions.RemoveAt(positionindex);
            UpdatePosition();
            if (entity.Positions.Count == 0)
                InvalidateNodes();
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
            UpdatePosition();
            if (entity.Positions.Count == 1)
                InvalidateNodes();
        }

        private void numX_ValueChanged(object sender,EventArgs e)
        {
            if (!Dirty)
            {
                EntityPosition pos = entity.Positions[positionindex];
                entity.Positions[positionindex] = new EntityPosition((short)numX.Value,pos.Y,pos.Z);
            }
        }

        private void numY_ValueChanged(object sender,EventArgs e)
        {
            if (!Dirty)
            {
                EntityPosition pos = entity.Positions[positionindex];
                entity.Positions[positionindex] = new EntityPosition(pos.X,(short)numY.Value,pos.Z);
            }
        }

        private void numZ_ValueChanged(object sender,EventArgs e)
        {
            if (!Dirty)
            {
                EntityPosition pos = entity.Positions[positionindex];
                entity.Positions[positionindex] = new EntityPosition(pos.X,pos.Y,(short)numZ.Value);
            }
        }

        private void UpdateSettings()
        {
            dirty.Push(true);
            if (settingindex >= entity.Settings.Count)
            {
                settingindex = entity.Settings.Count - 1;
            }
            // Do not make this else if,
            // sometimes both will run.
            // (this is intentional)
            if (settingindex < 0)
            {
                settingindex = 0;
            }
            // Do not remove this either
            if (settingindex >= entity.Settings.Count)
            {
                lblSettingIndex.Text = "-- / --";
                lblArgAs.Enabled =
                cmdPreviousSetting.Enabled =
                cmdNextSetting.Enabled =
                cmdRemoveSetting.Enabled =
                numSettingA.Enabled =
                numSettingB.Enabled =
                numSettingC.Enabled = false;
            }
            else
            {
                lblSettingIndex.Text = $"{settingindex+1} / {entity.Settings.Count}";
                lblArgAs.Text = MakeArgAsText();
                cmdPreviousSetting.Enabled = settingindex > 0;
                cmdNextSetting.Enabled = settingindex < entity.Settings.Count-1;
                cmdRemoveSetting.Enabled =
                lblArgAs.Enabled =
                numSettingA.Enabled =
                numSettingB.Enabled =
                numSettingC.Enabled = true;
                numSettingA.Value = entity.Settings[settingindex].ValueA;
                numSettingB.Value = entity.Settings[settingindex].ValueB;
                SetCVal(entity.Settings[settingindex].Value);
            }
            dirty.Pop();
        }

        private void cmdPreviousSetting_Click(object sender,EventArgs e)
        {
            --settingindex;
            UpdateSettings();
        }

        private void cmdNextSetting_Click(object sender,EventArgs e)
        {
            ++settingindex;
            UpdateSettings();
        }

        private void cmdAddSetting_Click(object sender,EventArgs e)
        {
            entity.Settings.Add(new EntitySetting(0,0));
            UpdateSettings();
        }

        private void cmdRemoveSetting_Click(object sender,EventArgs e)
        {
            entity.Settings.RemoveAt(settingindex);
            UpdateSettings();
        }

        private void numSettingA_ValueChanged(object sender,EventArgs e)
        {
            if (!Dirty)
            {
                EntitySetting s = entity.Settings[settingindex];
                entity.Settings[settingindex] = new EntitySetting((byte)numSettingA.Value,s.ValueB);
                SetCVal(entity.Settings[settingindex].Value);
                lblArgAs.Text = MakeArgAsText();
            }
        }

        private void numSettingB_ValueChanged(object sender,EventArgs e)
        {
            if (!Dirty)
            {
                EntitySetting s = entity.Settings[settingindex];
                entity.Settings[settingindex] = new EntitySetting(s.ValueA,(int)numSettingB.Value);
                SetCVal(entity.Settings[settingindex].Value);
                lblArgAs.Text = MakeArgAsText();
            }
        }

        internal void SetCVal(long val)
        {
            dirty.Push(true);
            // this is fucking stupid
            if (numSettingC.Hexadecimal)
            {
                if (val > 0xFFFFFFFF) val = 0xFFFFFFFF;
                else if (val < 0) val &= 0xFFFFFFFF;
                numSettingC.Value = unchecked((uint)val);
            }
            else
            {
                if (val > 0xFFFFFFFF) val = 0x7FFFFFFF;
                else if (val > 0x7FFFFFFF) val = -0x100000000 + val;
                else if (val < -0x80000000) val = -0x80000000;
                numSettingC.Value = unchecked((int)val);
            }
            dirty.Pop();
        }

        private void numSettingC_ValueChanged(object sender, EventArgs e)
        {
            if (!Dirty)
            {
                SetCVal((long)numSettingC.Value);
                entity.Settings[settingindex] = new EntitySetting(((long)numSettingC.Value).UInt32ToInt32());
                dirty.Push(true);
                numSettingA.Value = entity.Settings[settingindex].ValueA;
                numSettingB.Value = entity.Settings[settingindex].ValueB;
                dirty.Pop();
                lblArgAs.Text = MakeArgAsText();
            }
        }

        private void UpdateID()
        {
            if (entity.ID.HasValue)
            {
                numID.Value = entity.ID.Value;
                if (entity.AlternateID.HasValue)
                {
                    numID2.Value = entity.AlternateID.Value;
                }
                numID2.Enabled = entity.AlternateID.HasValue;
                chkID2.Checked = entity.AlternateID.HasValue;
            }
            else
            {
                numID2.Enabled = false;
            }
            numID.Enabled = entity.ID.HasValue;
            chkID.Checked = entity.ID.HasValue;
            chkID2.Enabled = entity.ID.HasValue;
        }

        private void chkID_CheckedChanged(object sender,EventArgs e)
        {
            numID.Enabled = chkID.Checked;
            chkID2.Enabled = chkID.Checked;
            if (chkID.Checked)
            {
                entity.ID = (int)numID.Value;
            }
            else
            {
                chkID2.Checked = false;
                entity.ID = null;
            }
            InvalidateNodes();
        }

        private void numID_ValueChanged(object sender,EventArgs e)
        {
            entity.ID = (int)numID.Value;
            InvalidateNodes();
        }

        private void chkID2_CheckedChanged(object sender,EventArgs e)
        {
            numID2.Enabled = chkID2.Checked;
            if (chkID2.Checked)
            {
                entity.AlternateID = (int)numID2.Value;
            }
            else
            {
                entity.AlternateID = null;
            }
        }

        private void numID2_ValueChanged(object sender,EventArgs e)
        {
            entity.AlternateID = (int)numID2.Value;
        }

        private void UpdateType()
        {
            if (entity.Type.HasValue)
            {
                numType.Value = entity.Type.Value;
            }
            numType.Enabled = entity.Type.HasValue;
            chkType.Checked = entity.Type.HasValue;
        }

        private void chkType_CheckedChanged(object sender,EventArgs e)
        {
            numType.Enabled = chkType.Checked;
            if (chkType.Checked)
            {
                entity.Type = (int)numType.Value;
            }
            else
            {
                entity.Type = null;
            }
        }

        private void numType_ValueChanged(object sender,EventArgs e)
        {
            entity.Type = (int)numType.Value;
        }

        private void UpdateSubtype()
        {
            if (entity.Subtype.HasValue)
            {
                numSubtype.Value = entity.Subtype.Value;
            }
            numSubtype.Enabled = entity.Subtype.HasValue;
            chkSubtype.Checked = entity.Subtype.HasValue;
        }

        private void chkSubtype_CheckedChanged(object sender,EventArgs e)
        {
            numSubtype.Enabled = chkSubtype.Checked;
            if (chkSubtype.Checked)
            {
                entity.Subtype = (int)numSubtype.Value;
            }
            else
            {
                entity.Subtype = null;
            }
        }

        private void numSubtype_ValueChanged(object sender,EventArgs e)
        {
            entity.Subtype = (int)numSubtype.Value;
        }

        private void UpdateBoxCount()
        {
            if (entity.BoxCount.HasValue)
            {
                numBoxCount.Value = entity.BoxCount.Value.ValueB;
            }
            numBoxCount.Enabled = entity.BoxCount.HasValue;
            chkBoxCount.Checked = entity.BoxCount.HasValue;
            if (entity.BonusBoxCount.HasValue)
            {
                numBonusBoxCount.Value = entity.BonusBoxCount.Value.ValueB;
            }
            numBonusBoxCount.Enabled = entity.BonusBoxCount.HasValue;
            chkBonusBoxCount.Checked = entity.BonusBoxCount.HasValue;
        }

        private void chkBoxCount_CheckedChanged(object sender,EventArgs e)
        {
            numBoxCount.Enabled = chkBoxCount.Checked;
            if (chkBoxCount.Checked)
            {
                entity.BoxCount = new EntitySetting(0,(int)numBoxCount.Value);
            }
            else
            {
                entity.BoxCount = null;
            }
        }

        private void numBoxCount_ValueChanged(object sender,EventArgs e)
        {
            entity.BoxCount = new EntitySetting(0,(int)numBoxCount.Value);
        }

        private void chkBonusBoxCount_CheckedChanged(object sender, EventArgs e)
        {
            numBonusBoxCount.Enabled = chkBonusBoxCount.Checked;
            if (chkBonusBoxCount.Checked)
            {
                entity.BonusBoxCount = new EntitySetting(0, (int)numBonusBoxCount.Value);
            }
            else
            {
                entity.BonusBoxCount = null;
            }
        }

        private void numBonusBoxCount_ValueChanged(object sender, EventArgs e)
        {
            entity.BonusBoxCount = new EntitySetting(0, (int)numBonusBoxCount.Value);
        }

        private void cmdClearAllVictims_Click(object sender,EventArgs e)
        {
            entity.Victims.Clear();
            UpdateVictim();
        }

        private void UpdateVictim()
        {
            dirty.Push(true);
            if (victimindex >= entity.Victims.Count)
            {
                victimindex = entity.Victims.Count-1;
            }
            // Do not make this else if,
            // sometimes both will run.
            // (this is intentional)
            if (victimindex < 0)
            {
                victimindex = 0;
            }
            if (victimindex >= entity.Victims.Count)
            {
                lblVictimIndex.Text = "-- / --";
                cmdPreviousVictim.Enabled =
                cmdNextVictim.Enabled =
                cmdRemoveVictim.Enabled =
                numVictimID.Enabled =
                cmdClearAllVictims.Enabled = false;
            }
            else
            {
                lblVictimIndex.Text = $"{victimindex+1} / {entity.Victims.Count}";
                cmdPreviousVictim.Enabled = victimindex > 0;
                cmdNextVictim.Enabled = victimindex < entity.Victims.Count-1;
                cmdRemoveVictim.Enabled =
                numVictimID.Enabled =
                cmdClearAllVictims.Enabled = true;
                numVictimID.Value = entity.Victims[victimindex].VictimID;
            }
            dirty.Pop();
        }

        private void cmdPreviousVictim_Click(object sender,EventArgs e)
        {
            --victimindex;
            UpdateVictim();
        }

        private void cmdNextVictim_Click(object sender,EventArgs e)
        {
            ++victimindex;
            UpdateVictim();
        }

        private void cmdInsertVictim_Click(object sender,EventArgs e)
        {
            if (entity.Victims.Count > 0)
            {
                entity.Victims.Insert(victimindex,entity.Victims[victimindex]);
            }
            else
            {
                victimindex = 0;
                entity.Victims.Add(new EntityVictim(10));
            }
            UpdateVictim();
        }

        private void cmdRemoveVictim_Click(object sender,EventArgs e)
        {
            entity.Victims.RemoveAt(victimindex);
            UpdateVictim();
        }

        private void numVictimID_ValueChanged(object sender,EventArgs e)
        {
            if (!Dirty)
            {
                entity.Victims[victimindex] = new EntityVictim((short)numVictimID.Value);
            }
        }

        private void UpdateLoadListA()
        {
            if (entity.LoadListA != null && entity.LoadListA.RowCount != 0)
            {
                fraLoadListPayload.Enabled = entity.LoadListB != null;
                if (loadlistarowindex >= entity.LoadListA.RowCount)
                    loadlistarowindex = entity.LoadListA.RowCount-1;
                lblMetavalueLoadA.Enabled = true;
                numMetavalueLoadA.Enabled = true;
                lblLoadListRowIndexA.Text = $"{loadlistarowindex+1} / {entity.LoadListA.RowCount}";
                numMetavalueLoadA.Value = entity.LoadListA.Rows[loadlistarowindex].MetaValue.Value;
                cmdPrevRowA.Enabled = loadlistarowindex > 0;
                cmdNextRowA.Enabled = loadlistarowindex + 1 < entity.LoadListA.RowCount;
                cmdRemoveRowA.Enabled = true;
                if (entity.LoadListA.Rows[loadlistarowindex].Values.Count > 0)
                {
                    if (loadlistaeidindex >= entity.LoadListA.Rows[loadlistarowindex].Values.Count)
                        loadlistaeidindex = entity.LoadListA.Rows[loadlistarowindex].Values.Count-1;
                    cmdInsertEIDA.Enabled = true;
                    cmdRemoveEIDA.Enabled = true;
                    txtEIDA.Enabled = true;
                    lblEIDErrA.Visible = true;
                    cmdPrevEIDA.Enabled = loadlistaeidindex > 0;
                    cmdNextEIDA.Enabled = loadlistaeidindex + 1 < entity.LoadListA.Rows[loadlistarowindex].Values.Count;
                    lblEIDIndexA.Text = $"{loadlistaeidindex+1} / {entity.LoadListA.Rows[loadlistarowindex].Values.Count}";
                    txtEIDA.Text = Entry.EIDToEName(entity.LoadListA.Rows[loadlistarowindex].Values[loadlistaeidindex]);
                }
                else
                {
                    cmdAppendEIDA.Enabled = true;
                    cmdInsertEIDA.Enabled = false;
                    cmdRemoveEIDA.Enabled = false;
                    txtEIDA.Enabled = false;
                    cmdPrevEIDA.Enabled = false;
                    cmdNextEIDA.Enabled = false;
                    lblEIDErrA.Visible = false;
                    lblEIDIndexA.Text = "-- / --";
                }
            }
            else
            {
                fraLoadListPayload.Enabled = false;
                entity.LoadListA = null;
                lblLoadListRowIndexA.Text = "-- / --";
                lblEIDIndexA.Text = "-- / --";
                lblMetavalueLoadA.Enabled = false;
                numMetavalueLoadA.Enabled = false;
                cmdPrevRowA.Enabled = false;
                cmdNextRowA.Enabled = false;
                cmdRemoveRowA.Enabled = false;
                txtEIDA.Enabled = false;
                cmdPrevEIDA.Enabled = false;
                cmdNextEIDA.Enabled = false;
                cmdRemoveEIDA.Enabled = false;
                cmdInsertEIDA.Enabled = false;
                cmdAppendEIDA.Enabled = false;
                lblEIDErrA.Visible = false;
            }
        }

        private void cmdPrevEIDA_Click(object sender, EventArgs e)
        {
            --loadlistaeidindex;
            UpdateLoadListA();
        }

        private void cmdNextEIDA_Click(object sender, EventArgs e)
        {
            ++loadlistaeidindex;
            UpdateLoadListA();
        }

        private void cmdPrevRowA_Click(object sender, EventArgs e)
        {
            --loadlistarowindex;
            UpdateLoadListA();
        }

        private void cmdNextRowA_Click(object sender, EventArgs e)
        {
            ++loadlistarowindex;
            UpdateLoadListA();
        }

        private void cmdRemoveEIDA_Click(object sender, EventArgs e)
        {
            entity.LoadListA.Rows[loadlistarowindex].Values.RemoveAt(loadlistaeidindex);
            UpdateLoadListA();
        }

        private void cmdInsertEIDA_Click(object sender, EventArgs e)
        {
            entity.LoadListA.Rows[loadlistarowindex].Values.Insert(loadlistaeidindex, entity.LoadListA.Rows[loadlistarowindex].Values[loadlistaeidindex]);
            UpdateLoadListA();
        }

        private void cmdAppendEIDA_Click(object sender, EventArgs e)
        {
            loadlistaeidindex = entity.LoadListA.Rows[loadlistarowindex].Values.Count;
            if (entity.LoadListA.Rows[loadlistarowindex].Values.Count > 0)
            {
                entity.LoadListA.Rows[loadlistarowindex].Values.Add(entity.LoadListA.Rows[loadlistarowindex].Values[loadlistaeidindex - 1]);
            }
            else
            {
                entity.LoadListA.Rows[loadlistarowindex].Values.Add(Entry.NullEID);
            }
            UpdateLoadListA();
        }

        private void txtEIDA_TextChanged(object sender, EventArgs e)
        {
            lblEIDErrA.Text = Entry.CheckEIDErrors(txtEIDA.Text, true);
            if (lblEIDErrA.Text != string.Empty) return;
            entity.LoadListA.Rows[loadlistarowindex].Values[loadlistaeidindex] = Entry.ENameToEID(txtEIDA.Text);
        }

        private void cmdRemoveRowA_Click(object sender, EventArgs e)
        {
            entity.LoadListA.Rows.RemoveAt(loadlistarowindex);
            UpdateLoadListA();
        }

        private void cmdInsertRowA_Click(object sender, EventArgs e)
        {
            if (entity.LoadListA == null || entity.LoadListA.Rows.Count == 0)
            {
                entity.LoadListA = new EntityT4Property();
                entity.LoadListA.Rows.Add(new EntityPropertyRow<int>());
                entity.LoadListA.Rows[entity.LoadListA.RowCount - 1].MetaValue = 0;
                loadlistarowindex = entity.LoadListA.RowCount - 1;
                loadlistaeidindex = 0;
            }
            else
            {
                var newrow = new EntityPropertyRow<int>();
                newrow.MetaValue = entity.LoadListA.Rows[loadlistarowindex].MetaValue;
                foreach (var val in entity.LoadListA.Rows[loadlistarowindex].Values)
                    newrow.Values.Add(val);
                entity.LoadListA.Rows.Insert(loadlistarowindex,newrow);
            }
            UpdateLoadListA();
        }

        private void numMetavalueLoadA_ValueChanged(object sender, EventArgs e)
        {
            entity.LoadListA.Rows[loadlistarowindex].MetaValue = (short)numMetavalueLoadA.Value;
        }

        private void UpdateLoadListB()
        {
            if (entity.LoadListB != null && entity.LoadListB.RowCount != 0)
            {
                fraLoadListPayload.Enabled = entity.LoadListA != null;
                if (loadlistbrowindex >= entity.LoadListB.RowCount)
                    loadlistbrowindex = entity.LoadListB.RowCount-1;
                lblMetavalueLoadB.Enabled = true;
                numMetavalueLoadB.Enabled = true;
                lblLoadListRowIndexB.Text = $"{loadlistbrowindex+1} / {entity.LoadListB.RowCount}";
                numMetavalueLoadB.Value = entity.LoadListB.Rows[loadlistbrowindex].MetaValue.Value;
                cmdPrevRowB.Enabled = loadlistbrowindex > 0;
                cmdNextRowB.Enabled = loadlistbrowindex + 1 < entity.LoadListB.RowCount;
                cmdRemoveRowB.Enabled = true;
                if (entity.LoadListB.Rows[loadlistbrowindex].Values.Count > 0)
                {
                    if (loadlistbeidindex >= entity.LoadListB.Rows[loadlistbrowindex].Values.Count)
                        loadlistbeidindex = entity.LoadListB.Rows[loadlistbrowindex].Values.Count-1;
                    cmdInsertEIDB.Enabled = true;
                    cmdRemoveEIDB.Enabled = true;
                    txtEIDB.Enabled = true;
                    lblEIDErrB.Visible = true;
                    cmdPrevEIDB.Enabled = loadlistbeidindex > 0;
                    cmdNextEIDB.Enabled = loadlistbeidindex + 1 < entity.LoadListB.Rows[loadlistbrowindex].Values.Count;
                    lblEIDIndexB.Text = $"{loadlistbeidindex+1} / {entity.LoadListB.Rows[loadlistbrowindex].Values.Count}";
                    txtEIDB.Text = Entry.EIDToEName(entity.LoadListB.Rows[loadlistbrowindex].Values[loadlistbeidindex]);
                }
                else
                {
                    cmdAppendEIDB.Enabled = true;
                    cmdInsertEIDB.Enabled = false;
                    cmdRemoveEIDB.Enabled = false;
                    txtEIDB.Enabled = false;
                    cmdPrevEIDB.Enabled = false;
                    cmdNextEIDB.Enabled = false;
                    lblEIDErrB.Visible = false;
                    lblEIDIndexB.Text = "-- / --";
                }
            }
            else
            {
                fraLoadListPayload.Enabled = false;
                entity.LoadListB = null;
                lblLoadListRowIndexB.Text = "-- / --";
                lblEIDIndexB.Text = "-- / --";
                lblMetavalueLoadB.Enabled = false;
                numMetavalueLoadB.Enabled = false;
                cmdPrevRowB.Enabled = false;
                cmdNextRowB.Enabled = false;
                cmdRemoveRowB.Enabled = false;
                txtEIDB.Enabled = false;
                cmdPrevEIDB.Enabled = false;
                cmdNextEIDB.Enabled = false;
                cmdRemoveEIDB.Enabled = false;
                cmdInsertEIDB.Enabled = false;
                cmdAppendEIDB.Enabled = false;
                lblEIDErrB.Visible = false;
            }
        }

        private void cmdPrevEIDB_Click(object sender, EventArgs e)
        {
            --loadlistbeidindex;
            UpdateLoadListB();
        }

        private void cmdNextEIDB_Click(object sender, EventArgs e)
        {
            ++loadlistbeidindex;
            UpdateLoadListB();
        }

        private void cmdPrevRowB_Click(object sender, EventArgs e)
        {
            --loadlistbrowindex;
            UpdateLoadListB();
        }

        private void cmdNextRowB_Click(object sender, EventArgs e)
        {
            ++loadlistbrowindex;
            UpdateLoadListB();
        }

        private void cmdRemoveEIDB_Click(object sender, EventArgs e)
        {
            entity.LoadListB.Rows[loadlistbrowindex].Values.RemoveAt(loadlistbeidindex);
            UpdateLoadListB();
        }

        private void cmdInsertEIDB_Click(object sender, EventArgs e)
        {
            entity.LoadListB.Rows[loadlistbrowindex].Values.Insert(loadlistbeidindex, entity.LoadListB.Rows[loadlistbrowindex].Values[loadlistbeidindex]);
            UpdateLoadListB();
        }

        private void cmdAppendEIDB_Click(object sender, EventArgs e)
        {
            loadlistbeidindex = entity.LoadListB.Rows[loadlistbrowindex].Values.Count;
            if (entity.LoadListB.Rows[loadlistbrowindex].Values.Count > 0)
            {
                entity.LoadListB.Rows[loadlistbrowindex].Values.Add(entity.LoadListB.Rows[loadlistbrowindex].Values[loadlistbeidindex - 1]);
            }
            else
            {
                entity.LoadListB.Rows[loadlistbrowindex].Values.Add(Entry.NullEID);
            }
            UpdateLoadListB();
        }

        private void txtEIDB_TextChanged(object sender, EventArgs e)
        {
            lblEIDErrB.Text = Entry.CheckEIDErrors(txtEIDB.Text, true);
            if (lblEIDErrB.Text != string.Empty) return;
            entity.LoadListB.Rows[loadlistbrowindex].Values[loadlistbeidindex] = Entry.ENameToEID(txtEIDB.Text);
        }

        private void cmdRemoveRowB_Click(object sender, EventArgs e)
        {
            entity.LoadListB.Rows.RemoveAt(loadlistbrowindex);
            UpdateLoadListB();
        }

        private void cmdInsertRowB_Click(object sender, EventArgs e)
        {
            if (entity.LoadListB == null || entity.LoadListB.Rows.Count == 0)
            {
                entity.LoadListB = new EntityT4Property();
                entity.LoadListB.Rows.Add(new EntityPropertyRow<int>());
                entity.LoadListB.Rows[entity.LoadListB.RowCount - 1].MetaValue = 0;
                loadlistbrowindex = entity.LoadListB.RowCount - 1;
                loadlistbeidindex = 0;
            }
            else
            {
                var newrow = new EntityPropertyRow<int>();
                newrow.MetaValue = entity.LoadListB.Rows[loadlistbrowindex].MetaValue;
                foreach (var val in entity.LoadListB.Rows[loadlistbrowindex].Values)
                    newrow.Values.Add(val);
                entity.LoadListB.Rows.Insert(loadlistbrowindex,newrow);
            }
            UpdateLoadListB();
        }

        private void numMetavalueLoadB_ValueChanged(object sender, EventArgs e)
        {
            entity.LoadListB.Rows[loadlistbrowindex].MetaValue = (short)numMetavalueLoadB.Value;
        }

        private void UpdateDrawListA()
        {
            if (entity.DrawListA != null && entity.DrawListA.RowCount != 0)
            {
                if (drawlistarowindex >= entity.DrawListA.RowCount)
                    drawlistarowindex = entity.DrawListA.RowCount-1;
                lblMetavalueDrawA.Enabled = true;
                numMetavalueDrawA.Enabled = true;
                lblDrawListRowIndexA.Text = $"{drawlistarowindex+1} / {entity.DrawListA.RowCount}";
                numMetavalueDrawA.Value = entity.DrawListA.Rows[drawlistarowindex].MetaValue.Value;
                cmdPrevRowDrawA.Enabled = drawlistarowindex > 0;
                cmdNextRowDrawA.Enabled = drawlistarowindex + 1 < entity.DrawListA.RowCount;
                cmdRemoveRowDrawA.Enabled = true;
                if (entity.DrawListA.Rows[drawlistarowindex].Values.Count > 0)
                {
                    if (drawlistaentityindex >= entity.DrawListA.Rows[drawlistarowindex].Values.Count)
                        drawlistaentityindex = entity.DrawListA.Rows[drawlistarowindex].Values.Count - 1;
                    cmdInsertEntityA.Enabled = true;
                    cmdRemoveEntityA.Enabled = true;
                    lblEntityA.Enabled = true;
                    numEntityA.Enabled = true;
                    cmdPrevEntityA.Enabled = drawlistaentityindex > 0;
                    cmdNextEntityA.Enabled = drawlistaentityindex + 1 < entity.DrawListA.Rows[drawlistarowindex].Values.Count;
                    lblEntityIndexA.Text = $"{drawlistaentityindex+1} / {entity.DrawListA.Rows[drawlistarowindex].Values.Count}";
                    numEntityA.Value = entity.DrawListA.Rows[drawlistarowindex].Values[drawlistaentityindex] >> 8 & 0xFFFF;
                }
                else
                {
                    cmdAppendEntityA.Enabled = true;
                    cmdInsertEntityA.Enabled = false;
                    cmdRemoveEntityA.Enabled = false;
                    lblEntityA.Enabled = false;
                    numEntityA.Enabled = false;
                    cmdPrevEntityA.Enabled = false;
                    cmdNextEntityA.Enabled = false;
                    lblEntityIndexA.Text = "-- / --";
                }
            }
            else
            {
                entity.DrawListA = null;
                lblDrawListRowIndexA.Text = "-- / --";
                lblEntityIndexA.Text = "-- / --";
                lblMetavalueDrawA.Enabled = false;
                numMetavalueDrawA.Enabled = false;
                cmdPrevRowDrawA.Enabled = false;
                cmdNextRowDrawA.Enabled = false;
                cmdRemoveRowDrawA.Enabled = false;
                lblEntityA.Enabled = false;
                numEntityA.Enabled = false;
                cmdPrevEntityA.Enabled = false;
                cmdNextEntityA.Enabled = false;
                cmdRemoveEntityA.Enabled = false;
                cmdInsertEntityA.Enabled = false;
                cmdAppendEntityA.Enabled = false;
            }
        }

        private void cmdPrevEntityA_Click(object sender, EventArgs e)
        {
            --drawlistaentityindex;
            UpdateDrawListA();
        }

        private void cmdNextEntityA_Click(object sender, EventArgs e)
        {
            ++drawlistaentityindex;
            UpdateDrawListA();
        }

        private void cmdPrevRowDrawA_Click(object sender, EventArgs e)
        {
            --drawlistarowindex;
            UpdateDrawListA();
        }

        private void cmdNextRowDrawA_Click(object sender, EventArgs e)
        {
            ++drawlistarowindex;
            UpdateDrawListA();
        }

        private void cmdRemoveEntityA_Click(object sender, EventArgs e)
        {
            entity.DrawListA.Rows[drawlistarowindex].Values.RemoveAt(drawlistaentityindex);
            UpdateDrawListA();
        }

        private void cmdInsertEntityA_Click(object sender, EventArgs e)
        {
            entity.DrawListA.Rows[drawlistarowindex].Values.Insert(drawlistaentityindex, entity.DrawListA.Rows[drawlistarowindex].Values[drawlistaentityindex]);
            UpdateDrawListA();
        }

        private void cmdAppendEntityA_Click(object sender, EventArgs e)
        {
            drawlistaentityindex = entity.DrawListA.Rows[drawlistarowindex].Values.Count;
            if (entity.DrawListA.Rows[drawlistarowindex].Values.Count > 0)
            {
                entity.DrawListA.Rows[drawlistarowindex].Values.Add(entity.DrawListA.Rows[drawlistarowindex].Values[drawlistaentityindex - 1]);
            }
            else
            {
                entity.DrawListA.Rows[drawlistarowindex].Values.Add(0);
            }
            UpdateDrawListA();
        }

        private void numEntityA_ValueChanged(object sender, EventArgs e)
        {
            if (controller is EntityController c)
            {
                foreach (ZoneEntry zone in c.ZoneEntryController.EntryChunkController.NSFController.NSF.GetEntries<ZoneEntry>())
                {
                    foreach (Entity otherentity in zone.Entities)
                    {
                        if (otherentity.ID.HasValue && otherentity.ID.Value == numEntityA.Value)
                        {
                            for (int i = 0; i < c.ZoneEntryController.ZoneEntry.ZoneCount; ++i)
                            {
                                if (zone.EID == BitConv.FromInt32(c.ZoneEntryController.ZoneEntry.Header, 0x194 + i * 4))
                                {
                                    entity.DrawListA.Rows[drawlistarowindex].Values[drawlistaentityindex] = (int)(i | (otherentity.ID << 8) | ((zone.Entities.IndexOf(otherentity) - BitConv.FromInt32(zone.Header, 0x188)) << 24));
                                }
                            }
                        }
                    }
                }
            }
            else if (controller is NewEntityController nc)
            {
                foreach (NewZoneEntry zone in nc.NewZoneEntryController.EntryChunkController.NSFController.NSF.GetEntries<NewZoneEntry>())
                {
                    foreach (Entity otherentity in zone.Entities)
                    {
                        if (otherentity.ID.HasValue && otherentity.ID.Value == numEntityA.Value)
                        {
                            for (int i = 0; i < nc.NewZoneEntryController.NewZoneEntry.ZoneCount; ++i)
                            {
                                if (zone.EID == BitConv.FromInt32(nc.NewZoneEntryController.NewZoneEntry.Header, 0x194 + i * 4))
                                {
                                    entity.DrawListA.Rows[drawlistarowindex].Values[drawlistaentityindex] = (int)(i | (otherentity.ID << 8) | ((zone.Entities.IndexOf(otherentity) - BitConv.FromInt32(zone.Header, 0x188)) << 24));
                                }
                            }
                        }
                    }
                }
            }
            UpdateDrawListA();
        }

        private void cmdRemoveRowDrawA_Click(object sender, EventArgs e)
        {
            entity.DrawListA.Rows.RemoveAt(drawlistarowindex);
            UpdateDrawListA();
        }

        private void cmdInsertRowDrawA_Click(object sender, EventArgs e)
        {
            if (entity.DrawListA == null || entity.DrawListA.Rows.Count == 0)
            {
                entity.DrawListA = new EntityInt32Property();
                entity.DrawListA.Rows.Add(new EntityPropertyRow<int>());
                entity.DrawListA.Rows[entity.DrawListA.RowCount - 1].MetaValue = 0;
            }
            else
            {
                var newrow = new EntityPropertyRow<int>();
                newrow.MetaValue = entity.DrawListA.Rows[drawlistarowindex].MetaValue;
                foreach (var val in entity.DrawListA.Rows[drawlistarowindex].Values)
                    newrow.Values.Add(val);
                entity.DrawListA.Rows.Insert(drawlistarowindex,newrow);
            }
            UpdateDrawListA();
        }

        private void numMetavalueDrawA_ValueChanged(object sender, EventArgs e)
        {
            entity.DrawListA.Rows[drawlistarowindex].MetaValue = (short)numMetavalueDrawA.Value;
        }

        private void UpdateDrawListB()
        {
            if (entity.DrawListB != null && entity.DrawListB.RowCount != 0)
            {
                if (drawlistbrowindex >= entity.DrawListB.RowCount)
                    drawlistbrowindex = entity.DrawListB.RowCount-1;
                lblMetavalueDrawB.Enabled = true;
                numMetavalueDrawB.Enabled = true;
                lblDrawListRowIndexB.Text = $"{drawlistbrowindex+1} / {entity.DrawListB.RowCount}";
                numMetavalueDrawB.Value = entity.DrawListB.Rows[drawlistbrowindex].MetaValue.Value;
                cmdPrevRowDrawB.Enabled = drawlistbrowindex > 0;
                cmdNextRowDrawB.Enabled = drawlistbrowindex + 1 < entity.DrawListB.RowCount;
                cmdRemoveRowDrawB.Enabled = true;
                if (entity.DrawListB.Rows[drawlistbrowindex].Values.Count > 0)
                {
                    if (drawlistbentityindex >= entity.DrawListB.Rows[drawlistbrowindex].Values.Count)
                        drawlistbentityindex = entity.DrawListB.Rows[drawlistbrowindex].Values.Count - 1;
                    cmdInsertEntityB.Enabled = true;
                    cmdRemoveEntityB.Enabled = true;
                    lblEntityB.Enabled = true;
                    numEntityB.Enabled = true;
                    cmdPrevEntityB.Enabled = drawlistbentityindex > 0;
                    cmdNextEntityB.Enabled = drawlistbentityindex + 1 < entity.DrawListB.Rows[drawlistbrowindex].Values.Count;
                    lblEntityIndexB.Text = $"{drawlistbentityindex+1} / {entity.DrawListB.Rows[drawlistbrowindex].Values.Count}";
                    numEntityB.Value = entity.DrawListB.Rows[drawlistbrowindex].Values[drawlistbentityindex] >> 8 & 0xFFFF;
                }
                else
                {
                    cmdAppendEntityB.Enabled = true;
                    cmdInsertEntityB.Enabled = false;
                    cmdRemoveEntityB.Enabled = false;
                    lblEntityB.Enabled = false;
                    numEntityB.Enabled = false;
                    cmdPrevEntityB.Enabled = false;
                    cmdNextEntityB.Enabled = false;
                    lblEntityIndexB.Text = "-- / --";
                }
            }
            else
            {
                entity.DrawListB = null;
                lblDrawListRowIndexB.Text = "-- / --";
                lblEntityIndexB.Text = "-- / --";
                lblMetavalueDrawB.Enabled = false;
                numMetavalueDrawB.Enabled = false;
                cmdPrevRowDrawB.Enabled = false;
                cmdNextRowDrawB.Enabled = false;
                cmdRemoveRowDrawB.Enabled = false;
                lblEntityB.Enabled = false;
                numEntityB.Enabled = false;
                cmdPrevEntityB.Enabled = false;
                cmdNextEntityB.Enabled = false;
                cmdRemoveEntityB.Enabled = false;
                cmdInsertEntityB.Enabled = false;
                cmdAppendEntityB.Enabled = false;
            }
        }

        private void cmdPrevEntityB_Click(object sender, EventArgs e)
        {
            --drawlistbentityindex;
            UpdateDrawListB();
        }

        private void cmdNextEntityB_Click(object sender, EventArgs e)
        {
            ++drawlistbentityindex;
            UpdateDrawListB();
        }

        private void cmdPrevRowDrawB_Click(object sender, EventArgs e)
        {
            --drawlistbrowindex;
            UpdateDrawListB();
        }

        private void cmdNextRowDrawB_Click(object sender, EventArgs e)
        {
            ++drawlistbrowindex;
            UpdateDrawListB();
        }

        private void cmdRemoveEntityB_Click(object sender, EventArgs e)
        {
            entity.DrawListB.Rows[drawlistbrowindex].Values.RemoveAt(drawlistbentityindex);
            UpdateDrawListB();
        }

        private void cmdInsertEntityB_Click(object sender, EventArgs e)
        {
            entity.DrawListB.Rows[drawlistbrowindex].Values.Insert(drawlistbentityindex, entity.DrawListB.Rows[drawlistbrowindex].Values[drawlistbentityindex]);
            UpdateDrawListB();
        }

        private void cmdAppendEntityB_Click(object sender, EventArgs e)
        {
            drawlistbentityindex = entity.DrawListB.Rows[drawlistbrowindex].Values.Count;
            if (entity.DrawListB.Rows[drawlistbrowindex].Values.Count > 0)
            {
                entity.DrawListB.Rows[drawlistbrowindex].Values.Add(entity.DrawListB.Rows[drawlistbrowindex].Values[drawlistbentityindex - 1]);
            }
            else
            {
                entity.DrawListB.Rows[drawlistbrowindex].Values.Add(new int());
            }
            UpdateDrawListB();
        }

        private void numEntityB_ValueChanged(object sender, EventArgs e)
        {
            if (controller is EntityController c)
            {
                foreach (ZoneEntry zone in c.ZoneEntryController.EntryChunkController.NSFController.NSF.GetEntries<ZoneEntry>())
                {
                    foreach (Entity otherentity in zone.Entities)
                    {
                        if (otherentity.ID.HasValue && otherentity.ID.Value == numEntityB.Value)
                        {
                            for (int i = 0; i < c.ZoneEntryController.ZoneEntry.ZoneCount; ++i)
                            {
                                if (zone.EID == BitConv.FromInt32(c.ZoneEntryController.ZoneEntry.Header, 0x194 + i * 4))
                                {
                                    entity.DrawListB.Rows[drawlistbrowindex].Values[drawlistbentityindex] = (int)(i | (otherentity.ID << 8) | ((zone.Entities.IndexOf(otherentity) - BitConv.FromInt32(zone.Header, 0x188)) << 24));
                                }
                            }
                        }
                    }
                }
            }
            else if (controller is NewEntityController nc)
            {
                foreach (NewZoneEntry zone in nc.NewZoneEntryController.EntryChunkController.NSFController.NSF.GetEntries<NewZoneEntry>())
                {
                    foreach (Entity otherentity in zone.Entities)
                    {
                        if (otherentity.ID.HasValue && otherentity.ID.Value == numEntityB.Value)
                        {
                            for (int i = 0; i < nc.NewZoneEntryController.NewZoneEntry.ZoneCount; ++i)
                            {
                                if (zone.EID == BitConv.FromInt32(nc.NewZoneEntryController.NewZoneEntry.Header, 0x194 + i * 4))
                                {
                                    entity.DrawListB.Rows[drawlistbrowindex].Values[drawlistbentityindex] = (int)(i | (otherentity.ID << 8) | ((zone.Entities.IndexOf(otherentity) - BitConv.FromInt32(zone.Header, 0x188)) << 24));
                                }
                            }
                        }
                    }
                }
            }
            UpdateDrawListB();
        }

        private void cmdRemoveRowDrawB_Click(object sender, EventArgs e)
        {
            entity.DrawListB.Rows.RemoveAt(drawlistbrowindex);
            UpdateDrawListB();
        }

        private void cmdInsertRowDrawB_Click(object sender, EventArgs e)
        {
            if (entity.DrawListB == null || entity.DrawListB.Rows.Count == 0)
            {
                entity.DrawListB = new EntityInt32Property();
                entity.DrawListB.Rows.Add(new EntityPropertyRow<int>());
                entity.DrawListB.Rows[entity.DrawListB.RowCount - 1].MetaValue = 0;
            }
            else
            {
                var newrow = new EntityPropertyRow<int>();
                newrow.MetaValue = entity.DrawListB.Rows[drawlistbrowindex].MetaValue;
                foreach (var val in entity.DrawListB.Rows[drawlistbrowindex].Values)
                    newrow.Values.Add(val);
                entity.DrawListB.Rows.Insert(drawlistbrowindex,newrow);
            }
            UpdateDrawListB();
        }

        private void numMetavalueDrawB_ValueChanged(object sender, EventArgs e)
        {
            entity.DrawListB.Rows[drawlistbrowindex].MetaValue = (short)numMetavalueDrawB.Value;
        }

        private void UpdateDDASettings()
        {
            if (entity.DDASettings.HasValue)
            {
                numDDASettings.Value = entity.DDASettings.Value >> 8;
            }
            numDDASettings.Enabled = entity.DDASettings.HasValue;
            chkDDASettings.Checked = entity.DDASettings.HasValue;
        }

        private void chkDDASettings_CheckedChanged(object sender, EventArgs e)
        {
            numDDASettings.Enabled = chkDDASettings.Checked;
            if (chkDDASettings.Checked)
            {
                entity.DDASettings = (int)numDDASettings.Value << 8;
            }
            else
            {
                entity.DDASettings = null;
            }
        }

        private void numDDASettings_ValueChanged(object sender, EventArgs e)
        {
            entity.DDASettings = (int)numDDASettings.Value << 8;
        }

        private void UpdateDDASection()
        {
            if (entity.DDASection.HasValue)
            {
                numDDASection.Value = entity.DDASection.Value;
            }
            numDDASection.Enabled = entity.DDASection.HasValue;
            chkDDASection.Checked = entity.DDASection.HasValue;
        }

        private void chkDDASection_CheckedChanged(object sender, EventArgs e)
        {
            numDDASection.Enabled = chkDDASection.Checked;
            if (chkDDASection.Checked)
            {
                entity.DDASection = (int)numDDASection.Value;
            }
            else
            {
                entity.DDASection = null;
            }
        }

        private void numDDASection_ValueChanged(object sender, EventArgs e)
        {
            entity.DDASection = (int)numDDASection.Value;
        }

        private void UpdateScaling()
        {
            if (entity.Scaling.HasValue)
            {
                numScaling.Value = entity.Scaling.Value;
            }
            numScaling.Enabled = entity.Scaling.HasValue;
            chkScaling.Checked = entity.Scaling.HasValue;
        }

        private void chkScaling_CheckedChanged(object sender, EventArgs e)
        {
            numScaling.Enabled = chkScaling.Checked;
            if (chkScaling.Checked)
            {
                entity.Scaling = (int)numScaling.Value;
            }
            else
            {
                entity.Scaling = null;
            }
        }

        private void numScaling_ValueChanged(object sender, EventArgs e)
        {
            entity.Scaling = (int)numScaling.Value;
        }

        private void UpdateOtherSettings()
        {
            if (entity.OtherSettings.HasValue)
            {
                numOtherSettings.Value = entity.OtherSettings.Value;
            }
            numOtherSettings.Enabled = entity.OtherSettings.HasValue;
            chkOtherSettings.Checked = entity.OtherSettings.HasValue;
        }

        private void chkOtherSettings_CheckedChanged(object sender, EventArgs e)
        {
            numOtherSettings.Enabled = chkOtherSettings.Checked;
            if (chkOtherSettings.Checked)
            {
                entity.OtherSettings = (int)numOtherSettings.Value;
            }
            else
            {
                entity.OtherSettings = null;
            }
        }

        private void numOtherSettings_ValueChanged(object sender, EventArgs e)
        {
            entity.OtherSettings = (int)numOtherSettings.Value;
        }

        private void UpdateSLST()
        {
            if (entity.SLST != null)
            {
                txtSLST.Text = Entry.EIDToEName(entity.SLST.Rows[0].Values[0]);
                chkSLST.Checked = true;
                lblEIDErr1.Visible = true;
                txtSLST.Enabled = true;
            }
            else
            {
                txtSLST.Enabled = false;
                chkSLST.Checked = false;
                lblEIDErr1.Visible = false;
                txtSLST.Enabled = false;
            }
        }

        private void chkSLST_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSLST.Checked)
            {
                lblEIDErr1.Text = Entry.CheckEIDErrors(txtSLST.Text, true);
                entity.SLST = new EntityT4Property();
                entity.SLST.Rows.Add(new EntityPropertyRow<int>());
                if (lblEIDErr1.Text != string.Empty)
                    entity.SLST.Rows[0].Values.Add(Entry.NullEID);
                else
                    entity.SLST.Rows[0].Values.Add(Entry.ENameToEID(txtSLST.Text));
            }
            else
            {
                entity.SLST = null;
            }
            UpdateSLST();
        }

        private void txtSLST_TextChanged(object sender, EventArgs e)
        {
            lblEIDErr1.Text = Entry.CheckEIDErrors(txtSLST.Text, true);
            if (lblEIDErr1.Text != string.Empty) return;
            entity.SLST.Rows[0].Values[0] = Entry.ENameToEID(txtSLST.Text);
        }

        private void tabSpecial_Enter(object sender, EventArgs e)
        {
            UpdateVictim();
            UpdateScaling();
            UpdateBoxCount();
            UpdateDDASection();
            UpdateDDASettings();
            UpdateOtherSettings();
            UpdateZMod();
            UpdateTTReward();
            tabSpecial.Enter -= tabSpecial_Enter;
        }

        private void tabCamera_Enter(object sender, EventArgs e)
        {
            UpdateSLST();
            UpdateCameraIndex();
            UpdateCameraSubIndex();
            UpdateMode();
            UpdateAvgDist();
            UpdateNeighbors();
            UpdateFOV();
            tabCamera.Enter -= tabCamera_Enter;
        }

        private void tabLoadLists_Enter(object sender, EventArgs e)
        {
            UpdateLoadListA();
            UpdateLoadListB();
            tabLoadLists.Enter -= tabLoadLists_Enter;
        }

        private void tabDrawLists_Enter(object sender, EventArgs e)
        {
            UpdateDrawListA();
            UpdateDrawListB();
            tabDrawLists.Enter -= tabDrawLists_Enter;
        }

        private void txtEIDA_LostFocus(object sender, EventArgs e)
        {
            UpdateLoadListA();
        }

        private void txtEIDB_LostFocus(object sender, EventArgs e)
        {
            UpdateLoadListB();
        }

        private void cmdLoadListVerify_Click(object sender, EventArgs e)
        {
            bool haserror = false;
            List<int> loadedentries = new List<int>();
            for (int i = 0; i < entity.Positions.Count; ++i)
            {
                foreach (var row in entity.LoadListA.Rows)
                {
                    if (row.MetaValue == i)
                    {
                        // load
                        foreach (int eid in row.Values)
                        {
                            loadedentries.Add(eid);
                        }
                    }
                }
                foreach (var row in entity.LoadListB.Rows)
                {
                    if (row.MetaValue == i)
                    {
                        // unload
                        foreach (int eid in row.Values)
                        {
                            if (!loadedentries.Remove(eid))
                            {
                                MessageBox.Show(this, $"Load lists are incorrect. {Entry.EIDToEName(eid)} was already deloaded by position {i}.", "Load list verification exception.");
                                haserror = true;
                            }
                        }
                    }
                }
            }
            if (loadedentries.Count == 0 && !haserror)
                MessageBox.Show(this, "Load lists are correct.", "Load list verification exception.");
            else if (loadedentries.Count != 0)
            {
                string eidlist = string.Empty;
                foreach (int eid in loadedentries)
                {
                    eidlist += Entry.EIDToEName(eid) + Environment.NewLine;
                }
                MessageBox.Show(this, $"Load lists are incorrect. The following entries are never deloaded:\n{eidlist}", "Load list verification exception.");
            }
        }

        private void cmdPayload_Click(object sender, EventArgs e)
        {
            List<int> loadedentries = new List<int>();
            for (int i = 0; i < numPayloadPosition.Value + 1; ++i)
            {
                foreach (var row in entity.LoadListA.Rows)
                {
                    if (row.MetaValue == i)
                    {
                        // load
                        foreach (int eid in row.Values)
                        {
                            loadedentries.Add(eid);
                        }
                    }
                }
                foreach (var row in entity.LoadListB.Rows)
                {
                    if (row.MetaValue == i)
                    {
                        // unload
                        foreach (int eid in row.Values)
                        {
                            if (!loadedentries.Remove(eid))
                            {
                                MessageBox.Show(this, $"Load lists are incorrect. {Entry.EIDToEName(eid)} was already deloaded by position {i}.", "Load list verification exception.");
                                return;
                            }
                        }
                    }
                }
            }
            EvList<Chunk> chunks = null;
            HashSet<Entry> entries = null;
            if (controller is EntityController c2c)
            {
                chunks = c2c.ZoneEntryController.EntryChunkController.NSFController.NSF.Chunks;
                entries = new HashSet<Entry>();
                foreach (int eid in loadedentries)
                {
                    entries.Add(c2c.ZoneEntryController.EntryChunkController.NSFController.NSF.GetEntry<Entry>(eid));
                }
            }
            else if (controller is NewEntityController c3c)
            {
                chunks = c3c.NewZoneEntryController.EntryChunkController.NSFController.NSF.Chunks;
                entries = new HashSet<Entry>();
                foreach (int eid in loadedentries)
                {
                    entries.Add(c3c.NewZoneEntryController.EntryChunkController.NSFController.NSF.GetEntry<Entry>(eid));
                }
            }
            HashSet<Chunk> loadedchunks = new HashSet<Chunk>();
            foreach (Chunk chunk in chunks)
            {
                if (chunk is NormalChunk c)
                {
                    foreach (Entry entry in entries)
                    {
                        if (c.Entries.Contains(entry))
                            loadedchunks.Add(chunk);
                    }
                }
            }
            lblPayload.Visible = true;
            lblPayload.Text = $"Payload is ~{loadedchunks.Count} normal chunks";
            if (loadedchunks.Count < 20)
            {
                lblPayload.ForeColor = Color.Green;
            }
            else if (loadedchunks.Count <= 21)
            {
                lblPayload.ForeColor = Color.Goldenrod;
            }
            else
            {
                lblPayload.ForeColor = Color.Red;
            }
        }

        private void chkSettingHex_CheckedChanged(object sender, EventArgs e)
        {
            numSettingC.Hexadecimal = chkSettingHex.Checked;
            SetCVal((long)numSettingC.Value);
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
                    for (int m = interpolator.Start - 1, i = interpolator.End - 2; i > m; --i)
                    {
                        entity.Positions.RemoveAt(i);
                    }
                    for (int i = 0; i < interpolator.Amount; ++i)
                    {
                        entity.Positions.Insert(i + interpolator.Start, new EntityPosition(interpolator.NewPositions[i + 1]));
                    }
                    UpdatePosition();
                }
            }
        }
        
        private void UpdateTTReward()
        {
            if (entity.TimeTrialReward.HasValue)
            {
                numTTReward.Value = entity.TimeTrialReward.Value >> 8;
            }
            numTTReward.Enabled = entity.TimeTrialReward.HasValue;
            chkTTReward.Checked = entity.TimeTrialReward.HasValue;
        }

        private void chkTTReward_CheckedChanged(object sender, EventArgs e)
        {
            numTTReward.Enabled = chkTTReward.Checked;
            if (chkTTReward.Checked)
            {
                entity.TimeTrialReward = (int)numTTReward.Value << 8;
            }
            else
            {
                entity.TimeTrialReward = null;
            }
        }

        private void numTTReward_ValueChanged(object sender, EventArgs e)
        {
            entity.TimeTrialReward = (int)numTTReward.Value << 8;
        }

        private void UpdateCameraIndex()
        {
            if (entity.CameraIndex.HasValue)
            {
                numCameraIndex.Value = entity.CameraIndex.Value;
            }
            numCameraIndex.Enabled = entity.CameraIndex.HasValue;
            chkCameraIndex.Checked = entity.CameraIndex.HasValue;
        }

        private void UpdateCameraSubIndex()
        {
            if (entity.CameraSubIndex.HasValue)
            {
                numCameraSubIndex.Value = entity.CameraSubIndex.Value;
            }
            numCameraSubIndex.Enabled = entity.CameraSubIndex.HasValue;
            chkCameraSubIndex.Checked = entity.CameraSubIndex.HasValue;
        }

        private void numCameraIndex_ValueChanged(object sender, EventArgs e)
        {
            entity.CameraIndex = (int)numCameraIndex.Value;
        }

        private void chkCameraIndex_CheckedChanged(object sender, EventArgs e)
        {
            numCameraIndex.Enabled = chkCameraIndex.Checked;
            if (chkCameraIndex.Checked)
                entity.CameraIndex = (int)numCameraIndex.Value;
            else
                entity.CameraIndex = null;
        }

        private void numCameraSubIndex_ValueChanged(object sender, EventArgs e)
        {
            entity.CameraSubIndex = (int)numCameraSubIndex.Value;
        }

        private void chkCameraSubIndex_CheckedChanged(object sender, EventArgs e)
        {
            numCameraSubIndex.Enabled = chkCameraSubIndex.Checked;
            if (chkCameraSubIndex.Checked)
                entity.CameraSubIndex = (int)numCameraSubIndex.Value;
            else
                entity.CameraSubIndex = null;
        }

        private void UpdateMode()
        {
            //if (entity.Mode.HasValue)
            //{
            //    numMode.Value = entity.Mode.Value;
            //}
            //numMode.Enabled = entity.Mode.HasValue;
            //chkMode.Checked = entity.Mode.HasValue;
        }

        private void numMode_ValueChanged(object sender, EventArgs e)
        {
            //entity.Mode = (byte)numMode.Value;
        }

        private void chkMode_CheckedChanged(object sender, EventArgs e)
        {
            //numMode.Enabled = chkMode.Checked;
            //if (chkMode.Checked)
            //    entity.Mode = (byte)numMode.Value;
            //else
            //    entity.Mode = null;
        }

        private void UpdateAvgDist()
        {
            if (entity.AverageDistance.HasValue)
            {
                numAvgDist.Value = entity.AverageDistance.Value.ValueB;
            }
            numAvgDist.Enabled = entity.AverageDistance.HasValue;
            chkAvgDist.Checked = entity.AverageDistance.HasValue;
        }

        private void numAvgDist_ValueChanged(object sender, EventArgs e)
        {
            entity.AverageDistance = new EntitySetting(0, (int)numAvgDist.Value);
        }

        private void chkAvgDist_CheckedChanged(object sender, EventArgs e)
        {
            numAvgDist.Enabled = chkAvgDist.Checked;
            if (chkAvgDist.Checked)
                entity.AverageDistance = new EntitySetting(0, (int)numAvgDist.Value);
            else
                entity.AverageDistance = null;
        }

        private void UpdateNeighbors()
        {
            if (entity.Neighbors != null && entity.Neighbors.RowCount != 0)
            {
                if (neighborindex >= entity.Neighbors.RowCount)
                    neighborindex = entity.Neighbors.RowCount - 1;
                numNeighborPosition.Value = entity.Neighbors.Rows[neighborindex].MetaValue.Value;
                lblNeighbor.Text = $"{neighborindex + 1} / {entity.Neighbors.RowCount}";
                cmdPrevNeighbor.Enabled = neighborindex > 0;
                cmdNextNeighbor.Enabled = neighborindex + 1 < entity.Neighbors.RowCount;
                lblNeighborPosition.Enabled =
                numNeighborPosition.Enabled =
                cmdRemoveNeighbor.Enabled = true;
                cmdInsertNeighborSetting.Enabled = true;
                neighborsettingindex = Math.Min(entity.Neighbors.Rows[neighborindex].Values.Count - 1, neighborsettingindex);
                if (entity.Neighbors.Rows[neighborindex].Values.Count > 0)
                {
                    lblNeighborSetting.Text = $"{neighborsettingindex+1} / {entity.Neighbors.Rows[neighborindex].Values.Count}";
                    cmdPrevNeighborSetting.Enabled = neighborsettingindex > 0;
                    cmdNextNeighborSetting.Enabled = neighborsettingindex + 1 < entity.Neighbors.Rows[neighborindex].Values.Count;
                    cmdRemoveNeighborSetting.Enabled =
                    numNeighborFlag.Enabled =
                    numNeighborZone.Enabled =
                    numNeighborCamera.Enabled =
                    numNeighborLink.Enabled =
                    lblNeighborFlag.Enabled =
                    lblNeighborZone.Enabled =
                    lblNeighborCamera.Enabled =
                    lblNeighborLink.Enabled = true;
                    numNeighborFlag.Value = (entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] & (0xFF << 0)) >> 0;
                    numNeighborCamera.Value = (entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] & (0xFF << 8)) >> 8;
                    numNeighborZone.Value = (entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] & (0xFF << 16)) >> 16;
                    numNeighborLink.Value = (entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] & (0xFF << 24)) >> 24;
                }
                else
                {
                    lblNeighborSetting.Text = "-- / --";
                    cmdPrevNeighborSetting.Enabled =
                    cmdNextNeighborSetting.Enabled =
                    cmdRemoveNeighborSetting.Enabled =
                    numNeighborFlag.Enabled =
                    numNeighborZone.Enabled =
                    numNeighborCamera.Enabled =
                    numNeighborLink.Enabled =
                    lblNeighborFlag.Enabled =
                    lblNeighborZone.Enabled =
                    lblNeighborCamera.Enabled =
                    lblNeighborLink.Enabled = false;
                }
            }
            else
            {
                entity.Neighbors = null;
                lblNeighbor.Text = "-- / --";
                lblNeighborSetting.Text = "-- / --";
                lblNeighborPosition.Enabled =
                numNeighborPosition.Enabled =
                cmdPrevNeighbor.Enabled =
                cmdNextNeighbor.Enabled =
                cmdRemoveNeighbor.Enabled =
                cmdInsertNeighborSetting.Enabled =
                cmdPrevNeighborSetting.Enabled =
                cmdNextNeighborSetting.Enabled =
                cmdRemoveNeighborSetting.Enabled =
                numNeighborFlag.Enabled =
                numNeighborZone.Enabled =
                numNeighborCamera.Enabled =
                numNeighborLink.Enabled =
                lblNeighborFlag.Enabled =
                lblNeighborZone.Enabled =
                lblNeighborCamera.Enabled =
                lblNeighborLink.Enabled = false;
            }
        }

        private void cmdNextNeighbor_Click(object sender, EventArgs e)
        {
            ++neighborindex;
            UpdateNeighbors();
        }

        private void cmdPrevNeighbor_Click(object sender, EventArgs e)
        {
            --neighborindex;
            UpdateNeighbors();
        }

        private void cmdRemoveNeighbor_Click(object sender, EventArgs e)
        {
            entity.Neighbors.Rows.RemoveAt(neighborindex);
            UpdateNeighbors();
        }

        private void cmdInsertNeighbor_Click(object sender, EventArgs e)
        {
            if (entity.Neighbors == null || entity.Neighbors.Rows.Count == 0)
            {
                entity.Neighbors = new EntityUInt32Property();
                entity.Neighbors.Rows.Add(new EntityPropertyRow<uint>());
                entity.Neighbors.Rows[entity.Neighbors.RowCount - 1].MetaValue = 0;
            }
            else
            {
                var newrow = new EntityPropertyRow<uint>();
                newrow.MetaValue = entity.Neighbors.Rows[neighborindex].MetaValue;
                foreach (var val in entity.Neighbors.Rows[neighborindex].Values)
                    newrow.Values.Add(val);
                entity.Neighbors.Rows.Insert(neighborindex,newrow);
            }
            UpdateNeighbors();
        }

        private void numNeighborFlag_ValueChanged(object sender, EventArgs e)
        {
            entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] &= 0xFFFFFF00;
            entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] |= (uint)((byte)numNeighborFlag.Value << 0);
        }

        private void numNeighborCamera_ValueChanged(object sender, EventArgs e)
        {
            entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] &= 0xFFFF00FF;
            entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] |= (uint)((byte)numNeighborCamera.Value << 8);
        }

        private void numNeighborZone_ValueChanged(object sender, EventArgs e)
        {
            entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] &= 0xFF00FFFF;
            entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] |= (uint)((byte)numNeighborZone.Value << 16);
        }

        private void numNeighborLink_ValueChanged(object sender, EventArgs e)
        {
            entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] &= 0x00FFFFFF;
            entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex] |= (uint)((byte)numNeighborLink.Value << 24);
        }

        private void numNeighborPosition_ValueChanged(object sender, EventArgs e)
        {
            entity.Neighbors.Rows[neighborindex].MetaValue = (short)numNeighborPosition.Value;
        }

        private void cmdPrevNeighborSetting_Click(object sender, EventArgs e)
        {
            --neighborsettingindex;
            UpdateNeighbors();
        }

        private void cmdNextNeighborSetting_Click(object sender, EventArgs e)
        {
            ++neighborsettingindex;
            UpdateNeighbors();
        }

        private void cmdRemoveNeighborSetting_Click(object sender, EventArgs e)
        {
            entity.Neighbors.Rows[neighborindex].Values.RemoveAt(neighborsettingindex);
            UpdateNeighbors();
        }

        private void cmdInsertNeighborSetting_Click(object sender, EventArgs e)
        {
            if (entity.Neighbors.Rows[neighborindex].Values.Count == 0)
                entity.Neighbors.Rows[neighborindex].Values.Add(0);
            else
                entity.Neighbors.Rows[neighborindex].Values.Insert(neighborsettingindex, entity.Neighbors.Rows[neighborindex].Values[neighborsettingindex]);
            UpdateNeighbors();
        }

        private void UpdateZMod()
        {
            if (entity.ZMod.HasValue)
            {
                numZMod.Value = entity.ZMod.Value;
            }
            numZMod.Enabled = entity.ZMod.HasValue;
            chkZMod.Checked = entity.ZMod.HasValue;
        }

        private void chkZMod_CheckedChanged(object sender, EventArgs e)
        {
            numZMod.Enabled = chkZMod.Checked;
            if (chkZMod.Checked)
            {
                entity.ZMod = (int)numZMod.Value;
            }
            else
            {
                entity.ZMod = null;
            }
        }

        private void numZMod_ValueChanged(object sender, EventArgs e)
        {
            entity.ZMod = (int)numZMod.Value;
        }

        private void UpdateFOV()
        {
            if (entity.FOV != null && entity.FOV.RowCount != 0)
            {
                if (fovframeindex >= entity.FOV.RowCount)
                    fovframeindex = entity.FOV.RowCount-1;
                lblFOVPosition.Enabled = true;
                numFOVPosition.Enabled = true;
                numFOVPosition.Value = entity.FOV.Rows[fovframeindex].MetaValue.Value;
                lblFOVFrame.Text = $"{fovframeindex+1} / {entity.FOV.RowCount}";
                cmdPrevFOVFrame.Enabled = fovframeindex > 0;
                cmdNextFOVFrame.Enabled = fovframeindex + 1 < entity.FOV.RowCount;
                cmdRemoveFOVFrame.Enabled = true;
                if (entity.FOV.Rows[fovframeindex].Values.Count > 0)
                {
                    if (fovindex >= entity.FOV.Rows[fovframeindex].Values.Count)
                        fovindex = entity.FOV.Rows[fovframeindex].Values.Count - 1;
                    cmdInsertFOV.Enabled = true;
                    cmdRemoveFOV.Enabled = true;
                    lblFOV.Enabled = true;
                    numFOV.Enabled = true;
                    cmdPrevFOV.Enabled = fovindex > 0;
                    cmdNextFOV.Enabled = fovindex + 1 < entity.FOV.Rows[fovframeindex].Values.Count;
                    lblFOVIndex.Text = $"{fovindex+1} / {entity.FOV.Rows[fovframeindex].Values.Count}";
                    numFOV.Value = entity.FOV.Rows[fovframeindex].Values[fovindex].VictimID;
                }
                else
                {
                    cmdInsertFOV.Enabled = false;
                    cmdRemoveFOV.Enabled = false;
                    lblFOV.Enabled = false;
                    numFOV.Enabled = false;
                    cmdPrevFOV.Enabled = false;
                    cmdNextFOV.Enabled = false;
                    lblFOVIndex.Text = "-- / --";
                }
            }
            else
            {
                entity.FOV = null;
                lblFOVFrame.Text = "-- / --";
                lblFOVIndex.Text = "-- / --";
                lblFOVPosition.Enabled = false;
                cmdPrevFOVFrame.Enabled = false;
                cmdNextFOVFrame.Enabled = false;
                cmdRemoveFOVFrame.Enabled = false;
                lblFOV.Enabled = false;
                numFOV.Enabled = false;
                cmdPrevFOV.Enabled = false;
                cmdNextFOV.Enabled = false;
                cmdRemoveFOV.Enabled = false;
                cmdInsertFOV.Enabled = false;
            }
        }

        private void cmdPrevFOVFrame_Click(object sender, EventArgs e)
        {
            --fovframeindex;
            UpdateFOV();
        }

        private void cmdNextFOVFrame_Click(object sender, EventArgs e)
        {
            ++fovframeindex;
            UpdateFOV();
        }

        private void cmdRemoveFOVFrame_Click(object sender, EventArgs e)
        {
            entity.FOV.Rows.RemoveAt(fovframeindex);
            UpdateFOV();
        }

        private void cmdInsertFOVFrame_Click(object sender, EventArgs e)
        {
            if (entity.FOV == null || entity.FOV.Rows.Count == 0)
            {
                entity.FOV = new EntityVictimProperty();
                entity.FOV.Rows.Add(new EntityPropertyRow<EntityVictim>());
                entity.FOV.Rows[entity.FOV.RowCount - 1].MetaValue = 0;
            }
            else
            {
                var newrow = new EntityPropertyRow<EntityVictim>();
                newrow.MetaValue = entity.FOV.Rows[fovframeindex].MetaValue;
                foreach (var val in entity.FOV.Rows[fovframeindex].Values)
                    newrow.Values.Add(val);
                entity.FOV.Rows.Insert(fovframeindex,newrow);
            }
            UpdateFOV();
        }

        private void cmdPrevFOV_Click(object sender, EventArgs e)
        {
            --fovindex;
            UpdateFOV();
        }

        private void cmdNextFOV_Click(object sender, EventArgs e)
        {
            ++fovindex;
            UpdateFOV();
        }

        private void cmdRemoveFOV_Click(object sender, EventArgs e)
        {
            entity.FOV.Rows[fovframeindex].Values.RemoveAt(fovindex);
            UpdateFOV();
        }

        private void cmdInsertFOV_Click(object sender, EventArgs e)
        {
            if (entity.FOV.Rows[fovframeindex].Values.Count == 0)
                entity.FOV.Rows[fovframeindex].Values.Add(new EntityVictim());
            else
                entity.FOV.Rows[fovframeindex].Values.Insert(fovindex, entity.FOV.Rows[fovframeindex].Values[fovindex]);
            UpdateFOV();
        }

        private void numFOVPosition_ValueChanged(object sender, EventArgs e)
        {
            entity.FOV.Rows[fovframeindex].MetaValue = (short)numFOVPosition.Value;
        }

        private void numFOV_ValueChanged(object sender, EventArgs e)
        {
            entity.FOV.Rows[fovframeindex].Values[fovindex] = new EntityVictim((short)numFOV.Value);
        }
    }
}
