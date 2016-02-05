// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonView.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Views
{
    using Orc.Analytics;
    using Orchestra;

    /// <summary>
    /// Interaction logic for RibbonView.xaml
    /// </summary>
    public partial class RibbonView
    {
        public RibbonView()
        {
            InitializeComponent();

            this.TrackViewForAnalyticsAsync();

            ribbon.AddAboutButton();
        }
    }
}