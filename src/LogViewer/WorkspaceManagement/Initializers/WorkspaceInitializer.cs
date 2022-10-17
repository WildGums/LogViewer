namespace LogViewer
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Orc.WorkspaceManagement;

    public class WorkspaceInitializer : IWorkspaceInitializer
    {
        public async Task InitializeAsync(IWorkspace workspace)
        {
            ArgumentNullException.ThrowIfNull(workspace);

            // NOTE: not injected because this is a dependency of IWorkspaceProvider
            var dependencyResolver = this.GetDependencyResolver();
            var workspaceManager = dependencyResolver.Resolve<IWorkspaceManager>();

            foreach (var provider in workspaceManager.Providers)
            {
                await provider.ProvideInformationAsync(workspace);
            }
        }
    }
}
