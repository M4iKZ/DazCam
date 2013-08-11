using System;
using System.Windows.Forms;
using DazCamUI.Controller;

namespace DazCamUI.UI
{
    public partial class IPMCalculator : Form
    {
        #region Constructors

        public IPMCalculator()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void IPMCalculator_Load(object sender, EventArgs e)
        {
            txtInchesX.Text = "0";
            txtInchesY.Text = "0";
            txtInchesZ.Text = "0";

            txtResolution.Text = "1";
            txtStepDelay.Text = "10";
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            var machine = Machine.GetMachine();

            int inchesX;
            int inchesY;
            int inchesZ;

            if (!int.TryParse(txtInchesX.Text, out inchesX))
            {
                MessageBox.Show("Inches must be an even integer");
                return;
            }

            if (!int.TryParse(txtInchesY.Text, out inchesY))
            {
                MessageBox.Show("Inches must be an even integer");
                return;
            }
            
            if (!int.TryParse(txtInchesZ.Text, out inchesZ))
            {
                MessageBox.Show("Inches must be an even integer");
                return;
            }

            int resolution;
            if (!int.TryParse(txtResolution.Text, out resolution)) resolution = 1;

            int stepDelay;
            if (!int.TryParse(txtStepDelay.Text, out stepDelay)) stepDelay = 10;

            int feedRate = resolution * 1000;
            feedRate += stepDelay;
            feedRate *= -1;

            machine.EnableStepperDrivers(true);

            var timeBegin = DateTime.Now;

            //TODO: this won't work as-is once we're asynch - will need event instead
            machine.MoveTo(inchesX, inchesY, inchesZ, true, true, feedRate);

            var timeElapsed = DateTime.Now.Subtract(timeBegin);

            machine.EnableStepperDrivers(false);

            double inchesTraveled = inchesX + inchesY + inchesZ;        //TODO: Totally wrong, need to calculate hypotenous

            var inchesPerMinute = (60000 / timeElapsed.TotalMilliseconds) * inchesTraveled;
            this.Text = inchesPerMinute.ToString() + " IPM";

            // Toggle direction
            txtInchesX.Text = (-1 * inchesX).ToString();
            txtInchesY.Text = (-1 * inchesY).ToString();
            txtInchesZ.Text = (-1 * inchesZ).ToString();
        }

        private void txtInchesX_Enter(object sender, EventArgs e)
        {
            txtInchesX.SelectAll();
        }

        private void txtInchesY_Enter(object sender, EventArgs e)
        {
            txtInchesY.SelectAll();
        }

        private void txtInchesZ_Enter(object sender, EventArgs e)
        {
            txtInchesZ.SelectAll();
        }

        private void txtResolution_Enter(object sender, EventArgs e)
        {
            txtResolution.SelectAll();
        }

        private void txtStepDelay_Enter(object sender, EventArgs e)
        {
            txtStepDelay.SelectAll();
        }

        #endregion
    }
}
