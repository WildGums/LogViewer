namespace LogViewer.Views
{
    using System;
    using Catel.MVVM;
    using Catel.Services;
    using Orchestra;

    /// <summary>
    /// Interaction logic for RibbonView.xaml
    /// </summary>
    public partial class RibbonView
    {
        public RibbonView(IServiceProvider serviceProvider, IViewModelWrapperService viewModelWrapperService,
            IDataContextSubscriptionService dataContextSubscriptionService, IAboutService aboutService)
            : base(serviceProvider, viewModelWrapperService, dataContextSubscriptionService)
        {
            InitializeComponent();

            ribbon.AddAboutButton(aboutService);
        }
    }
}
