// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileSystemService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using Models;

    public interface IFileSystemService
    {
        #region Properties
        string Filter { get; set; }
        #endregion

        #region Methods
        FolderNode LoadFileSystemContent(string path, bool isNavigationRoot = false);
        void ReleaseFileSystemContent(FolderNode folder);
        
        #endregion
    }
}