// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilterWorkspaceProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Orc.WorkspaceManagement;
    using Services;

    public class FilterWorkspaceProvider : WorkspaceProviderBase
    {
        #region Fields
        private readonly IFilterService _filterService;
        #endregion

        #region Constructors
        public FilterWorkspaceProvider(IWorkspaceManager workspaceManager, IFilterService filterService, IServiceLocator serviceLocator)
            : base(workspaceManager, serviceLocator)
        {
            Argument.IsNotNull(() => filterService);

            _filterService = filterService;
        }
        #endregion

        public override async Task ProvideInformationAsync(IWorkspace workspace)
        {
            Argument.IsNotNull(() => workspace);

            workspace.SetWorkspaceValue(Settings.Workspace.Filter.ShowDebug, _filterService.Filter.ShowDebug);
            workspace.SetWorkspaceValue(Settings.Workspace.Filter.ShowError, _filterService.Filter.ShowError);
            workspace.SetWorkspaceValue(Settings.Workspace.Filter.ShowInfo, _filterService.Filter.ShowInfo);
            workspace.SetWorkspaceValue(Settings.Workspace.Filter.ShowWarning, _filterService.Filter.ShowWarning);

            workspace.SetWorkspaceValue(Settings.Workspace.Filter.IsUseDateRange, _filterService.Filter.IsUseDateRange);
            workspace.SetWorkspaceValue(Settings.Workspace.Filter.StartDate, _filterService.Filter.StartDate);
            workspace.SetWorkspaceValue(Settings.Workspace.Filter.EndDate, _filterService.Filter.EndDate);
        }

        public override async Task ApplyWorkspaceAsync(IWorkspace workspace)
        {
            Argument.IsNotNull(() => workspace);

            _filterService.Filter.ShowDebug = workspace.GetWorkspaceValue(Settings.Workspace.Filter.ShowDebug, Settings.Workspace.Filter.ShowDebugDefaultValue);
            _filterService.Filter.ShowError = workspace.GetWorkspaceValue(Settings.Workspace.Filter.ShowError, Settings.Workspace.Filter.ShowErrorDefaultValue);
            _filterService.Filter.ShowInfo = workspace.GetWorkspaceValue(Settings.Workspace.Filter.ShowInfo, Settings.Workspace.Filter.ShowInfoDefaultValue);
            _filterService.Filter.ShowWarning = workspace.GetWorkspaceValue(Settings.Workspace.Filter.ShowWarning, Settings.Workspace.Filter.ShowWarningDefaultValue);

            _filterService.Filter.IsUseDateRange = workspace.GetWorkspaceValue(Settings.Workspace.Filter.IsUseDateRange, Settings.Workspace.Filter.IsUseDateRangeDefaultValue);
            _filterService.Filter.StartDate = workspace.GetWorkspaceValue(Settings.Workspace.Filter.StartDate, DateTime.Today);
            _filterService.Filter.EndDate = workspace.GetWorkspaceValue(Settings.Workspace.Filter.EndDate, DateTime.Today);
        }
    }
}