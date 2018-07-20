// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileNodeServiceExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Threading.Tasks;
    using Models;

    public static class IFileNodeServiceExtensions
    {
        public static async Task<FileNode> CreateFileNodeAsync(this IFileNodeService fileNodeService, string fileName)
        {
            return await Task.Factory.StartNew(() => fileNodeService.CreateFileNode(fileName));
        }

        public static async Task LoadFileNodeAsync(this IFileNodeService fileNodeService, FileNode fileNode)
        {
            await Task.Factory.StartNew(() => fileNodeService.LoadFileNode(fileNode));
        }

        public static async Task ReloadFileNodeAsync(this IFileNodeService fileNodeService, FileNode fileNode)
        {
            await Task.Factory.StartNew(() => fileNodeService.ReloadFileNode(fileNode));
        }
    }
}