// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileNodeService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Models;

    public interface IFileNodeService
    {
        #region Methods
        FileNode LoadFileNode(string fileName);
        void ReloadFileNode(FileNode fileNode);
        #endregion
    }
}