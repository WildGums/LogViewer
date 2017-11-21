// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterCopyResultToClipboardCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System.Linq;
    using System.Text;

    using Catel;
    using Catel.MVVM;

    using LogViewer.Services;

    using Orchestra.Services;

    public class FilterCopyResultToClipboardCommandContainer : CommandContainerBase
    {
        #region Fields
        private readonly IClipboardService _clipboardService;

        private readonly ILogTableService _logTableService;
        #endregion

        #region Constructors
        public FilterCopyResultToClipboardCommandContainer(ICommandManager commandManager, IClipboardService clipboardService, ILogTableService logTableService)
            : base(Commands.Filter.CopyResultToClipboard, commandManager)
        {
            Argument.IsNotNull(() => clipboardService);
            Argument.IsNotNull(() => logTableService);

            _clipboardService = clipboardService;
            _logTableService = logTableService;
        }
        #endregion

        #region Methods
        protected override void Execute(object parameter)
        {
            var stringBuilder = new StringBuilder();
            foreach (var record in _logTableService.LogTable.Records)
            {
                stringBuilder.AppendLine(record.ToString());
            }

            _clipboardService.CopyToClipboard(stringBuilder.ToString());
        }
        #endregion
    }
}