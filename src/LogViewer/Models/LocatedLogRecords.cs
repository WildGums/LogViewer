// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocatedLogRecords.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System.Collections.ObjectModel;
    using Catel.Data;

    public class LocatedLogRecords : CheckableFilesLocation
    {
        public ObservableCollection<SetOfLogRecords> SetOfLogRecords { get; set; } 
    }
}