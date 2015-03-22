// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AggregateLogService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Models;

    public class AggregateLogService : IAggregateLogService
    {
        #region Constructors
        public AggregateLogService()
        {
            AggregateLog = new Log();
        }
        #endregion

        #region Properties
        public Log AggregateLog { get; private set; }
        #endregion
    }
}