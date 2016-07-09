using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class EntityBox : UserControl
    {
        private EntityController controller;
        private Entity entity;

        private bool positiondirty;
        private int positionindex;
        private bool settingdirty;
        private int settingindex;
        private bool victimdirty;
        private int victimindex;
        private bool text;
        private int loadlistarowindex;
        private int loadlistaeidindex;
        private int loadlistbrowindex;
        private int loadlistbeidindex;
        private int drawlistarowindex;
        private int drawlistaentityindex;
        private int drawlistbrowindex;
        private int drawlistbentityindex;

        public EntityBox(EntityController controller)
        {
            this.controller = controller;
            entity = controller.Entity;
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
        }

        private void InvalidateNodes()
        {
            controller.InvalidateNode();
        }

        public static string Reverse(string str)
        {
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
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
            if (chkName.Checked)
            {
                entity.Name = txtName.Text;
            }
            else
            {
                entity.Name = null;
            }
            InvalidateNodes();
        }

        private void txtName_TextChanged(object sender,EventArgs e)
        {
            entity.Name = txtName.Text;
            InvalidateNodes();
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
                cmdNextAndRemove.Enabled = false;
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
                cmdRemovePosition.Enabled = true;
                cmdNextAndRemove.Enabled = (positionindex < entity.Positions.Count - 1);
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

        private void UpdateSettings()
        {
            settingdirty = true;
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
                cmdPreviousSetting.Enabled = false;
                cmdNextSetting.Enabled = false;
                cmdRemoveSetting.Enabled = false;
                lblSettingA.Enabled = false;
                lblSettingB.Enabled = false;
                numSettingA.Enabled = false;
                numSettingB.Enabled = false;
            }
            else
            {
                lblSettingIndex.Text = string.Format("{0} / {1}",settingindex + 1,entity.Settings.Count);
                cmdPreviousSetting.Enabled = (settingindex > 0);
                cmdNextSetting.Enabled = (settingindex < entity.Settings.Count - 1);
                cmdRemoveSetting.Enabled = true;
                lblSettingA.Enabled = true;
                lblSettingB.Enabled = true;
                numSettingA.Enabled = true;
                numSettingB.Enabled = true;
                numSettingA.Value = entity.Settings[settingindex].ValueA;
                numSettingB.Value = entity.Settings[settingindex].ValueB;
            }
            settingdirty = false;
        }

        private void cmdPreviousSetting_Click(object sender,EventArgs e)
        {
            settingindex--;
            UpdateSettings();
        }

        private void cmdNextSetting_Click(object sender,EventArgs e)
        {
            settingindex++;
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
            if (!settingdirty)
            {
                EntitySetting s = entity.Settings[settingindex];
                entity.Settings[settingindex] = new EntitySetting((byte)numSettingA.Value,s.ValueB);
            }
        }

        private void numSettingB_ValueChanged(object sender,EventArgs e)
        {
            if (!settingdirty)
            {
                EntitySetting s = entity.Settings[settingindex];
                entity.Settings[settingindex] = new EntitySetting(s.ValueA,(int)numSettingB.Value);
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
            victimdirty = true;
            if (victimindex >= entity.Victims.Count)
            {
                victimindex = entity.Victims.Count - 1;
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
                cmdPreviousVictim.Enabled = false;
                cmdNextVictim.Enabled = false;
                cmdInsertVictim.Enabled = false;
                cmdAppendVictim.Enabled = true;
                cmdRemoveVictim.Enabled = false;
                numVictimID.Enabled = false;
            }
            else
            {
                lblVictimIndex.Text = string.Format("{0} / {1}",victimindex + 1,entity.Victims.Count);
                cmdPreviousVictim.Enabled = (victimindex > 0);
                cmdNextVictim.Enabled = (victimindex < entity.Victims.Count - 1);
                cmdInsertVictim.Enabled = true;
                cmdRemoveVictim.Enabled = true;
                cmdAppendVictim.Enabled = false;
                numVictimID.Enabled = true;
                numVictimID.Value = entity.Victims[victimindex].VictimID;
            }
            victimdirty = false;
        }

        private void cmdPreviousVictim_Click(object sender,EventArgs e)
        {
            victimindex--;
            UpdateVictim();
        }

        private void cmdNextVictim_Click(object sender,EventArgs e)
        {
            victimindex++;
            UpdateVictim();
        }

        private void cmdInsertVictim_Click(object sender,EventArgs e)
        {
            entity.Victims.Insert(victimindex,entity.Victims[victimindex]);
            UpdateVictim();
        }

        private void cmdAppendVictim_Click(object sender,EventArgs e)
        {
            entity.Victims.Add(new EntityVictim(0));
            UpdateVictim();
        }

        private void cmdRemoveVictim_Click(object sender,EventArgs e)
        {
            entity.Victims.RemoveAt(victimindex);
            UpdateVictim();
        }

        private void numVictimID_ValueChanged(object sender,EventArgs e)
        {
            if (!victimdirty)
            {
                entity.Victims[victimindex] = new EntityVictim((short)numVictimID.Value);
            }
        }

        private void cmdNextAndRemove_Click(object sender, EventArgs e)
        {
            positionindex++;
            entity.Positions.RemoveAt(positionindex);
            UpdatePosition();
            if (entity.Positions.Count == 0)
                InvalidateNodes();

        }

        private void UpdateLoadListA()
        {
            if (entity.LoadListA != null && entity.LoadListA.Unknown != 0)
            {
                if (loadlistarowindex + 1 > entity.LoadListA.Unknown)
                    loadlistarowindex = entity.LoadListA.Unknown - 1;
                lblMetavalueLoadA.Enabled = true;
                numMetavalueLoadA.Enabled = true;
                lblLoadListRowIndexA.Text = string.Format("{0} / {1}", loadlistarowindex + 1,entity.LoadListA.Unknown);
                numMetavalueLoadA.Value = entity.LoadListA.Rows[loadlistarowindex].MetaValue.Value;
                cmdPrevRowA.Enabled = (loadlistarowindex > 0);
                cmdNextRowA.Enabled = (loadlistarowindex + 1 < entity.LoadListA.Unknown);
                cmdRemoveRowA.Enabled = true;
                if (entity.LoadListA.Rows[loadlistarowindex].Values.Count > 0)
                {
                    if (loadlistaeidindex + 1 > entity.LoadListA.Rows[loadlistarowindex].Values.Count)
                        loadlistaeidindex = entity.LoadListA.Rows[loadlistarowindex].Values.Count - 1;
                    cmdInsertEIDA.Enabled = true;
                    cmdRemoveEIDA.Enabled = true;
                    txtEIDA.Enabled = true;
                    cmdPrevEIDA.Enabled = (loadlistaeidindex > 0);
                    cmdNextEIDA.Enabled = (loadlistaeidindex + 1 < entity.LoadListA.Rows[loadlistarowindex].Values.Count);
                    lblEIDIndexA.Text = string.Format("{0} / {1}", loadlistaeidindex + 1, entity.LoadListA.Rows[loadlistarowindex].Values.Count);
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
                    lblEIDIndexA.Text = "-- / --";
                }
            }
            else
            {
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
            }
        }

        private void cmdPrevEIDA_Click(object sender, EventArgs e)
        {
            loadlistaeidindex--;
            UpdateLoadListA();
        }

        private void cmdNextEIDA_Click(object sender, EventArgs e)
        {
            loadlistaeidindex++;
            UpdateLoadListA();
        }

        private void cmdPrevRowA_Click(object sender, EventArgs e)
        {
            loadlistarowindex--;
            UpdateLoadListA();
        }

        private void cmdNextRowA_Click(object sender, EventArgs e)
        {
            loadlistarowindex++;
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
                entity.LoadListA.Rows[loadlistarowindex].Values.Add(new int());
            }
            UpdateLoadListA();
        }

        private void txtEIDA_TextChanged(object sender, EventArgs e)
        {
            text = true;
            if (txtEIDB.Text.Length == 5 && txtEIDA.Text.Length == 5)
            {
                lblEID1.Visible = false;
            }
            else
            {
                text = false;
                lblEID1.Visible = true;
            }
            for (int i = 0; i < txtEIDA.Text.Length; i++)
            {
                int ii = 0;
                while (ii < 64 && !(txtEIDA.Text[i] == Entry.ENameCharacterSet[ii]))
                {
                    ii++;
                }
                if (ii == 64)
                {
                    text = false;
                    lblEID2.Visible = true;
                    break;
                }
                else
                {
                    lblEID2.Visible = false;
                }
            }
            if (!lblEID2.Visible)
            {
                for (int i = 0; i < txtEIDB.Text.Length; i++)
                {
                    int ii = 0;
                    while (ii < 64 && !(txtEIDB.Text[i] == Entry.ENameCharacterSet[ii]))
                    {
                        ii++;
                    }
                    if (ii == 64)
                    {
                        text = false;
                        lblEID2.Visible = true;
                        break;
                    }
                    else
                    {
                        lblEID2.Visible = false;
                    }
                }
            }
            if (text == true)
            {
                entity.LoadListA.Rows[loadlistarowindex].Values[loadlistaeidindex] = BitConv.FromInt32(Entry.Str2EID(Reverse(txtEIDA.Text)));
                text = false;
            }
        }

        private void cmdRemoveRowA_Click(object sender, EventArgs e)
        {
            entity.LoadListA.Rows.RemoveAt(loadlistarowindex);
            UpdateLoadListA();
        }

        private void cmdInsertRowA_Click(object sender, EventArgs e)
        {
            if (entity.LoadListA == null)
                entity.LoadListA = new EntityT4Property();
            entity.LoadListA.Rows.Add(new EntityPropertyRow<int>());
            entity.LoadListA.Rows[entity.LoadListA.Unknown - 1].MetaValue = 0;
            loadlistarowindex = 0;
            loadlistaeidindex = 0;
            UpdateLoadListA();
        }

        private void numMetavalueLoadA_ValueChanged(object sender, EventArgs e)
        {
            entity.LoadListA.Rows[loadlistarowindex].MetaValue = (short)numMetavalueLoadA.Value;
        }
        
        private void UpdateLoadListB()
        {
            if (entity.LoadListB != null && entity.LoadListB.Unknown != 0)
            {
                if (loadlistbrowindex + 1 > entity.LoadListB.Unknown)
                    loadlistbrowindex = entity.LoadListB.Unknown - 1;
                lblMetavalueLoadB.Enabled = true;
                numMetavalueLoadB.Enabled = true;
                lblLoadListRowIndexB.Text = string.Format("{0} / {1}", loadlistbrowindex + 1, entity.LoadListB.Unknown);
                numMetavalueLoadB.Value = entity.LoadListB.Rows[loadlistbrowindex].MetaValue.Value;
                cmdPrevRowB.Enabled = (loadlistbrowindex > 0);
                cmdNextRowB.Enabled = (loadlistbrowindex + 1 < entity.LoadListB.Unknown);
                cmdRemoveRowB.Enabled = true;
                if (entity.LoadListB.Rows[loadlistbrowindex].Values.Count > 0)
                {
                    if (loadlistbeidindex + 1 > entity.LoadListB.Rows[loadlistbrowindex].Values.Count)
                        loadlistbeidindex = entity.LoadListB.Rows[loadlistbrowindex].Values.Count - 1;
                    cmdInsertEIDB.Enabled = true;
                    cmdRemoveEIDB.Enabled = true;
                    txtEIDB.Enabled = true;
                    cmdPrevEIDB.Enabled = (loadlistbeidindex > 0);
                    cmdNextEIDB.Enabled = (loadlistbeidindex + 1 < entity.LoadListB.Rows[loadlistbrowindex].Values.Count);
                    lblEIDIndexB.Text = string.Format("{0} / {1}", loadlistbeidindex + 1, entity.LoadListB.Rows[loadlistbrowindex].Values.Count);
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
                    lblEIDIndexB.Text = "-- / --";
                }
            }
            else
            {
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
            }
        }

        private void cmdPrevEIDB_Click(object sender, EventArgs e)
        {
            loadlistbeidindex--;
            UpdateLoadListB();
        }

        private void cmdNextEIDB_Click(object sender, EventArgs e)
        {
            loadlistbeidindex++;
            UpdateLoadListB();
        }

        private void cmdPrevRowB_Click(object sender, EventArgs e)
        {
            loadlistbrowindex--;
            UpdateLoadListB();
        }

        private void cmdNextRowB_Click(object sender, EventArgs e)
        {
            loadlistbrowindex++;
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
                entity.LoadListB.Rows[loadlistbrowindex].Values.Add(new int());
            }
            UpdateLoadListB();
        }

        private void txtEIDB_TextChanged(object sender, EventArgs e)
        {
            text = true;
            if (txtEIDB.Text.Length == 5 && txtEIDA.Text.Length == 5)
            {
                lblEID1.Visible = false;
            }
            else
            {
                text = false;
                lblEID1.Visible = true;
            }
            for (int i = 0; i < txtEIDA.Text.Length; i++)
            {
                int ii = 0;
                while (ii < 64 && !(txtEIDA.Text[i] == Entry.ENameCharacterSet[ii]))
                {
                    ii++;
                }
                if (ii == 64)
                {
                    text = false;
                    lblEID2.Visible = true;
                    break;
                }
                else
                {
                    lblEID2.Visible = false;
                }
            }
            if (!lblEID2.Visible)
            {
                for (int i = 0; i < txtEIDB.Text.Length; i++)
                {
                    int ii = 0;
                    while (ii < 64 && !(txtEIDB.Text[i] == Entry.ENameCharacterSet[ii]))
                    {
                        ii++;
                    }
                    if (ii == 64)
                    {
                        text = false;
                        lblEID2.Visible = true;
                        break;
                    }
                    else
                    {
                        lblEID2.Visible = false;
                    }
                }
            }
            if (text == true)
            {
                entity.LoadListB.Rows[loadlistbrowindex].Values[loadlistbeidindex] = BitConv.FromInt32(Entry.Str2EID(Reverse(txtEIDB.Text)));
                text = false;
            }
        }

        private void cmdRemoveRowB_Click(object sender, EventArgs e)
        {
            entity.LoadListB.Rows.RemoveAt(loadlistbrowindex);
            UpdateLoadListB();
        }

        private void cmdInsertRowB_Click(object sender, EventArgs e)
        {
            if (entity.LoadListB == null)
                entity.LoadListB = new EntityT4Property();
            entity.LoadListB.Rows.Add(new EntityPropertyRow<int>());
            entity.LoadListB.Rows[entity.LoadListB.Unknown - 1].MetaValue = 0;
            loadlistbrowindex = 0;
            loadlistbeidindex = 0;
            UpdateLoadListB();
        }

        private void numMetavalueLoadB_ValueChanged(object sender, EventArgs e)
        {
            entity.LoadListB.Rows[loadlistbrowindex].MetaValue = (short)numMetavalueLoadB.Value;
        }

        private void UpdateDrawListA()
        {
            if (entity.DrawListA != null && entity.DrawListA.Unknown != 0)
            {
                if (drawlistarowindex + 1 > entity.DrawListA.Unknown)
                    drawlistarowindex = entity.DrawListA.Unknown - 1;
                lblMetavalueDrawA.Enabled = true;
                numMetavalueDrawA.Enabled = true;
                lblDrawListRowIndexA.Text = string.Format("{0} / {1}", drawlistarowindex + 1, entity.DrawListA.Unknown);
                numMetavalueDrawA.Value = entity.DrawListA.Rows[drawlistarowindex].MetaValue.Value;
                cmdPrevRowDrawA.Enabled = (drawlistarowindex > 0);
                cmdNextRowDrawA.Enabled = (drawlistarowindex + 1 < entity.DrawListA.Unknown);
                cmdRemoveRowDrawA.Enabled = true;
                if (entity.DrawListA.Rows[drawlistarowindex].Values.Count > 0)
                {
                    if (drawlistaentityindex + 1 > entity.DrawListA.Rows[drawlistarowindex].Values.Count)
                        drawlistaentityindex = entity.DrawListA.Rows[drawlistarowindex].Values.Count - 1;
                    cmdInsertEntityA.Enabled = true;
                    cmdRemoveEntityA.Enabled = true;
                    lblEntityA.Enabled = true;
                    numEntityA.Enabled = true;
                    cmdPrevEntityA.Enabled = (drawlistaentityindex > 0);
                    cmdNextEntityA.Enabled = (drawlistaentityindex + 1 < entity.DrawListA.Rows[drawlistarowindex].Values.Count);
                    lblEntityIndexA.Text = string.Format("{0} / {1}", drawlistaentityindex + 1, entity.DrawListA.Rows[drawlistarowindex].Values.Count);
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
            drawlistaentityindex--;
            UpdateDrawListA();
        }

        private void cmdNextEntityA_Click(object sender, EventArgs e)
        {
            drawlistaentityindex++;
            UpdateDrawListA();
        }

        private void cmdPrevRowDrawA_Click(object sender, EventArgs e)
        {
            drawlistarowindex--;
            UpdateDrawListA();
        }

        private void cmdNextRowDrawA_Click(object sender, EventArgs e)
        {
            drawlistarowindex++;
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
                entity.DrawListA.Rows[drawlistarowindex].Values.Add(new int());
            }
            UpdateDrawListA();
        }

        private void numEntityA_ValueChanged(object sender, EventArgs e)
        {
            foreach (Chunk chunk in controller.ZoneEntryController.EntryChunkController.NSFController.NSF.Chunks)
            {
                if (chunk is EntryChunk)
                {
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is ZoneEntry)
                        {
                            foreach (Entity otherentity in ((ZoneEntry)entry).Entities)
                            {
                                if (otherentity.ID.HasValue && otherentity.ID.Value == numEntityA.Value)
                                {
                                    for (int i = 0; i < controller.ZoneEntryController.ZoneEntry.Unknown1[0x190];i++)
                                    {
                                        if (entry.EName == Entry.EIDToEName(BitConv.FromInt32(controller.ZoneEntryController.ZoneEntry.Unknown1,0x194 + i * 4)))
                                        {
                                            entity.DrawListA.Rows[drawlistarowindex].Values[drawlistaentityindex] = (int)(i | (otherentity.ID << 8) | ((((ZoneEntry)entry).Entities.IndexOf(otherentity) - BitConv.FromInt32(((ZoneEntry)entry).Unknown1, 0x188)) << 24));
                                        }
                                    }
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
            if (entity.DrawListA == null)
                entity.DrawListA = new EntityInt32Property();
            entity.DrawListA.Rows.Add(new EntityPropertyRow<int>());
            entity.DrawListA.Rows[entity.DrawListA.Unknown - 1].MetaValue = 0;
            UpdateDrawListA();
        }

        private void numMetavalueDrawA_ValueChanged(object sender, EventArgs e)
        {
            entity.DrawListA.Rows[drawlistarowindex].MetaValue = (short)numMetavalueDrawA.Value;
        }

        private void UpdateDrawListB()
        {
            if (entity.DrawListB != null && entity.DrawListB.Unknown != 0)
            {
                if (drawlistbrowindex + 1 > entity.DrawListB.Unknown)
                    drawlistbrowindex = entity.DrawListB.Unknown - 1;
                lblMetavalueDrawB.Enabled = true;
                numMetavalueDrawB.Enabled = true;
                lblDrawListRowIndexB.Text = string.Format("{0} / {1}", drawlistbrowindex + 1, entity.DrawListB.Unknown);
                numMetavalueDrawB.Value = entity.DrawListB.Rows[drawlistbrowindex].MetaValue.Value;
                cmdPrevRowDrawB.Enabled = (drawlistbrowindex > 0);
                cmdNextRowDrawB.Enabled = (drawlistbrowindex + 1 < entity.DrawListB.Unknown);
                cmdRemoveRowDrawB.Enabled = true;
                if (entity.DrawListB.Rows[drawlistbrowindex].Values.Count > 0)
                {
                    if (drawlistbentityindex + 1 > entity.DrawListB.Rows[drawlistbrowindex].Values.Count)
                        drawlistbentityindex = entity.DrawListB.Rows[drawlistbrowindex].Values.Count - 1;
                    cmdInsertEntityB.Enabled = true;
                    cmdRemoveEntityB.Enabled = true;
                    lblEntityB.Enabled = true;
                    numEntityB.Enabled = true;
                    cmdPrevEntityB.Enabled = (drawlistbentityindex > 0);
                    cmdNextEntityB.Enabled = (drawlistbentityindex + 1 < entity.DrawListB.Rows[drawlistbrowindex].Values.Count);
                    lblEntityIndexB.Text = string.Format("{0} / {1}", drawlistbentityindex + 1, entity.DrawListB.Rows[drawlistbrowindex].Values.Count);
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
            drawlistbentityindex--;
            UpdateDrawListB();
        }

        private void cmdNextEntityB_Click(object sender, EventArgs e)
        {
            drawlistbentityindex++;
            UpdateDrawListB();
        }

        private void cmdPrevRowDrawB_Click(object sender, EventArgs e)
        {
            drawlistbrowindex--;
            UpdateDrawListB();
        }

        private void cmdNextRowDrawB_Click(object sender, EventArgs e)
        {
            drawlistbrowindex++;
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
            foreach (Chunk chunk in controller.ZoneEntryController.EntryChunkController.NSFController.NSF.Chunks)
            {
                if (chunk is EntryChunk)
                {
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is ZoneEntry)
                        {
                            foreach (Entity otherentity in ((ZoneEntry)entry).Entities)
                            {
                                if (otherentity.ID.HasValue && otherentity.ID.Value == numEntityB.Value)
                                {
                                    for (int i = 0; i < controller.ZoneEntryController.ZoneEntry.Unknown1[0x190]; i++)
                                    {
                                        if (entry.EName == Entry.EIDToEName(BitConv.FromInt32(controller.ZoneEntryController.ZoneEntry.Unknown1, 0x194 + i * 4)))
                                        {
                                            entity.DrawListB.Rows[drawlistbrowindex].Values[drawlistbentityindex] = (int)(i | (otherentity.ID << 8) | ((((ZoneEntry)entry).Entities.IndexOf(otherentity) - BitConv.FromInt32(((ZoneEntry)entry).Unknown1, 0x188)) << 24));
                                        }
                                    }
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
            if (entity.DrawListB == null)
                entity.DrawListB = new EntityInt32Property();
            entity.DrawListB.Rows.Add(new EntityPropertyRow<int>());
            entity.DrawListB.Rows[entity.DrawListB.Unknown - 1].MetaValue = 0;
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
                numDDASettings.Value = entity.DDASettings.Value;
            }
            numDDASettings.Enabled = entity.DDASettings.HasValue;
            chkDDASettings.Checked = entity.DDASettings.HasValue;
        }

        private void chkDDASettings_CheckedChanged(object sender, EventArgs e)
        {
            numDDASettings.Enabled = chkDDASettings.Checked;
            if (chkDDASettings.Checked)
            {
                entity.DDASettings = (int)numDDASettings.Value;
            }
            else
            {
                entity.DDASettings = null;
            }
        }

        private void numDDASettings_ValueChanged(object sender, EventArgs e)
        {
            entity.DDASettings = (int)numDDASettings.Value;
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
                txtSLST.Text = entity.SLST;
                chkSLST.Checked = true;
            }
            else
            {
                txtSLST.Enabled = false;
                chkSLST.Checked = false;
            }
        }

        private void chkSLST_CheckedChanged(object sender, EventArgs e)
        {
            txtSLST.Enabled = chkSLST.Checked;
            if (chkSLST.Checked)
            {
                if (txtSLST.Text.Length == 5)
                {
                    text = true;
                    lblSLST1.Visible = false;
                }
                else
                {
                    text = false;
                    lblSLST1.Visible = true;
                }
                for (int i = 0;i < txtSLST.Text.Length;i++)
                {
                    int ii = 0;
                    while (ii < 64 && !(txtSLST.Text[i] == Entry.ENameCharacterSet[ii]))
                    {
                        ii++;
                    }
                    if (ii == 64)
                    {
                        i = txtSLST.Text.Length;
                        text = false;
                        lblSLST2.Visible = true;
                    }
                    else
                    {
                        lblSLST2.Visible = false;
                    }
                }
                if (text == true)
                {
                    entity.SLST = Reverse(txtSLST.Text);
                    text = false;
                }

            }
            else
            {
                entity.SLST = null;
                lblSLST1.Visible = false;
                lblSLST2.Visible = false;
            }
            InvalidateNodes();
        }

        private void txtSLST_TextChanged(object sender, EventArgs e)
        {
            if (txtSLST.Text.Length == 5)
            {
                text = true;
                lblSLST1.Visible = false;
            }
            else
            {
                text = false;
                lblSLST1.Visible = true;
            }
            for (int i = 0; i < txtSLST.Text.Length; i++)
            {
                int ii = 0;
                while (ii < 64 && !(txtSLST.Text[i] == Entry.ENameCharacterSet[ii]))
                {
                    ii++;
                }
                if (ii == 64)
                {
                    i = txtSLST.Text.Length;
                    text = false;
                    lblSLST2.Visible = true;
                }
                else
                {
                    lblSLST2.Visible = false;
                }
            }
            if (text == true)
            {
                entity.SLST = Reverse(txtSLST.Text);
                text = false;
            }
            InvalidateNodes();
        }

        private void tabGeneral_Enter(object sender, EventArgs e)
        {
            UpdateName();
            UpdatePosition();
            UpdateType();
            UpdateSubtype();
            UpdateSettings();
            UpdateID();
        }

        private void tabSpecial_Enter(object sender, EventArgs e)
        {
            UpdateVictim();
            UpdateScaling();
            UpdateBoxCount();
            UpdateDDASection();
            UpdateDDASettings();
            UpdateOtherSettings();
        }

        private void tabCamera_Enter(object sender, EventArgs e)
        {
            UpdateSLST();
        }

        private void tabLoadLists_Enter(object sender, EventArgs e)
        {
            UpdateLoadListA();
            UpdateLoadListB();
        }

        private void tabDrawLists_Enter(object sender, EventArgs e)
        {
            UpdateDrawListA();
            UpdateDrawListB();
        }

        private void txtEIDA_LostFocus(object sender, EventArgs e)
        {
            UpdateLoadListA();
        }

        private void txtEIDB_LostFocus(object sender, EventArgs e)
        {
            UpdateLoadListB();
        }
    }
}
