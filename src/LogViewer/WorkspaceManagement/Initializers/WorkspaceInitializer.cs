// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkspaceInitializer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.IoC;
    using Orc.WorkspaceManagement;

    public class WorkspaceInitializer : IWorkspaceInitializer
    {
        #region Methods
        public async Task InitializeAsync(IWorkspace workspace)
        {
            Argument.IsNotNull(() => workspace);

            // NOTE: not injected because this is a dependency of IWorkspaceProvider
            var dependencyResolver = this.GetDependencyResolver();
            var workspaceManager = dependencyResolver.Resolve<IWorkspaceManager>();

            foreach (var provider in workspaceManager.Providers)
            {
                await provider.ProvideInformationAsync(workspace);
            }
        }
        #endregion
    }
}