// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighlightableTextBlock.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Controls
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class HighlightableTextBlock : TextBlock
    {
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
            set
            {
                var regEx = RegularExpression;

                if (string.IsNullOrWhiteSpace(regEx) || !IsValidRegex(regEx))
                {
                    base.Text = value;
                    return;
                }

                var inlines = Inlines;

                inlines.Clear();
                var split = Regex.Split(value, regEx, RegexOptions.ExplicitCapture);
                if (split.Max(x => x.Length) == 1)
                {
                    base.Text = value;
                    return;
                }

                foreach (var str in split)
                {
                    var run = new Run(str);
                    if (Regex.IsMatch(str, regEx, RegexOptions.ExplicitCapture))
                    {
                        run.Background = HighlightBackground;
                        run.Foreground = HighlightForeground;
                    }

                    inlines.Add(run);
                }
            }
        }
        #endregion

        #region Methods
        public void UpdateText()
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