// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegexService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    public interface IRegexService
    {
        string ConvertToRegex(string templateString, bool matchCase, bool matchWholeWord);
    }
}