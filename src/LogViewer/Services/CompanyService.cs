// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompanyService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Catel;
    using Catel.Configuration;

    using LogViewer.Models;
    using LogViewer.Models.Base;

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
            string defaultCompanyName = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute), false)).Company;
            var companyNames = _configurationService.GetValue("Companies", defaultCompanyName);
            return companyNames.Split(',').Select(CreateCompanyByName);
        }

        public Company CreateCompanyByName(string companyName)
        {
            Argument.IsNotNullOrEmpty(() => companyName);

            var fullCompanyFolderPath = Path.Combine(_appDataService.GetRootAppDataFolder(), companyName);
            var products = Directory.GetDirectories(fullCompanyFolderPath).Select(folder => _productService.CreateNewProductItem(folder) as NavigationNode);

            return new Company { Name = companyName, Children = new ObservableCollection<NavigationNode>(products) };
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