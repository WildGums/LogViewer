namespace LogViewer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Xml;
    using Models;
    using YAXLib;
    using Path = Catel.IO.Path;

    public class SettingsSerialiser : ISettingsSerialiser
    {
        public IEnumerable<Company> DeserializeCompanies()
        {
            var yaxSerializer = new YAXSerializer(typeof(Company));
            var applicationDataDirectory = Path.GetApplicationDataDirectory();

            
           // TODO: inplement
            throw new NotImplementedException();
        }
    }
}