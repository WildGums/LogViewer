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
    using YAXLib;

    public class Product : NavigationNode
    {
        public ObservableCollection<LogFile> LogFiles { get; set; }
    }
}