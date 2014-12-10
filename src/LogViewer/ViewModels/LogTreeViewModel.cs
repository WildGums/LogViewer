// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogTreeViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using Catel.Fody;
    using Catel.Logging;
    using Catel.MVVM;
    using Models;
    using Views;

    public class LogTreeViewModel : ViewModelBase
    {
        public LogTreeViewModel(LogViewerModel logViewerModel)
        {
            LogViewer = logViewerModel;
        }

        [Model]
        [Expose("Companies")]
        public LogViewerModel LogViewer { get; set; }
    }
}