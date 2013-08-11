using System;
using System.Windows.Forms;
using DazCamUI.Controller;
using System.Linq;

namespace DazCamUI.UI
{
    public partial class Offsets : Form
    {
        #region Declarations

        private Machine _machine;
        private bool _suppressEvent = false;
        private string _axisToFind = "";

        #endregion

        #region Constructors

        public Offsets()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Offsets_Load(object sender, EventArgs e)
        {
            _machine = Machine.GetMachine();
            _machine.OnEdgeTouched += _machine_OnEdgeTouched;
            _machine.OnLocationChanged += _machine_OnLocationChanged;

            PopulateWorkspaceOffsets();
            UpdateLocation();
        }

        private void Offsets_FormClosed(object sender, FormClosedEventArgs e)
        {
            CommitCurrentOffset();
            _machine.Settings.Save();
        }

        private void listBoxOffsets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressEvent) return;
            PopulateCurrentWorkplaceOffset();
        }

        private void btnOffsetDelete_Click(object sender, EventArgs e)
        {
            var item = listBoxOffsets.SelectedItem as WorkspaceOffset;

            if (item == null) return;
            if (MessageBox.Show("Delete this offset?", "Are you sure?", MessageBoxButtons.YesNo) == DialogResult.No) return;

            listBoxOffsets.Items.Remove(item);
            _machine.Settings.WorkspaceOffsets.Remove(item);
        }

        private void btnOffsetAdd_Click(object sender, EventArgs e)
        {
            var newOffset = new WorkspaceOffset()
            {
                Name = "New Offset",
                Active = true,
                Type = listBoxGCodeIdentifiers.SelectedItem as OffsetType
            };

            var addedIndex = listBoxOffsets.Items.Add(newOffset);
            _suppressEvent = true;
            listBoxOffsets.SetItemCheckState(addedIndex, CheckState.Checked);
            _suppressEvent = false;

            listBoxOffsets.SelectedItem = newOffset;
            _machine.Settings.WorkspaceOffsets.Add(newOffset);
        }

        private void txtOffsetProperty_Leave(object sender, EventArgs e)
        {
            CommitCurrentOffset();
            RefreshListItem();
        }

        private void chkOffsetInvert_Clicked(object sender, EventArgs e)
        {
            CommitCurrentOffset();
        }

        void _machine_OnEdgeTouched(double touchedAt)
        {
            string coordinate = touchedAt.ToString("0.0000");
            if (_axisToFind == "X") txtFoundX.Text = coordinate;
            if (_axisToFind == "Y") txtFoundY.Text = coordinate;
            if (_axisToFind == "Z") txtFoundZ.Text = coordinate;
        }

        void _machine_OnLocationChanged(Coordinate machineLocation, int zServoAngle)
        {
            UpdateLocation();
        }

        private void btnJog_Click(object sender, EventArgs e)
        {
            JogMode();
        }

        private void Offsets_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
            if (e.KeyCode == Keys.J && e.Control) JogMode();
        }

        private void listBoxGCodeIdentifiers_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateWorkspaceOffsets();
        }

        private void listBoxOffsets_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_suppressEvent) return;

            var item = listBoxOffsets.SelectedItem as WorkspaceOffset;
            item.Active = !(e.CurrentValue == CheckState.Checked);
        }

        private void btnFindAll_Click(object sender, EventArgs e)
        {
            FindAxis("Z");
            FindAxis("X");
            FindAxis("Y");
        }

        private void btnFindX_Click(object sender, EventArgs e)
        {
            FindAxis("X");
        }

        private void btnFindY_Click(object sender, EventArgs e)
        {
            FindAxis("Y");
        }

        private void btnFindZ_Click(object sender, EventArgs e)
        {
            FindAxis("Z");
        }

        private void btnCopyX_Click(object sender, EventArgs e)
        {
            if (rbCurrentPosition.Checked) txtOffsetX.Text = txtCurrentX.Text;
            if (rbFoundPosition.Checked) txtOffsetX.Text = txtFoundX.Text;
            CommitCurrentOffset();
        }

        private void btnCopyY_Click(object sender, EventArgs e)
        {
            if (rbCurrentPosition.Checked) txtOffsetY.Text = txtCurrentY.Text;
            if (rbFoundPosition.Checked) txtOffsetY.Text = txtFoundY.Text;
            CommitCurrentOffset();
        }

        private void btnCopyZ_Click(object sender, EventArgs e)
        {
            if (rbCurrentPosition.Checked) txtOffsetZ.Text = txtCurrentZ.Text;
            if (rbFoundPosition.Checked) txtOffsetZ.Text = txtFoundZ.Text;
            CommitCurrentOffset();
        }

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            btnCopyX_Click(sender, e);
            btnCopyY_Click(sender, e);
            btnCopyZ_Click(sender, e);
        }

        private void btnAddToX_Click(object sender, EventArgs e)
        {
            if (rbCurrentPosition.Checked) txtOffsetX.Text = AddTwoCoordinates(txtCurrentX.Text, txtOffsetX.Text);
            if (rbFoundPosition.Checked) txtOffsetX.Text = AddTwoCoordinates(txtFoundX.Text, txtOffsetX.Text);
            CommitCurrentOffset();
        }

        private void btnAddToY_Click(object sender, EventArgs e)
        {
            if (rbCurrentPosition.Checked) txtOffsetY.Text = AddTwoCoordinates(txtCurrentY.Text, txtOffsetY.Text);
            if (rbFoundPosition.Checked) txtOffsetY.Text = AddTwoCoordinates(txtFoundY.Text, txtOffsetY.Text);
            CommitCurrentOffset();
        }

        private void btnAddToZ_Click(object sender, EventArgs e)
        {
            if (rbCurrentPosition.Checked) txtOffsetZ.Text = AddTwoCoordinates(txtCurrentZ.Text, txtOffsetZ.Text);
            if (rbFoundPosition.Checked) txtOffsetZ.Text = AddTwoCoordinates(txtFoundZ.Text, txtOffsetZ.Text);
            CommitCurrentOffset();
        }

        private void btnAddToAll_Click(object sender, EventArgs e)
        {
            btnAddToX_Click(sender, e);
            btnAddToY_Click(sender, e);
            btnAddToZ_Click(sender, e);
        }

        private void btnSubtractXFrom_Click(object sender, EventArgs e)
        {
            if (rbCurrentPosition.Checked) txtOffsetX.Text = SubtractTargetFromSourceCoordinates(txtCurrentX.Text, txtOffsetX.Text);
            if (rbFoundPosition.Checked) txtOffsetX.Text = SubtractTargetFromSourceCoordinates(txtFoundX.Text, txtOffsetX.Text);
            CommitCurrentOffset();
        }

        private void btnSubtractYFrom_Click(object sender, EventArgs e)
        {
            if (rbCurrentPosition.Checked) txtOffsetY.Text = SubtractTargetFromSourceCoordinates(txtCurrentY.Text, txtOffsetY.Text);
            if (rbFoundPosition.Checked) txtOffsetY.Text = SubtractTargetFromSourceCoordinates(txtFoundY.Text, txtOffsetY.Text);
            CommitCurrentOffset();
        }

        private void btnSubtractZFrom_Click(object sender, EventArgs e)
        {
            if (rbCurrentPosition.Checked) txtOffsetZ.Text = SubtractTargetFromSourceCoordinates(txtCurrentZ.Text, txtOffsetZ.Text);
            if (rbFoundPosition.Checked) txtOffsetZ.Text = SubtractTargetFromSourceCoordinates(txtFoundZ.Text, txtOffsetZ.Text);
            CommitCurrentOffset();
        }

        private void btnSubtractFromAll_Click(object sender, EventArgs e)
        {
            btnSubtractXFrom_Click(sender, e);
            btnSubtractYFrom_Click(sender, e);
            btnSubtractZFrom_Click(sender, e);
        }

        #endregion

        #region Private Methods

        private void UpdateLocation()
        {
            txtCurrentX.Text = _machine.Location.X.ToString("0.0000");
            txtCurrentY.Text = _machine.Location.Y.ToString("0.0000");
            txtCurrentZ.Text = _machine.Location.Z.ToString("0.0000");
        }

        private void PopulateWorkspaceOffsets()
        {
            // First, populate the list of GCode Identifiers if it isn't already loaded
            if (listBoxGCodeIdentifiers.Items.Count == 0)
            {
                listBoxGCodeIdentifiers.Items.AddRange(OffsetType.GetAllGCodeIdentifiers().ToArray());
                listBoxGCodeIdentifiers.SelectedIndex = 0;
            }

            // Next, populate the offsets based on the selected GCode Identifier
            listBoxOffsets.Items.Clear();
            var selectedOffsetType = listBoxGCodeIdentifiers.SelectedItem as OffsetType;

            if (selectedOffsetType != null)
            {
                var filteredOffsetList = from o in _machine.Settings.WorkspaceOffsets
                                         where o.Type.GCodeIdentifier == selectedOffsetType.GCodeIdentifier
                                         select o;

                listBoxOffsets.Items.AddRange(filteredOffsetList.ToArray());

                _suppressEvent = true;          // Prevent chain reactions
                for (int i = 0; i < listBoxOffsets.Items.Count; i++)
                {
                    if (((WorkspaceOffset)listBoxOffsets.Items[i]).Active)
                        listBoxOffsets.SetItemCheckState(i, CheckState.Checked);
                }
                _suppressEvent = false;

                if (listBoxOffsets.Items.Count > 0) listBoxOffsets.SelectedIndex = 0;
            }

            PopulateCurrentWorkplaceOffset();
        }

        private void PopulateCurrentWorkplaceOffset()
        {
            var item = listBoxOffsets.SelectedItem as WorkspaceOffset;
            bool enabled = false;

            if (item == null)
            {
                txtOffsetName.Text = "";
                txtOffsetX.Text = "";
                txtOffsetY.Text = "";
                txtOffsetZ.Text = "";
                chkOffsetInvertX.Checked = false;
                chkOffsetInvertY.Checked = false;
                chkOffsetInvertZ.Checked = false;
            }

            else
            {
                enabled = true;
                txtOffsetName.Text = item.Name;
                txtOffsetX.Text = item.X.ToString("0.0000");
                txtOffsetY.Text = item.Y.ToString("0.0000");
                txtOffsetZ.Text = item.Z.ToString("0.0000");
                chkOffsetInvertX.Checked = item.InvertX;
                chkOffsetInvertY.Checked = item.InvertY;
                chkOffsetInvertZ.Checked = item.InvertZ;
            }

            txtOffsetName.Enabled = enabled;
            txtOffsetX.Enabled = enabled;
            txtOffsetY.Enabled = enabled;
            txtOffsetZ.Enabled = enabled;
            chkOffsetInvertX.Enabled = enabled;
            chkOffsetInvertY.Enabled = enabled;
            chkOffsetInvertZ.Enabled = enabled;
            btnOffsetDelete.Enabled = enabled;
        }

        private void CommitCurrentOffset()
        {
            var item = listBoxOffsets.SelectedItem as WorkspaceOffset;
            if (item == null) return;

            double value;

            item.Name = txtOffsetName.Text.Trim();

            if (!double.TryParse(txtOffsetX.Text, out value)) value = 0;
            item.X = value;

            if (!double.TryParse(txtOffsetY.Text, out value)) value = 0;
            item.Y = value;

            if (!double.TryParse(txtOffsetZ.Text, out value)) value = 0;
            item.Z = value;

            item.InvertX = chkOffsetInvertX.Checked;
            item.InvertY = chkOffsetInvertY.Checked;
            item.InvertZ = chkOffsetInvertZ.Checked;
        }

        /// <summary>
        /// Refreshes the text in the listbox to match the changes in the fields
        /// </summary>
        private void RefreshListItem()
        {
            var item = listBoxOffsets.SelectedItem as WorkspaceOffset;
            if (item == null) return;

            _suppressEvent = true;          // Prevent a chain reaction
            listBoxOffsets.Items[listBoxOffsets.SelectedIndex] = item;
            _suppressEvent = false;
        }

        private void FindAxis(string axis)
        {
            _axisToFind = axis;
            _machine.FindEdge(axis);
            rbFoundPosition.Checked = true;
        }

        private string AddTwoCoordinates(string source, string target)
        {
            double sourceValue;
            double targetValue;

            if (!double.TryParse(source, out sourceValue)) sourceValue = 0;
            if (!double.TryParse(target, out targetValue)) targetValue = 0;

            return (sourceValue + targetValue).ToString("0.0000");
        }

        private string SubtractTargetFromSourceCoordinates(string source, string target)
        {
            double sourceValue;
            double targetValue;

            if (!double.TryParse(source, out sourceValue)) sourceValue = 0;
            if (!double.TryParse(target, out targetValue)) targetValue = 0;

            return (sourceValue - targetValue).ToString("0.0000");
        }

        #endregion

        #region Private Methods

        private void JogMode()
        {
            new Jog().ShowDialog(this);
            rbCurrentPosition.Checked = true;
        }

        #endregion
    }
}
