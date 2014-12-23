// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRegexService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    public interface IRegexService
    {
        string ConvertToRegex(string templateString, bool matchCase, bool matchWholeWord);
    }
}