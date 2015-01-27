// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchTemplate.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
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

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(this.TemplateString) || string.IsNullOrEmpty(this.TemplateString.Trim());
        }
        #endregion
    }
}