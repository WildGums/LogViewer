// --------------------------------------------------------------------------------------------------------------------
// <copyright file="logTableService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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