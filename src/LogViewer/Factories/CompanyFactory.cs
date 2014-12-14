// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyFactory.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Factories
{
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Extensions;
    using Models;
    using Models.Base;
    using Orchestra.Services;
    using Path = Catel.IO.Path;

    public class CompanyFactory : ICompanyFactory
    {
        private readonly IAppDataService _appDataService;
        private readonly IProductFactory _productFactory;

        public CompanyFactory(IAppDataService appDataService, IProductFactory productFactory)
        {
            _appDataService = appDataService;
            _productFactory = productFactory;
        }

        public Company CreateNewCompanyItem(string companyFolder)
        {

            var rootAppDataDir = _appDataService.GetRootAppDataFolder();
            var companyName = companyFolder.Substring(rootAppDataDir.Length + 1);


            var fullCompanyFolderPath = Path.Combine(_appDataService.GetRootAppDataFolder(), companyFolder);
            var products = Directory.GetDirectories(fullCompanyFolderPath).Select(folder => _productFactory.CreateNewProductItem(folder) as TreeNode);

            return new Company {Name = companyName, Children = new ObservableCollection<TreeNode>(products)};
        }
    }
}