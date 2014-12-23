// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Company.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    public class Company : NavigationNode
    {
        #region Properties
        public override bool AllowMultiSelection
        {
            get { return false; }
        }
        #endregion
    }
}