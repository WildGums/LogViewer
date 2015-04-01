// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileNodeService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Catel;
    using Catel.Collections;
    using Models;

    public class FileNodeService : IFileNodeService
    {
        #region Fields
        private static readonly Regex _fileNameMask = new Regex(@"^[a-zA-Z\.]+_(\d{4}-\d{2}-\d{2})_\d{6}_\d+\.log$", RegexOptions.Compiled);
        private readonly ILogReaderService _logReaderService;
        private readonly IIndexSearchService _indexSearchService;
        #endregion

        #region Constructors
        public FileNodeService(ILogReaderService logReaderService, IIndexSearchService indexSearchService)
        {
            Argument.IsNotNull(() => logReaderService);
            Argument.IsNotNull(() => indexSearchService);

            _logReaderService = logReaderService;
            _indexSearchService = indexSearchService;
        }
        #endregion

        #region Methods

 
        public FileNode LoadFileNode(string fileName)
        {
            Argument.IsNotNullOrEmpty(() => fileName);

            var fileNode = new FileNode(new FileInfo(fileName));

            fileNode.Name = fileNode.FileInfo.Name;
            fileNode.IsUnifyNamed = _fileNameMask.IsMatch(fileNode.FileInfo.Name);
            if (!fileNode.IsUnifyNamed)
            {
                fileNode.Name = fileNode.FileInfo.Name;
            }
            else
            {
                fileNode.Name = fileNode.FileInfo.Name;
                var dateTimeString = Regex.Match(fileNode.FileInfo.Name, @"(\d{4}-\d{2}-\d{2})").Value;
                fileNode.DateTime = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd", null, DateTimeStyles.None);
            }

            try
            {
                fileNode.Records.AddRange(_logReaderService.LoadRecordsFromFile(fileNode));

                _indexSearchService.EnsureFullTextIndex(fileNode);
            }
            catch
            {
                // ignored
            }

            return fileNode;
        }

        public void ReloadFileNode(FileNode fileNode)
        {
            var logRecords = fileNode.Records;

            logRecords.Clear();
            logRecords.AddRange(_logReaderService.LoadRecordsFromFile(fileNode));

            _indexSearchService.EnsureFullTextIndex(fileNode);
        }
        #endregion
    }
}