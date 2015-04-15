// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogTable.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System.Collections.ObjectModel;
    using Catel.Collections;
    using Catel.Data;

    public class LogTable : ModelBase
    {
        #region Constructors
        public LogTable()
        {
            Records = new FastObservableCollection<LogRecord>();
        }
        #endregion

        #region Properties
        public FastObservableCollection<LogRecord> Records { get; private set; }
        public bool IsTimestampVisible { get; set; }
        #endregion
    }
}