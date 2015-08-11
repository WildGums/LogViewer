// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterResetSearchTemplateCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using Catel;
    using Catel.MVVM;
    using Services;

    public class FilterResetSearchTemplateCommandContainer : CommandContainerBase
    {
        private readonly IFilterService _filterService;
  
        public FilterResetSearchTemplateCommandContainer(ICommandManager commandManager, IFilterService filterService)
            : base(Commands.Filter.ResetSearchTemplate, commandManager)
        {
            Argument.IsNotNull(() => filterService);

            _filterService = filterService;
        }

        protected override void Execute(object parameter)
        {
            _filterService.Filter.SearchTemplate.TemplateString = string.Empty;
        }
    }
}