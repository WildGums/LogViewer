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
    using Catel.Windows.Threading;

    public class HighlightableTextBlock : TextBlock
    {
        public HighlightableTextBlock()
        {

        }

        #region Properties
        public string RegularExpression
        {
            get { return (string)GetValue(RegularExpressionProperty); }
            set { SetValue(RegularExpressionProperty, value); }
        }

        public static readonly DependencyProperty RegularExpressionProperty = DependencyProperty.Register("RegularExpression", typeof(string), typeof(HighlightableTextBlock),
            new PropertyMetadata(string.Empty, async (sender, e) => await ((HighlightableTextBlock)sender).UpdateHighlighting()));

        public string HighlightableText
        {
            get { return (string)GetValue(HighlightableTextProperty); }
            set { SetValue(HighlightableTextProperty, value); }
        }

        public static readonly DependencyProperty HighlightableTextProperty = DependencyProperty.Register("HighlightableText", typeof(string), typeof(HighlightableTextBlock),
            new PropertyMetadata(async (sender, e) => await ((HighlightableTextBlock)sender).UpdateHighlighting()));

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


        private async Task UpdateHighlighting()
        {
            var textToCheck = HighlightableText;
            var regex = RegularExpression;
            // TODO: what should we do with value? Only add highlighted text?
            var value = textToCheck;
            var inlines = Inlines;

            if (string.IsNullOrWhiteSpace(textToCheck) || string.IsNullOrWhiteSpace(regex) || !IsValidRegex(regex))
            {
                UpdateText(value, false);
                return;
            }

            await Task.Factory.StartNew(() =>
            {
                var split = Regex.Split(textToCheck, regex, RegexOptions.ExplicitCapture);
                if (split.Max(x => x.Length) == 1)
                {
                    UpdateText(value, true);
                    return;
                }

                if (split.Length == 0)
                {
                    UpdateText(value, true);
                    return;
                }

                foreach (var str in split)
                {
                    var match = Regex.IsMatch(str, regex, RegexOptions.ExplicitCapture);
                    Dispatcher.BeginInvokeIfRequired(() =>
                    {
                        var run = new Run(str);
                        if (match)
                        {
                            run.Background = HighlightBackground;
                            run.Foreground = HighlightForeground;
                        }

                        inlines.Add(run);
                    });
                }
            });
        }
        #endregion

        #region Methods
        private void UpdateText(string newText, bool clearInlines)
        {
            Dispatcher.BeginInvokeIfRequired(() =>
            {
                if (clearInlines)
                {
                    var inlines = Inlines;
                    inlines.Clear();
                }

                if (!string.Equals(Text, newText))
                {
                    Text = newText;
                }
            });
        }

        public bool IsValidRegex(string regex)
        {
            if (string.IsNullOrEmpty(regex))
            {
                return false;
            }

            try
            {
                Regex.Match(string.Empty, regex);
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