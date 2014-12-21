// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System.Collections.ObjectModel;
    using Base;
    using Catel.Data;

    public class Product : NavigationNode
    {
        private readonly bool _allowMutiselection = false;

        public ObservableCollection<LogFile> LogFiles { get; set; }

        public override bool AllowMultiselection
        {
            get { return _allowMutiselection; }
        }
    }
}