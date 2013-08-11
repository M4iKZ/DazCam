
using System.Collections.Generic;
namespace DazCamUI.Controller
{
    public class WorkspaceOffset : Offset
    {
        #region Properties

        public bool Active { get; set; }
        public string Name { get; set; }
        public OffsetType Type { get; set; }

        #endregion

        #region Constructors

        public WorkspaceOffset() : base() { }
        public WorkspaceOffset(double x, double y, double z) : base(x, y, z) { }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
