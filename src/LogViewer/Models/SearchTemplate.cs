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

    public class SearchTemplate : ModelBase
    {
        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RegularExpression")
            {
                return;
            }

            CreateRegex();
            base.OnPropertyChanged(e);
        }

        private async Task CreateRegex()
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

        public void ClearDirtyFlag()
        {
            ClearIsDirtyOnAllChilds();
        }
    }
}