namespace LogViewer.Models
{
    using System;
    using System.ComponentModel;

    public class Filter : SimplyClearableModel
    {
        public Filter()
        {
            SearchTemplate = new SearchTemplate
            {
                UseFullTextSearch = true 
            };

            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
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
        public bool IsUseDateRange { get; set; }
    }
}
