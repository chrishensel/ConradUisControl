using System;

namespace ConradUisControl.Library
{
    /// <summary>
    /// Holds context information for the CUC-library.
    /// </summary>
    public static class CucGlobal
    {
        #region Fields

        /// <summary>
        /// Gets/sets the global configuration for the ConradUisControl.
        /// </summary>
        public static ICucConfiguration Configuration { get; set; }

        #endregion
    }
}
