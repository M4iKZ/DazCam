namespace DazCamUI.Controller
{
    public class AxisSettings
    {
        #region Declarations

        #endregion

        #region Properties

        public bool InvertDirection { get; set; }
        public int StepsPerTurn { get; set; }
        public int TurnsPerInch { get; set; }

        #endregion

        #region Constructors
        #endregion

        #region Public Methods

        public int FullStepsPerInch()

        {
            return StepsPerTurn * TurnsPerInch;
        }

        #endregion

        #region Private Methods
        #endregion
    }
}
