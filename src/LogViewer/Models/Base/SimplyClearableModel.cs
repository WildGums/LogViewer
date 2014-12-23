// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimplyClearableModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using Catel.Data;

    public abstract class SimplyClearableModel : ModelBase
    {
        #region Methods
        public void MarkClean()
        {
            var oldValue = HandlePropertyAndCollectionChanges;

            HandlePropertyAndCollectionChanges = false;

            IsDirty = false;

            HandlePropertyAndCollectionChanges = oldValue;
        }
        #endregion
    }
}