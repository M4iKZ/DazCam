using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DazCamUI.Controller
{
    public class OffsetType
    {
        #region Declarations

        public enum Categories
        {
            ToolLength,                 // Used for G43 
            WorkingCoordinateSystem,           // Used for G54-G59 
            ToolHead                    // Used for T1-T9 
        }

        public enum GCodeIdentifiers
        {
            G43,
            G54, G55, G56, G57, G58, G59,
            T1, T2, T3, T4, T5, T6, T7, T8, T9
        }

        #endregion

        #region Properties

        public string Caption { get; set; }
        public Categories Category { get; set; }
        public GCodeIdentifiers GCodeIdentifier { get; set; }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Caption;
        }

        public static List<OffsetType> GetAllGCodeIdentifiers(bool shortCaptions = false)
        {
            var list = new List<OffsetType>();

            list.Add(new OffsetType() { Category = Categories.ToolLength, GCodeIdentifier = GCodeIdentifiers.G43 });
            list.Add(new OffsetType() { Category = Categories.WorkingCoordinateSystem, GCodeIdentifier = GCodeIdentifiers.G54 });
            list.Add(new OffsetType() { Category = Categories.WorkingCoordinateSystem, GCodeIdentifier = GCodeIdentifiers.G55 });
            list.Add(new OffsetType() { Category = Categories.WorkingCoordinateSystem, GCodeIdentifier = GCodeIdentifiers.G56 });
            list.Add(new OffsetType() { Category = Categories.WorkingCoordinateSystem, GCodeIdentifier = GCodeIdentifiers.G57 });
            list.Add(new OffsetType() { Category = Categories.WorkingCoordinateSystem, GCodeIdentifier = GCodeIdentifiers.G58 });
            list.Add(new OffsetType() { Category = Categories.WorkingCoordinateSystem, GCodeIdentifier = GCodeIdentifiers.G59 });
            list.Add(new OffsetType() { Category = Categories.ToolHead, GCodeIdentifier = GCodeIdentifiers.T1 });
            list.Add(new OffsetType() { Category = Categories.ToolHead, GCodeIdentifier = GCodeIdentifiers.T2 });
            list.Add(new OffsetType() { Category = Categories.ToolHead, GCodeIdentifier = GCodeIdentifiers.T3 });
            list.Add(new OffsetType() { Category = Categories.ToolHead, GCodeIdentifier = GCodeIdentifiers.T4 });
            list.Add(new OffsetType() { Category = Categories.ToolHead, GCodeIdentifier = GCodeIdentifiers.T5 });
            list.Add(new OffsetType() { Category = Categories.ToolHead, GCodeIdentifier = GCodeIdentifiers.T6 });
            list.Add(new OffsetType() { Category = Categories.ToolHead, GCodeIdentifier = GCodeIdentifiers.T7 });
            list.Add(new OffsetType() { Category = Categories.ToolHead, GCodeIdentifier = GCodeIdentifiers.T8 });
            list.Add(new OffsetType() { Category = Categories.ToolHead, GCodeIdentifier = GCodeIdentifiers.T9 });

            foreach (var item in list)
            {
                if (shortCaptions) item.SetShortCaption();
                else item.SetLongCaption();
            }

            return list;
        }

        #endregion Public Methods

        #region Private Methods

        private void SetLongCaption()
        {
            string categoryText = "";

            if (Category == Categories.ToolLength) categoryText = "Tool Length";
            if (Category == Categories.WorkingCoordinateSystem) categoryText = "WCS";
            if (Category == Categories.ToolHead) categoryText = "Tool";

            Caption = categoryText + ": " + GCodeIdentifier.ToString();
        }

        private void SetShortCaption()
        {
            Caption = GCodeIdentifier.ToString();
        }   

        #endregion Private Methods
    }
}
