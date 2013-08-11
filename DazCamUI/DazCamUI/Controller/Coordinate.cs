
namespace DazCamUI.Controller
{
    public class Coordinate
    {
        #region Properties

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        #endregion

        #region Constructors

        public Coordinate() { }

        public Coordinate(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("<{0}, {1}, {2}>", X, Y, Z);
        }

        /// <summary>
        /// Returns a new Coordinate instance with a copy of the coordinate values
        /// </summary>
        public virtual Coordinate Copy()
        {
            return new Coordinate(X, Y, Z);
        }
    }
}