// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsWindow.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Views
{
    using Catel.Windows;
    using Orc.Analytics;
    using ViewModels;

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml.
    /// </summary>
    partial class SettingsWindow : DataWindow
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindow"/> class.
        /// </summary>
        public SettingsWindow()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindow"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public SettingsWindow(SettingsViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            this.TrackViewForAnalyticsAsync();
        }
        #endregion
    }
}