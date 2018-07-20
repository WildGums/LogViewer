// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterExportResultCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System.IO;
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
            Argument.IsNotNull(() => saveFileService);
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => logTableService);

            _saveFileService = saveFileService;
            _fileService = fileService;
            _logTableService = logTableService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            _saveFileService.FileName = "LogViewerFilterExport_" + FastDateTime.Now.ToString("yy-MM-dd_HHmmss_ms") + ".log";
            if (await _saveFileService.DetermineFileAsync())
            {
                _fileService.WriteAllLines(_saveFileService.FileName, _logTableService.LogTable.Records.Select(record => record.ToString()));
            }
        }
    }
}