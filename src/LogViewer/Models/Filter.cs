// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System;
    using Catel.Data;

    public class Filter : ModelBase
    {
        public bool ShowInfo { get; set; }
        public bool ShowDebug { get; set; }
        public bool ShowWarning { get; set; }
        public bool ShowError { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string TextToFind { get; set; }
    }
}