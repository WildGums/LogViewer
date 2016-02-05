// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegexService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Catel;

    internal class RegexService : IRegexService
    {
        #region IRegexService Members
        public string ConvertToRegex(string templateString, bool matchCase, bool matchWholeWord)
        {
            Argument.IsNotNull(() => templateString);

            var regex = templateString;

            if (!matchCase)
            {
                regex = regex.WrapToIgnoreCaseRegex();
            }

            if (matchWholeWord)
            {
                regex = regex.WrapToMatchWholeWordRegex();
            }

            return regex.WrapToFindRegex();
        }
        #endregion
    }
}