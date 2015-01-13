// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System.Collections.ObjectModel;

    public class Product : NavigationNode
    {
        public Product()
        {
            LogFiles = new ObservableCollection<LogFile>();
        }

        #region Properties
        public ObservableCollection<LogFile> LogFiles { get; private set; }

        public override bool AllowMultiSelection
        {
            get { return false; }
        }
        #endregion
    }
}