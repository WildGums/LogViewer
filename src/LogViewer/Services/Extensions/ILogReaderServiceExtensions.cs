// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogReaderServiceExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public static class ILogReaderServiceExtensions
    {
        public static Task<IEnumerable<LogRecord>> LoadRecordsFromFileAsync(this ILogReaderService logReaderService, FileNode fileNode)
        {
            return Task.Factory.StartNew(() => logReaderService.LoadRecordsFromFile(fileNode));
        }
    }
}