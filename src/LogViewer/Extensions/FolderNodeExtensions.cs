// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderNodeExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public static class FolderNodeExtensions
    {
        public static IEnumerable<FileNode> GetAllNestedFiles(this FolderNode folder)
        {
            var stack = new Stack<FolderNode>();
            stack.Push(folder);

            while (stack.Any())
            {
                folder = stack.Pop();
                foreach (var childFolder in folder.Directories)
                {
                    stack.Push(childFolder);
                }

                foreach (var file in folder.Files)
                {
                    yield return file;
                }
            }
        }
    }
}