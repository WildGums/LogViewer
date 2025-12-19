using System.Runtime.CompilerServices;
using Catel.IoC;
using LogViewer.Services;
using Orchestra.Services;
using Velopack;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    [ModuleInitializer]
    public static void Initialize()
    {
        VelopackApp.Build().Run();

        var serviceLocator = ServiceLocator.Default;
        
        serviceLocator.RegisterType<IRibbonService, RibbonService>();
        serviceLocator.RegisterType<IApplicationInitializationService, ApplicationInitializationService>();

        // ***** IMPORTANT NOTE *****
        //
        // Only register the shell services in the ModuleInitializer. All other types must be registered 
        // in the ApplicationInitializationService
    }
}
