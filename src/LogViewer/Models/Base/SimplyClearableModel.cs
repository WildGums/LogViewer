// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimplyClearableModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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
            IsDirty = false;
        }
        #endregion
    }
}