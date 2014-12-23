// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System.Collections.ObjectModel;

    using LogViewer.Models.Base;

    public class Product : NavigationNode
    {
        #region Fields
        private readonly bool _allowMutiselection = false;
        #endregion

        #region Properties
        public ObservableCollection<LogFile> LogFiles { get; set; }

        public override bool AllowMultiselection
        {
            get
            {
                return _allowMutiselection;
            }
        }
        #endregion
    }
}