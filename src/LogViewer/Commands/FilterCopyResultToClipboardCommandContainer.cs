// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterCopyResultToClipboardCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System.IO;
    using Catel;
    using Catel.MVVM;
    using Orchestra.Services;
    using Services;

    public class FilterCopyResultToClipboardCommandContainer : CommandContainerBase
    {
        private readonly IClipboardService _clipboardService;
        private readonly ILogTableService _logTableService;

        public FilterCopyResultToClipboardCommandContainer(ICommandManager commandManager, IClipboardService clipboardService,
            ILogTableService logTableService)
            : base(Commands.Filter.CopyResultToClipboard, commandManager)
        {
            Argument.IsNotNull(() => clipboardService);
            Argument.IsNotNull(() => logTableService);

            _clipboardService = clipboardService;
            _logTableService = logTableService;
        }

        protected override void Execute(object parameter)
        {
            StringWriter writer = new StringWriter();
            foreach (var record in _logTableService.LogTable.Records)
            {
                writer.WriteLine(record.ToString());
            }

            _clipboardService.CopyToClipboard(writer.GetStringBuilder().ToString());
        }
    }
}