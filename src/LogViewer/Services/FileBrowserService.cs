// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileBrowserService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Models;

    public class FileBrowserService : IFileBrowserService
    {
        #region Constructors
        public FileBrowserService()
        {
            FileBrowserModel = new FileBrowserModel();
        }
        #endregion

        #region Properties
        public FileBrowserModel FileBrowserModel { get; private set; }
        #endregion
    }
}