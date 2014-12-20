// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchTemplate.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Catel.Data;

    using LogViewer.Extensions;

    public class SearchTemplate : SimplyClearableModel
    {
        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RegularExpression")
            {
                return;
            }

            WrapIntoRegex();
            base.OnPropertyChanged(e);
        }

        private async Task WrapIntoRegex()
        {
            string regex = TemplateString;

            if (!MatchCase)
            {
                regex = regex.WrapToIgnoreCaseRegex();
            }

            if (MatchWholeWord)
            {
                regex = regex.WrapToMatchWholeWordRegex();
            }

            regex = regex.WrapToFindRegex();

            RegularExpression = regex;
        }

        public string TemplateString { get; set; }

        public bool MatchCase { get; set; }

        public bool MatchWholeWord { get; set; }

        public string RegularExpression { get; set; }        
    }
}