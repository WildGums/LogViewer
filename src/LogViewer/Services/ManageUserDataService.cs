// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManageUserDataService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.Services;
    using LogViewer.Services;
    using Orc.FileSystem;
    using Orc.FilterBuilder.Services;
    using Orc.WorkspaceManagement;
    using Orchestra.Services;
    using OrcFilterService = Orc.FilterBuilder.Services.IFilterService;

    public class ManageUserDataService : AppDataService, IManageUserDataService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        private readonly IFilterSchemeManager _filterSchemeManager;
        private readonly OrcFilterService _filterService;
        private readonly IMessageService _messageService;
        private readonly IWorkspaceManager _workspaceManager;

        public ManageUserDataService(IMessageService messageService, OrcFilterService filterService, IFilterSchemeManager filterSchemeManager, 
            IWorkspaceManager workspaceManager, ISaveFileService saveFileService, IProcessService processService, IDirectoryService directoryService,
            IFileService fileService)
            : base(saveFileService, processService, directoryService, fileService)
        {
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => filterService);
            Argument.IsNotNull(() => filterSchemeManager);
            Argument.IsNotNull(() => workspaceManager);

            _messageService = messageService;
            _filterService = filterService;
            _filterSchemeManager = filterSchemeManager;
            _workspaceManager = workspaceManager;
        }

        public async Task<bool> ResetFiltersAsync()
        {
            if (await _messageService.ShowAsync("Resetting filters will delete all your current filters. This action cannot be undone. Are you sure you want to reset the filters?", string.Empty, MessageButton.YesNo) == MessageResult.No)
            {
                return false;
            }

            Log.Info("Resetting filters");

            _filterService.SelectedFilter = null;

            _filterSchemeManager.FilterSchemes.Schemes.Clear();
            await _filterSchemeManager.SaveAsync();

            return true;
        }

        public async Task<bool> ResetWorkspacesAsync()
        {
            if (await _messageService.ShowAsync("Resetting workspaces will delete all your current workspaces. This action cannot be undone. Are you sure you want to reset the workspaces?", string.Empty, MessageButton.YesNo) == MessageResult.No)
            {
                return false;
            }

            Log.Info("Resetting workspaces");

            var workspaces = _workspaceManager.Workspaces;
            foreach (var workspace in workspaces)
            {
                if (workspace.CanDelete)
                {
                    await _workspaceManager.RemoveAsync(workspace);
                }
            }

            var newWorkspace = _workspaceManager.Workspaces.FirstOrDefault();
            if (newWorkspace != null)
            {
                await _workspaceManager.SetWorkspaceAsync(newWorkspace);
            }

            return true;
        }
    }
}
