namespace LogViewer
{
    using System;
    using System.Threading.Tasks;
    using Orc.WorkspaceManagement;
    using Services;

    public class FilterWorkspaceProvider : WorkspaceProviderBase
    {
        private readonly IFilterService _filterService;

        public FilterWorkspaceProvider(IFilterService filterService, IServiceProvider serviceProvider)
            : base()
        {
            ArgumentNullException.ThrowIfNull(filterService);

            _filterService = filterService;
        }
        public override async Task ProvideInformationAsync(IWorkspace workspace)
        {
            ArgumentNullException.ThrowIfNull(workspace);

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
            ArgumentNullException.ThrowIfNull(workspace);

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
