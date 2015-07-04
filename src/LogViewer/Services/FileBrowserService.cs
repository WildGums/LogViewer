// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileBrowserService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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