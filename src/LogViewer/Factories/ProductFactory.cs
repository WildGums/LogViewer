namespace LogViewer.Factories
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Models;
    using Models.Base;
    using Services;

    public class ProductFactory : IProductFactory
    {
        private readonly ILogRecordService _logRecordService;

        public ProductFactory(ILogRecordService logRecordService)
        {
            _logRecordService = logRecordService;
        }

        public Product CreateNewProductItem(string productFolder)
        {
            var productName = productFolder.Substring(productFolder.LastIndexOf('\\') + 1);

            var logRecords = Directory.GetFiles(productFolder, "*.log", SearchOption.TopDirectoryOnly).SelectMany(file => _logRecordService.LoadRecordsFromFile(file)).ToArray();

            //   var logFiles = Directory.GetFiles(productFolder, "*.log", SearchOption.TopDirectoryOnly).Select(fileName => _logFileFactory.CreateNewLogFileItem(fileName));
            return new Product { Name = productName/*, Children = new ObservableCollection<TreeNode>(logFiles)*/ };
        }
    }
}