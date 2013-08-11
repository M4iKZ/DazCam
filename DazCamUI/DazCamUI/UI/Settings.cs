using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DazCamUI.Controller;

namespace DazCamUI.UI
{
    public partial class Settings : Form
    {
        #region Declarations

        private ControllerSettings _settings;

        #endregion

        #region Constructor

        public Settings()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void Settings_Load(object sender, EventArgs e)
        {
            _settings = Machine.GetMachine().Settings;
            PopulateSettings();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CommitSettings();
        }

        private void btnExecHeaderFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK) return;
            txtExecHeaderFile.Text = openFileDialog.FileName;
        }

        private void btnExecFooterFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog(this) != System.Windows.Forms.DialogResult.OK) return;
            txtExecFooterFile.Text = openFileDialog.FileName;
        }

        #endregion

        #region Private Methods

        private void PopulateSettings()
        {
            // --- Axis Settings

            txtStepsPerTurnX.Text = _settings.XAxis.StepsPerTurn.ToString();
            txtTurnsPerInchX.Text = _settings.XAxis.TurnsPerInch.ToString();
            chkInvertX.Checked = _settings.XAxis.InvertDirection;

            txtStepsPerTurnY.Text = _settings.YAxis.StepsPerTurn.ToString();
            txtTurnsPerInchY.Text = _settings.YAxis.TurnsPerInch.ToString();
            chkInvertY.Checked = _settings.YAxis.InvertDirection;

            txtStepsPerTurnZ.Text = _settings.ZAxis.StepsPerTurn.ToString();
            txtTurnsPerInchZ.Text = _settings.ZAxis.TurnsPerInch.ToString();
            chkInvertZ.Checked = _settings.ZAxis.InvertDirection;

            // --- Standard Settings

            txtIPAddress.Text = _settings.MachineIPAddress;
            txtMaximumFeedRate.Text = _settings.FeedRateMaximum.ToString();
            txtDefaultCuttingFeedrate.Text = _settings.FeedRateCuttingDefault.ToString();
            txtArcLineSegmentLength.Text = _settings.ArcLineSegmentLength.ToString();
            txtExecSearchPath.Text = _settings.GCodeExecSearchPath;
            txtExecHeaderFile.Text = _settings.ExecHeaderFile;
            txtExecFooterFile.Text = _settings.ExecFooterFile;
            chkIgnoreLimits.Checked = _settings.IgnoreLimitsDuringExecution;

            if (_settings.HomeCorner == ControllerSettings.BedCorner.TopLeft) rbOriginUL.Checked = true;
            if (_settings.HomeCorner == ControllerSettings.BedCorner.TopRight) rbOriginUR.Checked = true;
            if (_settings.HomeCorner == ControllerSettings.BedCorner.BottomLeft) rbOriginLL.Checked = true;
            if (_settings.HomeCorner == ControllerSettings.BedCorner.BottomRight) rbOriginLR.Checked = true;
        }

        private void CommitSettings()
        {
            // --- Axis Settings

            _settings.XAxis.StepsPerTurn = SafeInt(txtStepsPerTurnX.Text);
            _settings.XAxis.TurnsPerInch = SafeInt(txtTurnsPerInchX.Text);
            _settings.XAxis.InvertDirection = chkInvertX.Checked;

            _settings.YAxis.StepsPerTurn = SafeInt(txtStepsPerTurnY.Text);
            _settings.YAxis.TurnsPerInch = SafeInt(txtTurnsPerInchY.Text);
            _settings.YAxis.InvertDirection = chkInvertY.Checked;

            _settings.ZAxis.StepsPerTurn = SafeInt(txtStepsPerTurnZ.Text);
            _settings.ZAxis.TurnsPerInch = SafeInt(txtTurnsPerInchZ.Text);
            _settings.ZAxis.InvertDirection = chkInvertZ.Checked;

            _settings.MachineIPAddress = txtIPAddress.Text.Trim();
            _settings.FeedRateMaximum = SafeInt(txtMaximumFeedRate.Text.Trim());
            _settings.FeedRateCuttingDefault = SafeInt(txtDefaultCuttingFeedrate.Text.Trim());
            _settings.ArcLineSegmentLength = SafeDouble(txtArcLineSegmentLength.Text.Trim());
            _settings.GCodeExecSearchPath = txtExecSearchPath.Text.Trim();
            _settings.ExecHeaderFile = txtExecHeaderFile.Text.Trim();
            _settings.ExecFooterFile = txtExecFooterFile.Text.Trim();
            _settings.IgnoreLimitsDuringExecution = chkIgnoreLimits.Checked;

            if (rbOriginUL.Checked) _settings.HomeCorner = ControllerSettings.BedCorner.TopLeft;
            if (rbOriginUR.Checked) _settings.HomeCorner = ControllerSettings.BedCorner.TopRight;
            if (rbOriginLL.Checked) _settings.HomeCorner = ControllerSettings.BedCorner.BottomLeft;
            if (rbOriginLR.Checked) _settings.HomeCorner = ControllerSettings.BedCorner.BottomRight;

            _settings.Save();

            //TODO: Only do this if related settings have changed - add a dirty flag
            Machine.GetMachine().InitializeHardware();
        }

        private int SafeInt(string value)
        {
            int intValue;
            if (!int.TryParse(value, out intValue)) intValue = 0;
            return intValue;
        }

        private double SafeDouble(string value)
        {
            double doubleValue;
            if (!double.TryParse(value, out doubleValue)) doubleValue = 0;
            return doubleValue;
        }

        #endregion
    }
}
