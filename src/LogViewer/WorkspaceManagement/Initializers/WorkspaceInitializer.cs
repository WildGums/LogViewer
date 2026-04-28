namespace LogViewer
{
    using System;
    using System.Threading.Tasks;
    using Orc.WorkspaceManagement;

    public class WorkspaceInitializer : IWorkspaceInitializer
    {
        //private readonly IWorkspaceManager _workspaceManager;

        public WorkspaceInitializer()
        {
        }

        public async Task InitializeAsync(IWorkspace workspace)
        {
            ArgumentNullException.ThrowIfNull(workspace);

            //foreach (var provider in _workspaceManager.Providers)
            //{
            //    await provider.ProvideInformationAsync(workspace);
            //}
        }
    }
}
