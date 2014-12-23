// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchTemplate.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    public class SearchTemplate : SimplyClearableModel
    {
        #region Properties
        public string TemplateString { get; set; }

        public bool MatchCase { get; set; }

        public bool MatchWholeWord { get; set; }

        public string RegularExpression { get; set; }
        #endregion
    }
}