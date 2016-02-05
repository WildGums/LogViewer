// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegexService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    public interface IRegexService
    {
        string ConvertToRegex(string templateString, bool matchCase, bool matchWholeWord);
    }
}