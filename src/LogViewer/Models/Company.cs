// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Company.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using LogViewer.Models.Base;

    public class Company : NavigationNode
    {
        #region Fields
        private readonly bool _allowMutiselection = false;
        #endregion

        #region Properties
        public override bool AllowMultiselection
        {
            get
            {
                return _allowMutiselection;
            }
        }
        #endregion
    }
}