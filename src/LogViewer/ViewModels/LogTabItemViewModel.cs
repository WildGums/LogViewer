// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogTabItemViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.ViewModels
{
    using System.Collections.Generic;
    using Catel.MVVM;
    using Models;

    public class LogTabItemViewModel : ViewModelBase
    {
        public LogTabItemViewModel(IEnumerable<LogRecord> tabItemData)
        {
            TabHeader = "Header __";
        }

        public string TabHeader { get; set; }
    }
}