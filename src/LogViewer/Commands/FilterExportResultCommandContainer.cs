// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterExportResultCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System.IO;
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

        protected override void Execute(object parameter)
        {
            _saveFileService.FileName = "LogViewerFilterExport_" + FastDateTime.Now.ToString("yy-MM-dd_HHmmss_ms") + ".log";
            var determineFileAsync = _saveFileService.DetermineFileAsync();
            determineFileAsync.Wait();

            if (determineFileAsync.Result)
            {
                using (var writer = new StreamWriter(_fileService.Open(_saveFileService.FileName, FileMode.Create, FileAccess.Write)))
                {
                    foreach (var record in _logTableService.LogTable.Records)
                    {
                        writer.WriteLine(record.ToString());
                    }
                }
            }
        }
    }
}