// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkspaceInitializer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using Catel;
    using Catel.IoC;
    using Orc.WorkspaceManagement;

    public class WorkspaceInitializer : IWorkspaceInitializer
    {
        #region Methods
        public void Initialize(IWorkspace workspace)
        {
            Argument.IsNotNull(() => workspace);

            // NOTE: not injected because this is a dependency of IWorkspaceProvider
            var dependencyResolver = this.GetDependencyResolver();
            var workspaceManager = dependencyResolver.Resolve<IWorkspaceManager>();

            foreach (var provider in workspaceManager.Providers)
            {
                provider.ProvideInformation(workspace);
            }
        }
        #endregion
    }
}