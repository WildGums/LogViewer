// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileBrowserService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Models;

    public interface IFileBrowserService
    {
        #region Properties
        FileBrowserModel FileBrowserModel { get; }
        #endregion
    }
}