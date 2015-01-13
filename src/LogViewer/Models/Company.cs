// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Company.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
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