// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationNode.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Models
{
    using System.ComponentModel;
    using Catel.Data;

    public abstract class NavigationNode : ModelBase
    {
        #region Properties
        public string Name { get; set; }
        public string FullName { get; set; }
        public bool IsSelected { get; set; }
        public bool IsItemSelected { get; set; }

        [DefaultValue(true)]
        public bool IsVisible { get; set; }

        public abstract bool AllowMultiSelection { get; }
        #endregion
    }
}