// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Filter.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System;
    using System.ComponentModel;
    using System.Xml.Linq;

    using Catel.Data;

    public class Filter : SimplyClearableModel
    {
        public Filter()
        {
            SearchTemplate = new SearchTemplate();            
        }

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
        public SearchTemplate SearchTemplate { get; set; }
        [DefaultValue(true)]
        public bool UseTextSearch { get; set; }
        [DefaultValue(false)]
        public bool UseFilterRange { get; set; }
    }
}