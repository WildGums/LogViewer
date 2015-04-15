// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileNodeService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface IFileNodeService
    {
        #region Methods
        FileNode CreateFileNode(string fileName);
        void LoadFileNode(FileNode fileNode);
        void ReloadFileNode(FileNode fileNode);
        #endregion
    }
}