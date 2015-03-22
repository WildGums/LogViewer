// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFileService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Catel;
    using Catel.Collections;
    using Models;

    public class LogFileService : ILogFileService
    {
        #region Fields
        private static readonly Regex _fileNameMask = new Regex(@"^[a-zA-Z\.]+_(\d{4}-\d{2}-\d{2})_\d{6}_\d+\.log$", RegexOptions.Compiled);
        private readonly ILogReaderService _logReaderService;
        private readonly IIndexSearchService _indexSearchService;
        #endregion

        #region Constructors
        public LogFileService(ILogReaderService logReaderService, IIndexSearchService indexSearchService)
        {
            Argument.IsNotNull(() => logReaderService);
            Argument.IsNotNull(() => indexSearchService);

            _logReaderService = logReaderService;
            _indexSearchService = indexSearchService;
        }
        #endregion

        #region Methods

        #region ILogFileService Members
        public IEnumerable<FileNode> GetLogFiles(string filesFolder)
        {
            Argument.IsNotNullOrEmpty(() => filesFolder);

            return Directory.GetFiles(filesFolder, "*.log", SearchOption.TopDirectoryOnly).Select(LoadLogFile).ToArray();
        }
        #endregion

        public FileNode LoadLogFile(string fileName)
        {
            Argument.IsNotNullOrEmpty(() => fileName);

            var logFile = new FileNode(new FileInfo(fileName));

            logFile.Name = logFile.FileInfo.Name;
            logFile.IsUnifyNamed = _fileNameMask.IsMatch(logFile.FileInfo.Name);
            if (!logFile.IsUnifyNamed)
            {
                logFile.Name = logFile.FileInfo.Name;
            }
            else
            {
                logFile.Name = logFile.FileInfo.Name;
                var dateTimeString = Regex.Match(logFile.FileInfo.Name, @"(\d{4}-\d{2}-\d{2})").Value;
                logFile.DateTime = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd", null, DateTimeStyles.None);
            }

            try
            {
                logFile.Records.AddRange(_logReaderService.LoadRecordsFromFile(logFile));

                _indexSearchService.EnsureFullTextIndex(logFile);
            }
            catch
            {
                // ignored
            }

            return logFile;
        }
        #endregion
    }
}