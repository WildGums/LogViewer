// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICompanyService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using Models;

    public interface ICompanyService
    {
        Company CreateNewCompanyItem(string companyFolder);
    }
}