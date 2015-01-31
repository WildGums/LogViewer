namespace LogViewer.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class BoolToInvisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Constructor to initialize values for BoolToInvisibilityConverter
        /// </summary>
        public BoolToInvisibilityConverter()
        {
            TrueValue = Visibility.Collapsed;
            FalseValue = Visibility.Visible;
        }

        /// <summary>
        /// True visibility value
        /// </summary>
        public Visibility TrueValue { get; set; }

        /// <summary>
        /// False visibility value
        /// </summary>
        public Visibility FalseValue { get; set; }

        /// <summary>
        /// Modifies the value before passing it to the target for display in the UI
        /// </summary>
        /// <param name="value">The source data being passed to the target</param>
        /// <param name="targetType">The type of data expected by the target dependency property</param>
        /// <param name="parameter">An optional parameters to be used in the converter logic</param>
        /// <param name="culture">The culture of the conversion</param>
        /// <returns>
        /// TrueValue if value equals true
        /// FalseValue if value equals false
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool val = System.Convert.ToBoolean(value);
            return val ? TrueValue : FalseValue;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in Systen.Windows.Date.BindingMode.TwoWay binding
        /// </summary>
        /// <param name="value">The source data being passed to the target</param>
        /// <param name="targetType">The type of data expected by the target dependency property</param>
        /// <param name="parameter">An optional parameters to be used in the converter logic</param>
        /// <param name="culture">The culture of the conversion</param>
        /// <returns>
        /// true if value equals TrueValue
        /// false if value equals FalseValue
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return TrueValue.Equals(value) ? true : false;
        }
    }
}
