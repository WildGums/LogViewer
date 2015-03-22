// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System.Collections.ObjectModel;
    using Catel.Data;

    public class Log : ModelBase
    {
        #region Constructors
        public Log()
        {
            Records = new ObservableCollection<LogRecord>();
        }
        #endregion

        #region Properties
        public ObservableCollection<LogRecord> Records { get; private set; }
        #endregion
    }
}