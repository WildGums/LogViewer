// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimestampToStringConverter.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Converters
{
    using System;
    using Catel.MVVM.Converters;

    public class TimestampToStringConverter : ValueConverterBase<DateTime>
    {
        #region Methods
        protected override object Convert(DateTime value, Type targetType, object parameter)
        {
            return value.ToString(CurrentCulture.DateTimeFormat);
        }
        #endregion
    }
}