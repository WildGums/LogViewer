// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileNodeService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
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
        private readonly IDispatcherService _dispatcherService;
        private readonly IFilterService _filterService;
        private readonly object _lockObject = new object();
        private readonly ILogReaderService _logReaderService;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _loadingFileNodeBatch;
        #endregion

        #region Constructors
        public FileNodeService(ILogReaderService logReaderService, IDispatcherService dispatcherService, IFilterService filterService)
        {
            Argument.IsNotNull(() => logReaderService);
            Argument.IsNotNull(() => dispatcherService);
            Argument.IsNotNull(() => filterService);

            _logReaderService = logReaderService;
            _dispatcherService = dispatcherService;
            _filterService = filterService;
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
                _dispatcherService.Invoke(() =>
                {
                    var fileRecords = _logReaderService.LoadRecordsFromFileAsync(fileNode).Result;

                    var logRecords = fileNode.Records;
                    using (logRecords.SuspendChangeNotifications())
                    {
                        ((ICollection<LogRecord>)logRecords).ReplaceRange(fileRecords);
                    }
                }, true);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to load file node '{0}'", fileNode);
            }
        }

        public void ReloadFileNode(FileNode fileNode)
        {
            lock (_lockObject)
            {
                LoadFileNode(fileNode);
            }

            _filterService.ApplyFilesFilter();
        }

        public void ParallelLoadFileNodeBatch(params FileNode[] fileNodes)
        {
            if (fileNodes == null || !fileNodes.Any())
            {
                return;
            }

            var tasks = fileNodes.Select(node => (Action) (() => LoadFileNode(node))).ToArray();

            if (_loadingFileNodeBatch != null)
            {
                EndParallelFileNodesLoading();
            }

            _loadingFileNodeBatch = Task.Factory.StartNew(() => BeginParallelFileNodesLoading(tasks))
                .ContinueWith(task => ResumeParallelFileNodesLoading());
        }

        private void BeginParallelFileNodesLoading(Action[] tasks)
        {
            Argument.IsNotNullOrEmptyArray(() => tasks);

            lock (_lockObject)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                var parallelOptions = new ParallelOptions() {CancellationToken = _cancellationTokenSource.Token};
                Parallel.Invoke(parallelOptions, tasks);
            }
        }

        private void EndParallelFileNodesLoading()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                if (!_loadingFileNodeBatch.Wait(100))
                {
                    Log.Warning("Loading FileNode batch was not awaited.");
                }
            }

            _cancellationTokenSource = null;
        }

        private void ResumeParallelFileNodesLoading()
        {
            _filterService.ApplyLogRecordsFilter();

            _loadingFileNodeBatch = null;
        }
        #endregion
    }
}