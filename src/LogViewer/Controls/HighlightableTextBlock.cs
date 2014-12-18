// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighlightableTextBlock.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer.Controls
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class HighlightableTextBlock : TextBlock
    {
        private string _regularExpression;

        public string SearchTemplate
        {
            get
            {
                if (null == (string) GetValue(SearchTemplateProperty))
                {
                    SetValue(SearchTemplateProperty, string.Empty);
                }
                return (string) GetValue(SearchTemplateProperty);
            }
            set
            {
                SetValue(SearchTemplateProperty, value);
                UpdateRegex();
            }
        }

        public string HighlightableText
        {
            get { return (string) GetValue(HighlightableTextProperty); }
            set { SetValue(HighlightableTextProperty, value); }
        }

        public Brush HighlightForeground
        {
            get
            {
                if ((Brush) GetValue(HighlightForegroundProperty) == null)
                {
                    SetValue(HighlightForegroundProperty, Brushes.Black);
                }
                return (Brush) GetValue(HighlightForegroundProperty);
            }
            set { SetValue(HighlightForegroundProperty, value); }
        }

        public Brush HighlightBackground
        {
            get
            {
                if ((Brush) GetValue(HighlightBackgroundProperty) == null)
                {
                    SetValue(HighlightBackgroundProperty, Brushes.Yellow);
                }
                return (Brush) GetValue(HighlightBackgroundProperty);
            }
            set { SetValue(HighlightBackgroundProperty, value); }
        }

        private new string Text
        {
            set
            {
                if (string.IsNullOrWhiteSpace(RegularExpression) || !IsValidRegex(RegularExpression))
                {
                    base.Text = value;
                    return;
                }

                Inlines.Clear();
                var split = Regex.Split(value, RegularExpression, RegexOptions.IgnoreCase);
                foreach (var str in split)
                {
                    var run = new Run(str);
                    if (Regex.IsMatch(str, RegularExpression, RegexOptions.IgnoreCase))
                    {
                        run.Background = HighlightBackground;
                        run.Foreground = HighlightForeground;
                    }
                    Inlines.Add(run);
                }
            }
        }

        public string RegularExpression
        {
            get { return _regularExpression; }
            set
            {
                _regularExpression = value;
                Text = base.Text;
            }
        }

        public static void SearchTemplatePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var stb = obj as HighlightableTextBlock;
            if (stb == null)
            {
                return;
            }

            stb.UpdateRegex();
        }

        public static void HighlightableTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var stb = obj as HighlightableTextBlock;
            stb.Text = stb.HighlightableText;
        }

        private void UpdateRegex()
        {
            var newRegularExpression = string.Empty;
/*
            if (string.IsNullOrEmpty(SearchTemplate))
            {
                return;
            }*/

            if (newRegularExpression.Length > 0)
            {
                newRegularExpression += "|";
            }
            newRegularExpression += RegexWrap(SearchTemplate);

            if (RegularExpression != newRegularExpression)
            {
                RegularExpression = newRegularExpression;
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
                Regex.Match("", regEx);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        private string RegexWrap(string str)
        {
            return String.Format("(?={0})|(?<={0})", str);
        }

        public static readonly DependencyProperty SearchTemplateProperty = DependencyProperty.Register("SearchTemplate", typeof (string), typeof (HighlightableTextBlock), new PropertyMetadata(SearchTemplatePropertyChanged));
        public static readonly DependencyProperty HighlightableTextProperty = DependencyProperty.Register("HighlightableText", typeof (string), typeof (HighlightableTextBlock), new PropertyMetadata(HighlightableTextChanged));
        public static readonly DependencyProperty HighlightForegroundProperty = DependencyProperty.Register("HighlightForeground", typeof (Brush), typeof (HighlightableTextBlock));
        public static readonly DependencyProperty HighlightBackgroundProperty = DependencyProperty.Register("HighlightBackground", typeof (Brush), typeof (HighlightableTextBlock));
    }
}