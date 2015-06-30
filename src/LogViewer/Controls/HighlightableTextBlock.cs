// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighlightableTextBlock.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Controls
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;
    using Catel.IoC;
    using Catel.Services;

    public class HighlightableTextBlock : TextBlock
    {
        #region Fields
        private readonly IDispatcherService _dispatcherService;
        #endregion

        public HighlightableTextBlock()
        {
            var serviceLocator = this.GetServiceLocator();
            _dispatcherService = serviceLocator.ResolveType<IDispatcherService>();
        }

        #region Properties
        public string RegularExpression
        {
            get { return (string)GetValue(RegularExpressionProperty); }
            set { SetValue(RegularExpressionProperty, value); }
        }

        public static readonly DependencyProperty RegularExpressionProperty = DependencyProperty.Register("RegularExpression", typeof(string), typeof(HighlightableTextBlock), new PropertyMetadata(string.Empty, RegularExpressionPropertyChanged));

        public string HighlightableText
        {
            get { return (string)GetValue(HighlightableTextProperty); }
            set { SetValue(HighlightableTextProperty, value); }
        }

        public static readonly DependencyProperty HighlightableTextProperty = DependencyProperty.Register("HighlightableText", typeof(string), typeof(HighlightableTextBlock), new PropertyMetadata(HighlightableTextChanged));

        public Brush HighlightForeground
        {
            get { return (Brush)GetValue(HighlightForegroundProperty); }
            set { SetValue(HighlightForegroundProperty, value); }
        }

        public static readonly DependencyProperty HighlightForegroundProperty = DependencyProperty.Register("HighlightForeground", typeof(Brush), typeof(HighlightableTextBlock), new PropertyMetadata(Brushes.Black));

        public Brush HighlightBackground
        {
            get { return (Brush)GetValue(HighlightBackgroundProperty); }
            set { SetValue(HighlightBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HighlightBackgroundProperty = DependencyProperty.Register("HighlightBackground", typeof(Brush), typeof(HighlightableTextBlock), new PropertyMetadata(Brushes.Yellow));
        

        private new string Text
        {
            set { HilightText(value); }
        }

        private async void HilightText(string value)
        {
            await Task.Factory.StartNew(() =>
            {
                string regEx = string.Empty;

                _dispatcherService.Invoke(() =>
                {
                    regEx = RegularExpression;
                }, true);

                if (string.IsNullOrWhiteSpace(regEx) || !IsValidRegex(regEx))
                {
                    _dispatcherService.Invoke(() => { base.Text = value; }, true);
                    return;
                }

                var inlines = Inlines;

                _dispatcherService.Invoke(() => inlines.Clear(), true);

                var split = Regex.Split(value, regEx, RegexOptions.ExplicitCapture);
                if (split.Max(x => x.Length) == 1)
                {
                    _dispatcherService.Invoke(() => { base.Text = value; }, true);
                    return;
                }

                foreach (var str in split)
                {
                    var match = Regex.IsMatch(str, regEx, RegexOptions.ExplicitCapture);
                    _dispatcherService.Invoke(() =>
                    {
                        var run = new Run(str);
                        if (match)
                        {
                            run.Background = HighlightBackground;
                            run.Foreground = HighlightForeground;
                        }

                        inlines.Add(run);
                    }, true);
                    
                }
            });
        }
        #endregion

        #region Methods
        private void UpdateText()
        {
            Text = base.Text;
        }

        public static void RegularExpressionPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var textBlock = obj as HighlightableTextBlock;
            if (textBlock == null)
            {
                return;
            }

            textBlock.UpdateText();
        }

        public static void HighlightableTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var textBlock = obj as HighlightableTextBlock;
            if (textBlock != null)
            {
                textBlock.Text = textBlock.HighlightableText;
            }
        }

        public bool IsValidRegex(string regEx)
        {
            if (string.IsNullOrEmpty(regEx))
            {
                return false;
            }

            try
            {
                Regex.Match(string.Empty, regEx);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}