// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel.Fody;
    using Catel.MVVM;
    using Models;

    public class RibbonViewModel : ViewModelBase
    {
        public RibbonViewModel(LogViewerModel logViewerModel)
        {
            LogViewer = logViewerModel;
        }



        protected override async Task Initialize()
        {
            await base.Initialize();
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        [Model]        
        public LogViewerModel LogViewer { get; set; }

        [Model]        
        [ViewModelToModel("LogViewer")]
        public Filter Filter { get; set; }

        [ViewModelToModel("Filter")]
        public DateTime StartDate { get; set; }

        [ViewModelToModel("Filter")]
        public DateTime EndDate { get; set; }        
        
        [ViewModelToModel("Filter")]
        public bool ShowInfo { get; set; }

        [ViewModelToModel("Filter")]
        public bool ShowDebug { get; set; }

        [ViewModelToModel("Filter")]
        public bool ShowWarning { get; set; }

        [ViewModelToModel("Filter")]
        public bool ShowError { get; set; }

        [ViewModelToModel("Filter")]
        public string SearchTemplate { get; set; }
    }
}