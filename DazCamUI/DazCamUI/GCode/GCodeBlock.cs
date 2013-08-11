using System;
using System.Collections.Generic;

namespace DazCamUI.GCode
{
    public class GCodeBlock
    {
        #region Declarations

        #endregion

        #region Properties

        public double? G { get; set; }
        public double? M { get; set; }
        public double? X { get; set; }
        public double? Y { get; set; }
        public double? Z { get; set; }
        public double? I { get; set; }
        public double? J { get; set; }
        public double? F { get; set; }
        public double? T { get; set; }
        public double? P { get; set; }
        public double? Q { get; set; }
        public double? R { get; set; }
        public double? S { get; set; }

        public List<string> ModifierCodes { get; private set; }

        #endregion

        #region Constructor

        public GCodeBlock()
        {
            ModifierCodes = new List<string>();
        }

        #endregion

        #region Static Methods

        public static List<GCodeBlock> Parse(string blockLine)
        {
            var words = blockLine.ToUpper().Split(' ');

            var newBlocks = new List<GCodeBlock>();
            var newBlock = new GCodeBlock();
            newBlocks.Add(newBlock);

            foreach (string word in words)
            {
                if (word.Length < 2) continue;

                double value;
                string wordType = word.Substring(0, 1);
                if (!double.TryParse(word.Substring(1), out value)) value = 0;

                // Another G word on the same line as a previous G word, starts a new block
                if (wordType == "G" && newBlock.G.HasValue)
                {
                    newBlock = new GCodeBlock();
                    newBlocks.Add(newBlock);
                }

                // Another M word on the same line as a previous M word, starts a new block
                if (wordType == "M" && newBlock.M.HasValue)
                {
                    newBlock = new GCodeBlock();
                    newBlocks.Add(newBlock);
                }

                switch (wordType)
                {
                    case "G": newBlock.G = value; break;
                    case "M": newBlock.M = value; break;
                    case "X": newBlock.X = value; break;
                    case "Y": newBlock.Y = value; break;
                    case "Z": newBlock.Z = value; break;
                    case "I": newBlock.I = value; break;
                    case "J": newBlock.J = value; break;
                    case "F": newBlock.F = value; break;
                    case "T": newBlock.T = value; break;
                    case "P": newBlock.P = value; break;
                    case "Q": newBlock.Q = value; break;
                    case "R": newBlock.R = value; break;
                    case "S": newBlock.S = value; break;
                }
            }

            return newBlocks;
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        #endregion
    }
}
