namespace LogViewer.Services
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Models;
    using Models.Base;

    public class ProductService : IProductService
    {
        private readonly ILogFileService _logFileService;
        private readonly ILogRecordService _logRecordService;

        public ProductService(ILogFileService logFileService, ILogRecordService logRecordService)
        {
            _logFileService = logFileService;
            _logRecordService = logRecordService;
        }

        public Product CreateNewProductItem(string productFolder)
        {
            var productName = productFolder.Substring(productFolder.LastIndexOf('\\') + 1);
            var logFiles = _logFileService.GetLogFIles(productFolder);

            return new Product { Name = productName, LogFiles = new ObservableCollection<LogFile>(logFiles) };
        }
    }
}