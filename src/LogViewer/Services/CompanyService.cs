// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Extensions;
    using Models;
    using Models.Base;
    using Orchestra.Services;
    using Path = Catel.IO.Path;

    public class CompanyService : ICompanyService
    {
        private readonly IAppDataService _appDataService;
        private readonly IProductService _productService;

        public CompanyService(IAppDataService appDataService, IProductService productService)
        {
            _appDataService = appDataService;
            _productService = productService;
        }

        public Company CreateNewCompanyItem(string companyFolder)
        {

            var rootAppDataDir = _appDataService.GetRootAppDataFolder();
            var companyName = companyFolder.Substring(rootAppDataDir.Length + 1);


            var fullCompanyFolderPath = Path.Combine(_appDataService.GetRootAppDataFolder(), companyFolder);
            var products = Directory.GetDirectories(fullCompanyFolderPath).Select(folder => _productService.CreateNewProductItem(folder) as NavigationNode);

            return new Company {Name = companyName, Children = new ObservableCollection<NavigationNode>(products)};
        }
    }
}