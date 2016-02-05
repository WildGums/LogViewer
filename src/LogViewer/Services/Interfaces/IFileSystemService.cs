// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileSystemService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
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