// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System;
    using System.ComponentModel;
    using Catel.Data;

    public class Filter : ModelBase
    {
        [DefaultValue(true)]
        public bool ShowInfo { get; set; }
        [DefaultValue(true)]
        public bool ShowDebug { get; set; }
        [DefaultValue(true)]
        public bool ShowWarning { get; set; }
        [DefaultValue(true)]
        public bool ShowError { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TextToFind { get; set; }
    }
}