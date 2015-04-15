// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileNodeService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.Logging;
    using Catel.Services;
    using MethodTimer;
    using Models;

    public class FileNodeService : IFileNodeService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly Regex _fileNameMask = new Regex(@"^[a-zA-Z\.]+_(\d{4}-\d{2}-\d{2})_\d{6}_\d+\.log$", RegexOptions.Compiled);
        private readonly IIndexSearchService _indexSearchService;
        private readonly IDispatcherService _dispatcherService;
        private readonly ILogReaderService _logReaderService;
        #endregion

        #region Constructors
        public FileNodeService(ILogReaderService logReaderService, IIndexSearchService indexSearchService, IDispatcherService dispatcherService)
        {
            Argument.IsNotNull(() => logReaderService);
            Argument.IsNotNull(() => indexSearchService);
            Argument.IsNotNull(() => dispatcherService);

            _logReaderService = logReaderService;
            _indexSearchService = indexSearchService;
            _dispatcherService = dispatcherService;
        }
        #endregion

        #region Methods
        public FileNode CreateFileNode(string fileName)
        {
            Argument.IsNotNullOrEmpty(() => fileName);

            //Log.Debug("Creating file node '{0}'", fileName);

            var fileNode = new FileNode(new FileInfo(fileName));

            fileNode.Name = fileNode.FileInfo.Name;
            fileNode.IsUnifyNamed = _fileNameMask.IsMatch(fileNode.FileInfo.Name);
            if (!fileNode.IsUnifyNamed)
            {
                fileNode.Name = fileNode.FileInfo.Name;
            }
            else
            {
                fileNode.Name = fileNode.FileInfo.Name;
                var dateTimeString = Regex.Match(fileNode.FileInfo.Name, @"(\d{4}-\d{2}-\d{2})").Value;
                fileNode.DateTime = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd", null, DateTimeStyles.None);
            }

            return fileNode;
        }

        [Time]
        public void LoadFileNode(FileNode fileNode)
        {
            Log.Debug("Loading file node '{0}'", fileNode);

            try
            {              
                var fileRecords = _logReaderService.LoadRecordsFromFileAsync(fileNode).Result;

               _dispatcherService.Invoke(() =>
               {
                   var logRecords = fileNode.Records;
                   using (logRecords.SuspendChangeNotifications())
                   {
                       logRecords.ReplaceRange(fileRecords);
                   }
               });                

                _indexSearchService.EnsureFullTextIndexAsync(fileNode).Wait();
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to load file node '{0}'", fileNode);
            }
        }

        public void ReloadFileNode(FileNode fileNode)
        {
            LoadFileNode(fileNode);
        }
        #endregion
    }
}