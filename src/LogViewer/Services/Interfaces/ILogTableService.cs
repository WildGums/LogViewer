// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogTableService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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