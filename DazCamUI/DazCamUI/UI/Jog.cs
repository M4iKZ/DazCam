using System;
using System.Windows.Forms;
using DazCamUI.Controller;
using System.Drawing;

namespace DazCamUI.UI
{
    public partial class Jog : Form
    {
        private Machine.JogModeTypes jogMode;
        private Machine _machine;

        public Jog()
        {
            InitializeComponent();
        }

        private void Jog_Load(object sender, EventArgs e)
        {
            _machine = Machine.GetMachine();
            jogMode = Machine.JogModeTypes.XY;
            UpdateCurrentJogMode();
        }

        private void UpdateCurrentJogMode()
        {
            Button button = btnXY;

            switch (jogMode)
            {
                case Machine.JogModeTypes.XY: button = btnXY; break;
                case Machine.JogModeTypes.XZ: button = btnXZ; break;
                case Machine.JogModeTypes.YZ: button = btnYZ; break;
            }

            btnXY.BackColor = SystemColors.Control;
            btnXZ.BackColor = SystemColors.Control;
            btnYZ.BackColor = SystemColors.Control;
            button.BackColor = Color.LightSteelBlue;

            _machine.EnableJog(jogMode);
        }

        private void btnXY_Click(object sender, EventArgs e)
        {
            jogMode = Machine.JogModeTypes.XY;
            UpdateCurrentJogMode();
            btnYZ.Focus();                   //Toggle focus between these two buttons since these 2 modes would be the most common
        }

        private void btnXZ_Click(object sender, EventArgs e)
        {
            jogMode = Machine.JogModeTypes.XZ;
            UpdateCurrentJogMode();
        }

        private void btnYZ_Click(object sender, EventArgs e)
        {
            jogMode = Machine.JogModeTypes.YZ;
            UpdateCurrentJogMode();
            btnXY.Focus();                  //Toggle focus between these two buttons since these 2 modes would be the most common
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Jog_FormClosing(object sender, FormClosingEventArgs e)
        {
            _machine.EnableJog(Machine.JogModeTypes.None);
        }

        private void Jog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
        }
    }
}
