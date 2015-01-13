// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICompanyService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using System.Collections.Generic;

    using LogViewer.Models;

    public interface ICompanyService
    {
        #region Methods
        Company CreateCompanyByDirectoryPath(string companyFolder);

        IEnumerable<Company> LoadCompanies();

        Company CreateCompanyByName(string companyName);

        void SaveCompanies(IEnumerable<Company> companies);
        #endregion
    }
}