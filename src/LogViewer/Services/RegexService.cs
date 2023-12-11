namespace LogViewer.Services
{
    using System;

    internal class RegexService : IRegexService
    {
        public string ConvertToRegex(string templateString, bool matchCase, bool matchWholeWord)
        {
            ArgumentNullException.ThrowIfNull(templateString);

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
    }
}
