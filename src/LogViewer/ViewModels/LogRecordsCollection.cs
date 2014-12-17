// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogRecordsCollection.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace LogViewer.ViewModels
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Data;

    using LogViewer.Models;

    public class LogRecordsCollection : ListCollectionView, INotifyPropertyChanged
    {
        public LogRecordsCollection(ObservableCollection<LogRecord> collection)
            : base(collection)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}