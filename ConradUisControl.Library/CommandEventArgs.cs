using System;

namespace ConradUisControl.Library
{
    /// <summary>
    /// Provides event args for the case that the user has invoked via HTTP.
    /// </summary>
    public class CommandEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets/sets the command text. Example: http://localhost/[COMMAND]/.
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// Gets/sets the command text. Example: http://localhost/[COMMAND]?[PARAMETER1]&[PARAMETER2].
        /// </summary>
        public string[] Parameters { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a default instance of <see cref="CommandEventArgs"/>.
        /// </summary>
        public CommandEventArgs()
        {
            Parameters = new string[0];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string containing the name of the command and its parameters.
        /// </summary>
        /// <returns>Format: [COMMAND];[PARAMETERS,]</returns>
        public override string ToString()
        {
            string parameters = "";
            if (Parameters != null && Parameters.Length > 0)
            {
                parameters = string.Format("; {0}", string.Join(",", Parameters));
            }
            return string.Format("{0}{1}", Command, parameters);
        }

        #endregion
    }
}
