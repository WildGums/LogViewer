﻿namespace LogViewer.Models
{
    public class SearchTemplate : SimplyClearableModel
    {
        #region Properties
        public string TemplateString { get; set; }

        public bool UseFullTextSearch { get; set; }

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