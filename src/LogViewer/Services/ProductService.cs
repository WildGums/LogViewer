// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.IO;
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Models;
    using Path = Catel.IO.Path;

    public class ProductService : IProductService
    {
        #region Fields
        private readonly ILogFileService _logFileService;
        #endregion

        #region Constructors
        public ProductService(ILogFileService logFileService)
        {
            Argument.IsNotNull(() => logFileService);

            _logFileService = logFileService;
        }
        #endregion

        #region IProductService Members
        public Product CreateNewProductItem(string productFolder)
        {
            Argument.IsNotNullOrEmpty(() => productFolder);

            var productName = productFolder.Substring(productFolder.LastIndexOf('\\') + 1);
            var nestedLogFolder = Path.Combine(productFolder, "log");
            if (Directory.Exists(nestedLogFolder))
            {
                productFolder = nestedLogFolder;
            }

            var logFiles = _logFileService.GetLogFiles(productFolder);

            var files = logFiles as LogFile[] ?? logFiles.ToArray();

            var product = new Product
            {
                Name = productName
            };

            product.LogFiles.AddRange(files);
            product.Children.AddRange(files);

            return product;
        }
        #endregion
    }
}