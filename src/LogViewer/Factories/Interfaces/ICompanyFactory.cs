// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICompanyFactory.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Factories
{
    using Models;

    public interface ICompanyFactory
    {
        Company CreateNewCompanyItem(string companyFolder);
    }
}