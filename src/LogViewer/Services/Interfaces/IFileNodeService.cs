// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileNodeService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Models;

    public interface IFileNodeService
    {
        #region Methods
        FileNode CreateFileNode(string fileName);
        void LoadFileNode(FileNode fileNode);
        void ReloadFileNode(FileNode fileNode);
        void ParallelLoadFileNodeBatch(params FileNode[] fileNodes);
        #endregion
    }
}