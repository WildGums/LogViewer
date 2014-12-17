// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Base;
    using Catel.Data;
    using Catel.IO;
    using Services;

    public class LogViewerModel : ModelBase
    {
        public LogViewerModel()
        {
            Companies = new ObservableCollection<Company>();
        }
/*
        public LogViewerConfig Config { get; set; }
        public Filter Filter { get; set; }
*/
        public ObservableCollection<Company> Companies { get; set; }
        
        public NavigationNode SelectedItem { get; set; }
    }
}