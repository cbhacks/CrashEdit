using Crash;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

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

        public EntityBox(EntityController controller)
        {
            this.controller = controller;
            entity = controller.Entity;
            InitializeComponent();
            UpdateName();
            UpdatePosition();
            UpdateSettings();
            UpdateID();
            UpdateType();
            UpdateSubtype();
            UpdateBoxCount();
            UpdateVictim();
            //UpdateDrawLists();
            positionindex = 0;
            victimindex = 0;
        }

        private void InvalidateNodes()
        {
            controller.InvalidateNode();
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
            InvalidateNodes();
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
            InvalidateNodes();
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
            InvalidateNodes();
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
            InvalidateNodes();
        }

        private void numBoxCount_ValueChanged(object sender,EventArgs e)
        {
            entity.BoxCount = new EntitySetting(0,(int)numBoxCount.Value);
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

        /*private void UpdateLoadListA()
        {
            loadlistadirty = true;
            if (loadlistaindex >= entity.LoadListA.Count)
            {
                loadlistaindex = entity.LoadListA.Count - 1;
            }
            // Do not make this else if,
            // sometimes both will run.
            // (this is intentional)
            if (loadlistaindex < 0)
            {
                loadlistaindex = 0;
            }
            // Do not remove this either
            if (loadlistaindex >= entity.LoadListA.Count)
            {
                lblEIDIndexA.Text = "-- / --";
                cmdPrevEIDA.Enabled = false;
                cmdNextEIDA.Enabled = false;
                cmdInsertEIDA.Enabled = false;
                cmdRemoveEIDA.Enabled = false;
                txtEIDA.Enabled = false;
            }
            else
            {
                lblEIDIndexA.Text = string.Format("{0} / {1}", loadlistaindex + 1, entity.LoadListA.Count);
                cmdPrevEIDA.Enabled = (loadlistaindex > 0);
                cmdNextEIDA.Enabled = (loadlistaindex < entity.LoadListA.Count - 1);
                cmdInsertEIDA.Enabled = true;
                cmdRemoveEIDA.Enabled = true;
                cmdNextAndRemove.Enabled = (loadlistaindex < entity.LoadListA.Count - 1);
                txtEIDA.Enabled = true;
                txtEIDA.Text = Entry.EIDToEName(entity.LoadListA[loadlistaindex]);
            }
            loadlistadirty = false;
        }

        private void UpdateLoadListB()
        {
            loadlistbdirty = true;
            if (loadlistbindex >= entity.LoadListB.Count)
            {
                loadlistbindex = entity.LoadListB.Count - 1;
            }
            // Do not make this else if,
            // sometimes both will run.
            // (this is intentional)
            if (loadlistbindex < 0)
            {
                loadlistbindex = 0;
            }
            // Do not remove this either
            if (loadlistbindex >= entity.LoadListB.Count)
            {
                lblEIDIndexA.Text = "-- / --";
                cmdPrevEIDB.Enabled = false;
                cmdNextEIDB.Enabled = false;
                cmdInsertEIDB.Enabled = false;
                cmdRemoveEIDB.Enabled = false;
                txtEIDB.Enabled = false;
            }
            else
            {
                lblEIDIndexA.Text = string.Format("{0} / {1}", loadlistbindex + 1, entity.LoadListB.Count);
                cmdPrevEIDB.Enabled = (loadlistbindex > 0);
                cmdNextEIDB.Enabled = (loadlistbindex < entity.LoadListB.Count - 1);
                cmdInsertEIDB.Enabled = true;
                cmdRemoveEIDB.Enabled = true;
                cmdNextAndRemove.Enabled = (loadlistbindex < entity.LoadListB.Count - 1);
                txtEIDB.Enabled = true;
                txtEIDB.Text = Entry.EIDToEName(entity.LoadListB[loadlistbindex]);
            }
            loadlistbdirty = false;
        }

        private void cmdPrevEIDA_Click(object sender, EventArgs e)
        {
            loadlistaindex--;
            UpdateLoadListA();
        }

        private void cmdPrevEIDB_Click(object sender, EventArgs e)
        {
            loadlistbindex--;
            UpdateLoadListB();
        }

        private void cmdNextEIDA_Click(object sender, EventArgs e)
        {
            loadlistaindex++;
            UpdateLoadListA();
        }

        private void cmdNextEIDB_Click(object sender, EventArgs e)
        {
            loadlistbindex++;
            UpdateLoadListB();
        }

        private void cmdRemoveEIDA_Click(object sender, EventArgs e)
        {
            entity.LoadListA.RemoveAt(loadlistaindex);
            UpdateLoadListA();
        }

        private void cmdRemoveEIDB_Click(object sender, EventArgs e)
        {
            entity.LoadListB.RemoveAt(loadlistbindex);
            UpdateLoadListB();
        }

        private void cmdInsertEIDA_Click(object sender, EventArgs e)
        {
            entity.LoadListA.Insert(loadlistaindex,entity.LoadListA[loadlistaindex]);
            UpdateLoadListA();
        }

        private void cmdInsertEIDB_Click(object sender, EventArgs e)
        {
            entity.LoadListA.Insert(loadlistaindex, entity.LoadListA[loadlistaindex]);
            UpdateLoadListA();
        }

        private void cmdClearEIDA_Click(object sender, EventArgs e)
        {
            entity.LoadListA.Clear();
            UpdateLoadListA();
        }

        private void cmdClearEIDB_Click(object sender, EventArgs e)
        {
            entity.LoadListB.Clear();
            UpdateLoadListB();
        }

        private void cmdAppendEIDA_Click(object sender, EventArgs e)
        {
            loadlistaindex = entity.LoadListA.Count;
            if (entity.LoadListA.Count > 0)
            {
                entity.LoadListA.Add(entity.LoadListA[loadlistaindex - 1]);
            }
            else
            {
                entity.LoadListA.Add(new int());
            }
            UpdateLoadListA();
            if (entity.LoadListA.Count == 1)
                InvalidateNodes();
        }

        private void cmdAppendEIDB_Click(object sender, EventArgs e)
        {
            loadlistbindex = entity.LoadListB.Count;
            if (entity.LoadListB.Count > 0)
            {
                entity.LoadListB.Add(entity.LoadListB[loadlistbindex - 1]);
            }
            else
            {
                entity.LoadListB.Add(new int());
            }
            UpdateLoadListB();
            if (entity.LoadListB.Count == 1)
                InvalidateNodes();
        }*/

        /*private void UpdateDrawLists()
        {
            foreach (EntityPropertyRow<int> drawlist in entity.DrawListA)
            {
                foreach (int value in drawlist.Values)
                {
                    if ((value & 0xFFFF00) >> 8 == entity.ID.Value)
                    {
                        unchecked
                        {
                            drawlist.Values.Add((value & 0xFF) | (maxid << 8) | (newindex << 24));
                        }
                        break;
                    }
                }
                if (drawlist.Values.Contains(entity.ID.Value))
                {
                    drawlist.Values.Add(maxid);
                }
            }
        }*/
    }
}
