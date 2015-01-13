// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimplyClearableModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
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