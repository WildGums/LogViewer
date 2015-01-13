// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
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