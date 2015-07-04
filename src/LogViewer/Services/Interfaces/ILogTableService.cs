// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogTableService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Models;

    public interface ILogTableService
    {
        #region Properties
        LogTable LogTable { get; }
        #endregion
    }
}