// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAggregateLogService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Models;

    public interface IAggregateLogService
    {
        Log AggregateLog { get; }
    }
}