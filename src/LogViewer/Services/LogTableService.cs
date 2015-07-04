// --------------------------------------------------------------------------------------------------------------------
// <copyright file="logTableService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Models;

    public class LogTableService : ILogTableService
    {
        #region Constructors
        public LogTableService()
        {
            LogTable = new LogTable();
        }
        #endregion

        #region Properties
        public LogTable LogTable { get; private set; }
        #endregion
    }
}