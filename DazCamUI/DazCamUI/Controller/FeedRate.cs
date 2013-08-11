namespace DazCamUI.Controller
{
    public class FeedRate
    {
        #region Declarations

        public enum StepResolutions
        {
            Full = 1,
            Half = 2,
            Quarter = 4,
            Eighth = 8
        }

        #endregion

        #region Properties

        public StepResolutions Resolution { get; set; }
        public int StepRatePercentage { get; set; }

        #endregion

        #region Constructors

        public FeedRate()
        {
            Resolution = StepResolutions.Half;
            StepRatePercentage = 50;
        }

        #endregion

        #region Public Methods

        public int GetResolutionUnits()
        {
            switch (Resolution)
            {
                case StepResolutions.Full: return 1;
                case StepResolutions.Half: return 2;
                case StepResolutions.Quarter: return 4;
                case StepResolutions.Eighth: return 8;
            }
            return 1;
        }

        public void SetResolutionUnits(int resolution)
        {
            switch (resolution)
            {
                case 1: Resolution = StepResolutions.Full; break;
                case 2: Resolution = StepResolutions.Half; break;
                case 4: Resolution = StepResolutions.Quarter; break;
                case 8: Resolution = StepResolutions.Eighth; break;
                default: Resolution = StepResolutions.Full; break;
            }
        }

        #endregion
    }
}
