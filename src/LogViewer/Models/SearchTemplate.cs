// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchTemplate.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System;
    using System.ComponentModel;

    using Catel.Data;

    using LogViewer.Extensions;

    public class SearchTemplate : ModelBase
    {
        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RegularExpression")
            {
                return;
            }

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
            base.OnPropertyChanged(e);
        }

        public string TemplateString { get; set; }

        public bool MatchCase { get; set; }

        public bool MatchWholeWord { get; set; }

        public string RegularExpression { get; set; }
    }
}