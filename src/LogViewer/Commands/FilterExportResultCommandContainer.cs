﻿namespace LogViewer
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Orc.FileSystem;
    using Services;

    public class FilterExportResultCommandContainer : CommandContainerBase
    {
        private readonly ISaveFileService _saveFileService;
        private readonly IFileService _fileService;
        private readonly ILogTableService _logTableService;

        public FilterExportResultCommandContainer(ICommandManager commandManager,
            ISaveFileService saveFileService,
            IFileService fileService,
            ILogTableService logTableService)
            : base(Commands.Filter.ExportResult, commandManager)
        {
            ArgumentNullException.ThrowIfNull(saveFileService);
            ArgumentNullException.ThrowIfNull(fileService);
            ArgumentNullException.ThrowIfNull(logTableService);

            _saveFileService = saveFileService;
            _fileService = fileService;
            _logTableService = logTableService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            var result = await _saveFileService.DetermineFileAsync(new DetermineSaveFileContext
            {
                FileName = "LogViewerFilterExport_" + FastDateTime.Now.ToString("yy-MM-dd_HHmmss_ms") + ".log"
            });

            if (result.Result)
            {
                _fileService.WriteAllLines(result.FileName, _logTableService.LogTable.Records.Select(record => record.ToString()));
            }
        }
    }
}
