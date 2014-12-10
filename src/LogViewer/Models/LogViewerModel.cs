// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogViewerModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Catel.Data;
    using Catel.IO;
    using Services;
    using YAXLib;

    public class LogViewerModel : ModelBase
    {
        public LogViewerModel(ISettingsSerialiser settingsSerialiser)
        {
            /*Companies = settingsSerialiser.DeserializeCompanies();
            
            var applicationDataDirectory = Path.GetApplicationDataDirectory();
            var serializer = new YAXLib.YAXSerializer(typeof (Company));
            serializer.Deserialize()
            var product = new Product() {Name = "ddd"};
            Companies.Add(new Company { Name = "sssss", IsChecked = null, Products = new ObservableCollection<Product>(new[] { product }) });*/
        }
        public LogViewerConfig Config { get; set; }
        public Filter Filter { get; set; }
        public ObservableCollection<Company> Companies { get; set; }
    }
}