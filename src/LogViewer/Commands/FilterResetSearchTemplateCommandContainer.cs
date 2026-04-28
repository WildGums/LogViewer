namespace LogViewer
{
    using System;
    using Catel.MVVM;
    using Services;

    public class FilterResetSearchTemplateCommandContainer : CommandContainerBase
    {
        private readonly IFilterService _filterService;
  
        public FilterResetSearchTemplateCommandContainer(ICommandManager commandManager, IFilterService filterService,
            IServiceProvider serviceProvider)
            : base(Commands.Filter.ResetSearchTemplate, commandManager, serviceProvider)
        {
            _filterService = filterService;
        }

        public override void Execute(object parameter)
        {
            _filterService.Filter.SearchTemplate.TemplateString = string.Empty;
        }
    }
}
