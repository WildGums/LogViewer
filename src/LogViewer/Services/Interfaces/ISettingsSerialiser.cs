namespace LogViewer.Services
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Models;

    public interface ISettingsSerialiser
    {
        IEnumerable<Company> DeserializeCompanies();
    }
}