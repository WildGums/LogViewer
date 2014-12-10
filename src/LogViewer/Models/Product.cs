// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System.Collections.ObjectModel;
    using Catel.Data;

    public class Product : ModelBase
    {
        public string Name { get; set; }

        public bool IsChecked { get; set; }

        public ObservableCollection<LogFile> LogFiles { get; set; }
    }
}