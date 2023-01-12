namespace LogViewer.Converters
{
    using System;
    using System.Globalization;
    using Catel.MVVM.Converters;

    public class TimestampToStringConverter : ValueConverterBase
    {
        #region Methods
        protected override object Convert(object value, Type targetType, object parameter)
        {
            if (!(value is DateTime))
            {
                return null;
            }
            
            var dateTimeValue = (DateTime) value;
            return dateTimeValue.ToString(CultureInfo.CurrentCulture.DateTimeFormat);
        }
        #endregion
    }
}