using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using DazCamUI.Controller;
using DazCamUI.GCode;

namespace DazCamUI
{
    public partial class Positions : Form
    {
        #region Declarations

        private Machine _machine;
        private ControllerSettings _settings;

        private string _gCodeFilename = "";
        private bool _executingGCode = false;

        #endregion

        #region Constructor

        public Positions()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void Positions_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel.Text = "";

            _machine = Machine.GetMachine();
            _settings = _machine.Settings;

            // Subscribe to events
            _machine.OnCommandResponse += Machine_OnCommandResponse;
            _machine.OnCommandSending += Machine_OnCommandSending;
            _machine.OnLocationChanged += Machine_OnLocationChanged;
            _machine.OnEStopNotification += Machine_OnEStopNotification;

            txtSpeed.Text = _machine.Settings.FeedRateManual.ToString();

            UpdateCurrentPositionDisplay();
            UpdateDisplayLabels();

            PopulateOffsetSetting();
        }

        void Machine_OnEStopNotification(bool eStopActive)
        {
            Invoke(new MethodInvoker(() =>
            {
                btnClearEStop.Visible = eStopActive;
                EnableOrDisableMotionFunctions();
            }));
        }

        private void Machine_OnLocationChanged(Coordinate machineLocation, int zServoAngle)
        {
            Invoke(new MethodInvoker(() =>
            {
                UpdateCurrentPositionDisplay();
            }));
        }

        private void iPMCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new UI.IPMCalculator().ShowDialog(this);
        }

        private void Machine_OnCommandSending(string commandToBeSent)
        {
            Invoke(new MethodInvoker(() =>
            {
                LogText(commandToBeSent);
            }));
        }

        private void Machine_OnCommandResponse(bool isError, string responseText)
        {
            Invoke(new MethodInvoker(() =>
            {
                LogText(responseText);
            }));
        }

        private void btnMoveDelta_Click(object sender, EventArgs e)
        {
            double inchesX;
            double inchesY;
            double inchesZ;

            if (!double.TryParse(txtMoveDeltaX.Text, out inchesX)) inchesX = 0;
            if (!double.TryParse(txtMoveDeltaY.Text, out inchesY)) inchesY = 0;
            if (!double.TryParse(txtMoveDeltaZ.Text, out inchesZ)) inchesZ = 0;

            _machine.EnableStepperDrivers(true);
            _machine.MoveTo(inchesX, inchesY, inchesZ, true, chkLinearInterpolation.Checked, GetSelectedSpeed());
            _machine.EnableStepperDrivers(false);
        }

        private void btnGoto_Click(object sender, EventArgs e)
        {
            double gotoX;
            double gotoY;
            double gotoZ;

            if (!double.TryParse(txtCurrentX.Text, out gotoX)) gotoX = 0;
            if (!double.TryParse(txtCurrentY.Text, out gotoY)) gotoY = 0;
            if (!double.TryParse(txtCurrentZ.Text, out gotoZ)) gotoZ = 0;

            _machine.EnableStepperDrivers(true);
            _machine.MoveTo(gotoX, gotoY, gotoZ, false, chkLinearInterpolation.Checked, GetSelectedSpeed());
            _machine.EnableStepperDrivers(false);
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            _machine.Ping();
        }

        private void chkEnableSteppers_CheckedChanged(object sender, EventArgs e)
        {
            _machine.EnableStepperDrivers(chkEnableSteppers.Checked);
        }

        private void btnClearResponses_Click(object sender, EventArgs e)
        {
            txtResponse.Text = "";
        }

        private void txtMoveDeltaX_Enter(object sender, EventArgs e)
        {
            txtMoveDeltaX.SelectAll();
        }

        private void txtMoveDeltaY_Enter(object sender, EventArgs e)
        {
            txtMoveDeltaY.SelectAll();
        }

        private void txtMoveDeltaZ_Enter(object sender, EventArgs e)
        {
            txtMoveDeltaZ.SelectAll();
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            _machine.RequestStatus();
        }

        private void trackZServo_KeyUp(object sender, KeyEventArgs e)
        {
            _machine.MoveZServo(180 - trackZServo.Value);
        }

        private void trackZServo_MouseUp(object sender, MouseEventArgs e)
        {
            _machine.MoveZServo(180 - trackZServo.Value);
        }

        private void trackZServo_ValueChanged(object sender, EventArgs e)
        {
            txtZServoAngle.Text = (180 - trackZServo.Value).ToString();
        }

        private void btnClearEStop_Click(object sender, EventArgs e)
        {
            _machine.ClearEStop();
        }

        private void openGCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                _gCodeFilename = openFileDialog.FileName;           //TODO: Add to MRU
                txtGCode.Text = File.ReadAllText(_gCodeFilename);
            }
        }

        private void findHomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _machine.FindHome(true, true, true);
        }

        private void splitContainer1_Panel2_Resize(object sender, EventArgs e)
        {
            txtResponse.Height = splitContainer1.Panel2.ClientSize.Height - (btnClearResponses.Top + btnClearResponses.Height + 10);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _settings.Save();
            this.Close();
        }

        private void saveMachineSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMachineSettings();
        }

        private void Positions_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveMachineSettings();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.Clear();
            _gCodeFilename = "";
        }

        private void saveGCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCodeSaveToFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCodeSaveAsToFile();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.SelectAll();
        }

        private void remarkSelectedLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemarkSelectedLines(true);
        }

        private void unremarkSelectedLinesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemarkSelectedLines(false);
        }

        private void btnSetTop_Click(object sender, EventArgs e)
        {
            _settings.ZServoTopPosition = Convert.ToInt32(txtZServoAngle.Text); ;
            UpdateDisplayLabels();
        }

        private void btnSetBottom_Click(object sender, EventArgs e)
        {
            _settings.ZServoBottomPosition = Convert.ToInt32(txtZServoAngle.Text);
            UpdateDisplayLabels();
        }

        private void txtSpeed_Leave(object sender, EventArgs e)
        {
            ExtractManualFeedRate();
        }

        private void txtZServoAngle_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.Enter)
            {
                int angle;
                if (!int.TryParse(txtZServoAngle.Text, out angle)) return;

                _machine.MoveZServo(angle);

                e.Handled = true;
                trackZServo.Focus();
            }
        }

        private void absoluteHomeSafeZFirstToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _machine.WorkingOffset = null;      // Work with Machine coords

            double safeZRetract = 1;            //TODO: Make this a setting
            
            // Retract by an amount (or less if we're already closer to home)
            Coordinate safeZ = _machine.Location.Copy();
            if (safeZ.Z <= safeZRetract) safeZ.Z = 0;
            else safeZ.Z -=  safeZRetract;

            _machine.EnableStepperDrivers(true);
            _machine.MoveTo(safeZ, false, true, _settings.FeedRateMaximum);
            _machine.MoveTo(0, 0, 0, false, false, _machine.Settings.FeedRateMaximum);
            _machine.EnableStepperDrivers(false);

            SetMachineWorkingOffset();          // Restore any offsets
        }

        private void gotoHomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _machine.EnableStepperDrivers(true);
            _machine.MoveTo(0, 0, 0, false, false, _machine.Settings.FeedRateMaximum);
            _machine.EnableStepperDrivers(false);
        }

        private void jogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DazCamUI.UI.Jog().ShowDialog(this);
        }

        private void btnZTop_Click(object sender, EventArgs e)
        {
            _machine.MoveZServo(_machine.Settings.ZServoTopPosition);
        }

        private void btnZBottom_Click(object sender, EventArgs e)
        {
            _machine.MoveZServo(_machine.Settings.ZServoBottomPosition, true);
        }

        private void btnZLoad_Click(object sender, EventArgs e)
        {
            _machine.MoveZServo(_machine.Settings.ZServoLoadPosition);
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteGCode();
        }

        private void clearEStopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _machine.ClearEStop();
        }

        private void requestStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _machine.RequestStatus();
        }

        private void btnSendRawCommand_Click(object sender, EventArgs e)
        {
            _machine.SendCommand(txtRawCommand.Text);
        }

        private void zTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.SelectedText = GCodeFunctions.GetFunctionTemplate(GCodeFunctions.FunctionTemplates.ZTop) + "\r\n";
        }

        private void zBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.SelectedText = GCodeFunctions.GetFunctionTemplate(GCodeFunctions.FunctionTemplates.ZBottom) + "\r\n";
        }

        private void zLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.SelectedText = GCodeFunctions.GetFunctionTemplate(GCodeFunctions.FunctionTemplates.ZLoad) + "\r\n";
        }

        private void endToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.SelectedText = GCodeFunctions.GetFunctionTemplate(GCodeFunctions.FunctionTemplates.End) + "\r\n";
        }

        private void execToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.SelectedText = GCodeFunctions.GetFunctionTemplate(GCodeFunctions.FunctionTemplates.Exec) + "\r\n";
        }

        private void g4DwellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.SelectedText = "G4 P{ms Delay}\r\n";
        }

        private void g90AbsoluteModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.SelectedText = "G90 (Absolute Mode)\r\n";
        }

        private void g91IncrementalModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.SelectedText = "G91 (Incremental mode)\r\n";
        }

        private void g53OneTimeMachineCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.SelectedText = "G53 (One-time Machine Coordinates)\r\n";
        }

        private void g83DrillCycleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtGCode.SelectedText = "G83 Xnn Ynn Znn Fnn P{Dwell} Q{Depth Increment} R{Part Surface}\r\n";
        }

        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _machine.Ping();
        }

        private void copyPositionAsGCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string toCopy = string.Format("X{0} Y{1} Z{2}", txtCurrentX.Text, txtCurrentY.Text, txtCurrentZ.Text);
            Clipboard.SetText(toCopy);
        }

        private void contractGCodePaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = 470;
        }

        private void expandGCodePaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = 900;
        }

        private void offsetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DazCamUI.UI.Offsets().ShowDialog(this);
            SetMachineWorkingOffset();
        }

        private void setZeroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _machine.ZeroCurrentPosition();
        }


        private void GCodeParser_OnProgressNotify(string progressMessage, out bool cancel)
        {
            Invoke(new MethodInvoker(() =>
            {
                toolStripStatusLabel.Text = progressMessage;
            }));
            cancel = false;
        }

        private void chkToolLengthOffset_CheckedChanged(object sender, EventArgs e)
        {
            _settings.ToolLengthOffsetActive = chkToolLengthOffset.Checked;
            SetMachineWorkingOffset();
            _settings.Save();
        }

        private void cboToolOffsets_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.ActiveToolOffset = ((OffsetType)cboToolOffsets.SelectedItem).GCodeIdentifier;
            SetMachineWorkingOffset();
            _settings.Save();
        }

        private void cboWCS_SelectedIndexChanged(object sender, EventArgs e)
        {
            _settings.ActiveWorkingCoordinateSystem = ((OffsetType)cboWCS.SelectedItem).GCodeIdentifier;
            SetMachineWorkingOffset();
            _settings.Save();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DazCamUI.UI.Settings().ShowDialog(this);
            UpdateDisplayLabels();
        }

        #endregion

        #region Private Methods

        private void LogText(string message)
        {
            txtResponse.AppendText(message + "\r\n");
        }

        private void EnableOrDisableMotionFunctions()
        {
            // Disable motion functions if in EStop, Jog mode, or while running G-Code

            bool enable = _machine.EStopActive == false && _executingGCode == false;

            bool statusEnable = enable;
            bool clearEStopEnable = enable;

            if (!enable)
            {
                if (_machine.EStopActive)
                {
                    statusEnable = true;
                    clearEStopEnable = true;
                }
            }

            btnGoto.Enabled = enable;
            btnMoveDelta.Enabled = enable;
            chkEnableSteppers.Enabled = enable;
            btnZLoad.Enabled = enable;
            btnZTop.Enabled = enable;
            btnZBottom.Enabled = enable;
            txtZServoAngle.Enabled = enable;
            trackZServo.Enabled = enable;
            btnPing.Enabled = enable;
            btnStatus.Enabled = statusEnable;
            btnSendRawCommand.Enabled = enable;

            runToolStripMenuItem.Enabled = enable;
            findHomeToolStripMenuItem.Enabled = enable;
            gotoHomeToolStripMenuItem.Enabled = enable;
            iPMCalculatorToolStripMenuItem.Enabled = enable;
            offsetsToolStripMenuItem.Enabled = enable;
            pingToolStripMenuItem.Enabled = enable;
            requestStatusToolStripMenuItem.Enabled = statusEnable;
            clearEStopToolStripMenuItem.Enabled = clearEStopEnable;
            jogToolStripMenuItem.Enabled = enable;
            setZeroToolStripMenuItem.Enabled = enable;
        }

        private int GetSelectedSpeed()
        {
            if (rbSpeedManualRate.Checked)
            {
                ExtractManualFeedRate();
                return _machine.Settings.FeedRateManual;
            }

            return _machine.Settings.FeedRateMaximum;
        }

        private void UpdateCurrentPositionDisplay()
        {
            var location = _machine.GetWorkingLocation();
            txtCurrentX.Text = location.X.ToString("0.0000");
            txtCurrentY.Text = location.Y.ToString("0.0000");
            txtCurrentZ.Text = location.Z.ToString("0.0000");

            lblMachineX.Text = _machine.Location.X.ToString("0.0000");
            lblMachineY.Text = _machine.Location.Y.ToString("0.0000");
            lblMachineZ.Text = _machine.Location.Z.ToString("0.0000");

            txtZServoAngle.Text = _machine.ZServoAngle.ToString();
            trackZServo.Value = 180 - _machine.ZServoAngle;
        }

        private void GCodeSaveAsToFile()
        {
            saveFileDialog.FileName = "";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                _gCodeFilename = saveFileDialog.FileName;
                GCodeSaveToFile();
            }
        }

        private void SaveMachineSettings()
        {
            _settings.Save();
        }

        private void GCodeSaveToFile()
        {
            if (_gCodeFilename.Length == 0)
            {
                GCodeSaveAsToFile();
                return;
            }

            File.WriteAllText(_gCodeFilename, txtGCode.Text);
        }

        private void RemarkSelectedLines(bool addRemark)
        {
            if (txtGCode.SelectionLength == 0) return;

            string replace = "";

            foreach (string line in Regex.Split(txtGCode.SelectedText, "\r\n"))
            {
                if (addRemark)
                {
                    if (line.Length == 0)
                        replace += "\r\n";
                    else
                        replace += "/" + line + "\r\n";
                }
                else
                {
                    if (line.Length > 0 && line.Substring(0, 1) == "/") replace += line.Substring(1) + "\r\n";
                    else replace += line + "\r\n";
                }
            }
            txtGCode.SelectedText = replace.Remove(replace.Length - 2);
        }

        private void UpdateDisplayLabels()
        {
            lblZTop.Text = _settings.ZServoTopPosition.ToString();
            lblZBottom.Text = _settings.ZServoBottomPosition.ToString();
            rbSpeedRapid.Text = String.Format("&Rapid Speed ({0} IPM)", _settings.FeedRateMaximum.ToString());
        }

        private void ExtractManualFeedRate()
        {
            int speedIPM;
            if (!int.TryParse(txtSpeed.Text, out speedIPM)) return;

            _machine.Settings.FeedRateManual = speedIPM;
        }

        private void ExecuteGCode()
        {
            _executingGCode = true;
            EnableOrDisableMotionFunctions();

            var threadStartDelegate = new ThreadStart(StartExecutingGCodeThread);
            var GCodeExecutionThread = new Thread(threadStartDelegate);
            GCodeExecutionThread.IsBackground = true;

            GCodeExecutionThread.Start();
        }

        private void StartExecutingGCodeThread()
        {
            var timeStart = DateTime.Now;
            _machine.EnqueueCommands = true;

            try
            {
                if (_settings.IgnoreLimitsDuringExecution) _machine.IgnoreLimitSwitches(true);
                _machine.EnableStepperDrivers(true);

                var parser = new GCodeParser();
                parser.OnProgressNotify += new GCodeParser.OnProgressNotifyEventHandler(GCodeParser_OnProgressNotify);

                Invoke(new MethodInvoker(() =>
                {
                    parser.WorkspaceOffset = ((OffsetType)cboWCS.SelectedItem).GCodeIdentifier;
                    parser.ToolOffset = ((OffsetType)cboToolOffsets.SelectedItem).GCodeIdentifier;
                    parser.ToolLengthCompensation = chkToolLengthOffset.Checked;
                }));

                parser.ExecuteCode(txtGCode.Lines.ToList());

                _machine.EnableStepperDrivers(false);
                if (_settings.IgnoreLimitsDuringExecution) _machine.IgnoreLimitSwitches(false);
            }

            finally
            {
                _machine.EnqueueCommands = false;
            }

            _machine.RunQueuedCommands();
            _executingGCode = false;
            var timeElapsed = DateTime.Now.Subtract(timeStart);

            Invoke(new MethodInvoker(() =>
            {
                EnableOrDisableMotionFunctions();
                toolStripStatusLabel.Text = "";
                LogText("Program run time: " + timeElapsed.ToString());
                SetMachineWorkingOffset();          // Restore to UI defined offsets
            }));
        }

        private void PopulateOffsetSetting()
        {
            // Populate combos on first pass
            if (cboWCS.Items.Count == 0)
            {
                var offsets = OffsetType.GetAllGCodeIdentifiers(true);

                var wcsList = from o in offsets
                              where o.Category == OffsetType.Categories.WorkingCoordinateSystem
                              select o;

                cboWCS.Items.AddRange(wcsList.ToArray());

                var toolOffsetList = from o in offsets
                                     where o.Category == OffsetType.Categories.ToolHead
                                     select o;

                cboToolOffsets.Items.AddRange(toolOffsetList.ToArray());
            }

            // Select active Working Coordinate System
            foreach (var i in cboWCS.Items)
            {
                if (((OffsetType)i).GCodeIdentifier == _settings.ActiveWorkingCoordinateSystem) cboWCS.SelectedItem = i;
            }

            // Select active Tool offset
            foreach (var i in cboToolOffsets.Items)
            {
                if (((OffsetType)i).GCodeIdentifier == _settings.ActiveToolOffset) cboToolOffsets.SelectedItem = i;
            }

            chkToolLengthOffset.Checked = _settings.ToolLengthOffsetActive;

            SetMachineWorkingOffset();
        }

        private void SetMachineWorkingOffset()
        {
            var offset = new Offset();

            if (cboWCS.SelectedItem != null)
                offset += _settings.CombinedWorkspaceOffset(((OffsetType)cboWCS.SelectedItem).GCodeIdentifier);

            if (cboToolOffsets.SelectedItem != null)
                offset += _settings.CombinedWorkspaceOffset(((OffsetType)cboToolOffsets.SelectedItem).GCodeIdentifier);

            if (chkToolLengthOffset.Checked)
                offset += _settings.CombinedWorkspaceOffset(OffsetType.GCodeIdentifiers.G43);

            _machine.WorkingOffset = offset;
            UpdateCurrentPositionDisplay();
        }

        #endregion
    }
}
