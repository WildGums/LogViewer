namespace LogViewer.Factories
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Models;
    using Models.Base;
    using Services;

    public class ProductFactory : IProductFactory
    {
        private readonly ILogFileService _logFileService;
        private readonly ILogRecordService _logRecordService;

        public ProductFactory(ILogFileService logFileService, ILogRecordService logRecordService)
        {
            _logFileService = logFileService;
            _logRecordService = logRecordService;
        }

        public Product CreateNewProductItem(string productFolder)
        {
            var productName = productFolder.Substring(productFolder.LastIndexOf('\\') + 1);
            var logFiles = _logFileService.GetLogFIles(productFolder).ToLookup(x => x.HasUnifiedName);
            var notUnifyNamedFIles = logFiles[false];

            var unifyNamedFIles = logFiles[true].GroupBy(x => x.Name).Select(x => _logFileService.CreateLogFilesGroup(x.Key, x.ToArray()));

            return new Product { Name = productName, Children = new ObservableCollection<TreeNode>(notUnifyNamedFIles.Cast<TreeNode>().Union(unifyNamedFIles)) };
        }
    }
}