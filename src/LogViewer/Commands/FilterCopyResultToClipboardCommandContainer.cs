namespace LogViewer
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Catel.MVVM;

    using LogViewer.Services;

    using Orchestra.Services;

    public class FilterCopyResultToClipboardCommandContainer : CommandContainerBase
    {
        private readonly IClipboardService _clipboardService;
        private readonly ILogTableService _logTableService;

        public FilterCopyResultToClipboardCommandContainer(ICommandManager commandManager, IClipboardService clipboardService, ILogTableService logTableService)
            : base(Commands.Filter.CopyResultToClipboard, commandManager)
        {
            ArgumentNullException.ThrowIfNull(clipboardService);
            ArgumentNullException.ThrowIfNull(logTableService);

            _clipboardService = clipboardService;
            _logTableService = logTableService;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            var stringBuilder = new StringBuilder();

            foreach (var record in _logTableService.LogTable.Records)
            {
                stringBuilder.AppendLine(record.ToString());
            }

            _clipboardService.CopyToClipboard(stringBuilder.ToString());
        }
    }
}
