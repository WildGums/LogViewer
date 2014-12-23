// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.ViewModels
{
    using System.ComponentModel;

    using Catel.Fody;
    using Catel.MVVM;

    using LogViewer.Models;
    using LogViewer.Services;

    public class RibbonViewModel : ViewModelBase
    {
        #region Fields
        private readonly IRegexService _regexService;
        #endregion

        #region Constructors
        public RibbonViewModel(LogViewerModel logViewerModel, IRegexService regexService)
        {
            _regexService = regexService;
            Filter = logViewerModel.Filter;

            SearchTemplate.PropertyChanged += OnSearchTemplatePropertyChanged;
        }
        #endregion

        #region Properties
        [Model]
        [Expose("StartDate")]
        [Expose("EndDate")]
        [Expose("ShowInfo")]
        [Expose("ShowDebug")]
        [Expose("ShowWarning")]
        [Expose("ShowError")]
        [Expose("UseTextSearch")]
        [Expose("UseDateRange")]
        public Filter Filter { get; set; }

        [ViewModelToModel("Filter")]
        public SearchTemplate SearchTemplate { get; set; }
        #endregion

        #region Methods
        private void OnSearchTemplatePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RegularExpression")
            {
                return;
            }

            SearchTemplate.RegularExpression = _regexService.ConvertToRegex(SearchTemplate.TemplateString, SearchTemplate.MatchCase, SearchTemplate.MatchWholeWord);
        }
        #endregion
    }
}