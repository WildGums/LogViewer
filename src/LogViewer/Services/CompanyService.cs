// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.Collections;
    using Catel.Configuration;
    using Catel.Reflection;
    using Models;
    using Orchestra.Services;
    using Path = Catel.IO.Path;

    public class CompanyService : ICompanyService
    {
        #region Fields
        private readonly IAppDataService _appDataService;
        private readonly IProductService _productService;
        private readonly IConfigurationService _configurationService;
        #endregion

        #region Constructors
        public CompanyService(IAppDataService appDataService, IProductService productService, IConfigurationService configurationService)
        {
            Argument.IsNotNull(() => appDataService);
            Argument.IsNotNull(() => productService);
            Argument.IsNotNull(() => configurationService);

            _appDataService = appDataService;
            _productService = productService;
            _configurationService = configurationService;
        }
        #endregion

        #region ICompanyService Members
        public Company CreateCompanyByDirectoryPath(string companyFolder)
        {
            Argument.IsNotNullOrEmpty(() => companyFolder);

            return CreateCompanyByName(new DirectoryInfo(companyFolder).Name);
        }

        public IEnumerable<Company> LoadCompanies()
        {
            string defaultCompanyName = AssemblyHelper.GetEntryAssembly().Company();
            var companyNames = _configurationService.GetValue("Companies", defaultCompanyName);
            return companyNames.Split(',').Select(CreateCompanyByName);
        }

        public Company CreateCompanyByName(string companyName)
        {
            Argument.IsNotNullOrEmpty(() => companyName);

            var fullCompanyFolderPath = Path.Combine(_appDataService.GetRootAppDataFolder(), companyName);
            var products = Directory.GetDirectories(fullCompanyFolderPath).Select(folder => _productService.CreateNewProductItem(folder) as NavigationNode);

            var company = new Company
            {
                Name = companyName
            };

            company.Children.AddRange(products);

            return company;
        }

        public void SaveCompanies(IEnumerable<Company> companies)
        {
            Argument.IsNotNull(() => companies);

            var stringOfCompanies = string.Join(",", companies.Select(c => c.Name));
            _configurationService.SetValue("Companies", stringOfCompanies);
        }
        #endregion
    }
}