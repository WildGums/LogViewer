// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonView.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
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

            this.TrackViewForAnalytics();

            ribbon.AddAboutButton();
        }
    }
}