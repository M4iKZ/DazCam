
namespace DazCamUI.Controller
{
    public class Offset : Coordinate
    {
        #region Properties

        public bool InvertX { get; set; }
        public bool InvertY { get; set; }
        public bool InvertZ { get; set; }

        #endregion

        #region Constructors

        public Offset() : base() { }
        public Offset(double x, double y, double z) : base(x, y, z) { }

        #endregion

        #region Operator Overloads

        /// <summary>
        /// Use to add multiple offsets into a single offset that represents a coordinate system
        /// </summary>
        public static Offset operator +(Offset o1, Offset o2)
        {
            var addedOffset = new Offset();

            addedOffset.X = o1.X + o2.X;
            addedOffset.Y = o1.Y + o2.Y;
            addedOffset.Z = o1.Z + o2.Z;

            // Inverted coordinate systems cancel each other out (like multiplying a negative by a negative)
            addedOffset.InvertX = o1.InvertX != o2.InvertX;
            addedOffset.InvertY = o1.InvertY != o2.InvertY;
            addedOffset.InvertZ = o1.InvertZ != o2.InvertZ;

            return addedOffset;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Using this offset instance, convert a working coordinate to a machine coordinate. WARNING: If calling this method with multiple offsets where one or more
        /// offsets contain an inverted coordinate, the order of execution is important. It's better and safer to first add all offsets into a single combined
        /// offset and call this method once using that instance.
        /// </summary>
        public Coordinate ConvertToMachineCoordinate(Coordinate workingCoordinate)
        {
            var worldCoordinate = new Coordinate(workingCoordinate.X, workingCoordinate.Y, workingCoordinate.Z);

            if (this.InvertX) worldCoordinate.X = -worldCoordinate.X;
            if (this.InvertY) worldCoordinate.Y = -worldCoordinate.Y;
            if (this.InvertZ) worldCoordinate.Z = -worldCoordinate.Z;

            worldCoordinate.X += this.X;
            worldCoordinate.Y += this.Y;
            worldCoordinate.Z += this.Z;

            return worldCoordinate;
        }

        /// <summary>
        /// Using this offset instance, convert a world coordinate to a working coordinate. WARNING: If calling this method with multiple offsets where one or more
        /// offsets contain an inverted coordinate, the order of execution is important. It's better and safer to first add all offsets into a single combined
        /// offset and call this method once using that instance.
        /// </summary>
        public Coordinate ConvertCoordinateToWorking(Coordinate worldCoordinate)
        {
            var workingCoordinate = new Coordinate();

            workingCoordinate.X = worldCoordinate.X - this.X;
            workingCoordinate.Y = worldCoordinate.Y - this.Y;
            workingCoordinate.Z = worldCoordinate.Z - this.Z;

            if (this.InvertX) workingCoordinate.X = -workingCoordinate.X;
            if (this.InvertY) workingCoordinate.Y = -workingCoordinate.Y;
            if (this.InvertZ) workingCoordinate.Z = -workingCoordinate.Z;

            return workingCoordinate;
        }

        #endregion
    }
}
