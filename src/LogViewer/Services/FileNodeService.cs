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
    using Microsoft.Extensions.Logging;
    using Models;

    public class FileNodeService : IFileNodeService
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(FileNodeService));

        private static readonly Regex FileNameMask = new Regex(@"^[a-zA-Z\.]+_(\d{4}-\d{2}-\d{2})_\d{6}_\d+\.log$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

        private readonly IDispatcherService _dispatcherService;
        private readonly IFilterService _filterService;
        private readonly object _lockObject = new object();
        private readonly ILogReaderService _logReaderService;
#pragma warning disable IDISP006 // Implement IDisposable
        private CancellationTokenSource _cancellationTokenSource;
#pragma warning restore IDISP006 // Implement IDisposable
        private Task _loadingFileNodeBatch;

        public FileNodeService(ILogReaderService logReaderService, IDispatcherService dispatcherService, 
            IFilterService filterService)
        {
            ArgumentNullException.ThrowIfNull(logReaderService);
            ArgumentNullException.ThrowIfNull(dispatcherService);
            ArgumentNullException.ThrowIfNull(filterService);

            _logReaderService = logReaderService;
            _dispatcherService = dispatcherService;
            _filterService = filterService;
        }

        public FileNode CreateFileNode(string fileName)
        {
            Argument.IsNotNullOrEmpty(() => fileName);

            //Logger.LogDebug("Creating file node '{0}'", fileName);

            var fileNode = new FileNode(new FileInfo(fileName));

            fileNode.Name = fileNode.FileInfo.Name;
            fileNode.IsUnifyNamed = FileNameMask.IsMatch(fileNode.FileInfo.Name);
            if (!fileNode.IsUnifyNamed)
            {
                fileNode.Name = fileNode.FileInfo.Name;
            }
            else
            {
                fileNode.Name = fileNode.FileInfo.Name;
                var dateTimeString = Regex.Match(fileNode.FileInfo.Name, @"(\d{4}-\d{2}-\d{2})", RegexOptions.None, TimeSpan.FromSeconds(1)).Value;
                fileNode.DateTime = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd", null, DateTimeStyles.None);
            }

            return fileNode;
        }

        [Time]
        public void LoadFileNode(FileNode fileNode)
        {
            Logger.LogDebug("Loading file node '{0}'", fileNode);

            try
            {
                _dispatcherService.Invoke(() =>
                {
                    var fileRecords = _logReaderService.LoadRecordsFromFileAsync(fileNode).Result;

                    var logRecords = fileNode.Records;
                    //using (logRecords.SuspendChangeNotifications())
                    {
                        ((ICollection<LogRecord>)logRecords).ReplaceRange(fileRecords);
                    }
                }, true);
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to load file node '{0}'", fileNode);
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
            if (fileNodes is null || !fileNodes.Any())
            {
                return;
            }

            var tasks = fileNodes.Select(node => (Action) (() => LoadFileNode(node))).ToArray();

            if (_loadingFileNodeBatch is not null)
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
#pragma warning disable IDISP003 // Dispose previous before re-assigning
                _cancellationTokenSource = new CancellationTokenSource();
#pragma warning restore IDISP003 // Dispose previous before re-assigning
                var parallelOptions = new ParallelOptions() {CancellationToken = _cancellationTokenSource.Token};
                Parallel.Invoke(parallelOptions, tasks);
            }
        }

        private void EndParallelFileNodesLoading()
        {
            if (_cancellationTokenSource is not null)
            {
                _cancellationTokenSource.Cancel();
                if (!_loadingFileNodeBatch.Wait(100))
                {
                    Logger.LogWarning("Loading FileNode batch was not awaited.");
                }
            }

#pragma warning disable IDISP003 // Dispose previous before re-assigning
            _cancellationTokenSource = null;
#pragma warning restore IDISP003 // Dispose previous before re-assigning
        }

        private void ResumeParallelFileNodesLoading()
        {
            _filterService.ApplyLogRecordsFilter();

            _loadingFileNodeBatch = null;
        }
    }
}
