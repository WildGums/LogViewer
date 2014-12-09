// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilesLocation.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System.Collections.ObjectModel;
    using Catel.Data;

    public class FilesLocation : ModelBase
    {
        public string Path { get; set; }
        public string DisplayName { get; set; }
    }
}