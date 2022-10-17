namespace LogViewer
{
    using System;
    using Catel;
    using Catel.MVVM;
    using Services;

    public class FilterResetSearchTemplateCommandContainer : CommandContainerBase
    {
        private readonly IFilterService _filterService;
  
        public FilterResetSearchTemplateCommandContainer(ICommandManager commandManager, IFilterService filterService)
            : base(Commands.Filter.ResetSearchTemplate, commandManager)
        {
            ArgumentNullException.ThrowIfNull(filterService);

            _filterService = filterService;
        }

        protected override void Execute(object parameter)
        {
            _filterService.Filter.SearchTemplate.TemplateString = string.Empty;
        }
    }
}
