// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Linq;
    using Catel;
    using Catel.Collections;
    using Models;

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
            var logFiles = _logFileService.GetLogFIles(productFolder);

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