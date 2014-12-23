// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Services
{
    using LogViewer.Models;

    public interface IProductService
    {
        #region Methods
        Product CreateNewProductItem(string productFolder);
        #endregion
    }
}