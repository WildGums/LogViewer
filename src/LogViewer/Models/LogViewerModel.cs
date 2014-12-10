// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System.Collections.ObjectModel;
    using Catel.Data;

    public class LogViewerModel : ModelBase
    {
        public LogViewerConfig Config { get; set; }
        public Filter Filter { get; set; }
        public ObservableCollection<Company> Companies { get; set; }
        public string StatusText { get; set; }
    }
}