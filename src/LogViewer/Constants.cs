// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using System.Windows.Input;
    using Orc.Squirrel;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public static class Analytics
    {
        public const string AccountId = "UA-54671533-10";

        public static class Application
        {
            public const string Name = "Application";

            public const string StartupTime = "Startup time";
        }
    }

    public static class Settings
    {
        public static class Application
        {
            public static class General
            {
                public const string EnableAnalytics = "General.EnableAnalytics";
                public const bool EnableAnalyticsDefaultValue = true;
            }

            public static class AutomaticUpdates
            {
                public const bool CheckForUpdatesDefaultValue = false;

                public static readonly UpdateChannel[] AvailableChannels =
                {
                    new UpdateChannel("Stable", "http://downloads.sesolutions.net.au/logviewer/stable"),
                    new UpdateChannel("Beta", "http://downloads.sesolutions.net.au/logviewer/beta"),
                    new UpdateChannel("Alpha", "http://downloads.sesolutions.net.au/logviewer/alpha")
                };

                public static readonly UpdateChannel DefaultChannel = AvailableChannels[0];
            }
        }

        public static class Workspace
        {
            public static class General
            {
                public const string EnableTooltips = "General.EnableToolips";
                public const bool EnableTooltipsDefaultValue = false;
            }

            public static class Filter
            {
                public const string ShowDebug = "ShowDebug";
                public const bool ShowDebugDefaultValue = true;
                public const string ShowError = "ShowError";
                public const bool ShowErrorDefaultValue = true;
                public const string ShowInfo = "ShowInfo";
                public const bool ShowInfoDefaultValue = true;
                public const string ShowWarning = "ShowWarning";
                public const bool ShowWarningDefaultValue = true;
                public const string IsUseDateRange = "IsUseDateRange";
                public const bool IsUseDateRangeDefaultValue = false;
                public const string StartDate = "StartDate";
                public const string EndDate = "EndDate";
            }
        }
    }

    public static class Commands
    {
        public static class File
        {
            public const string Exit = "File.Exit";
            public static readonly InputGesture ExitInputGesture = new InputGesture(Key.F4, ModifierKeys.Alt);
        }

        public static class Filter
        {
            public const string ResetSearchTemplate = "Filter.ResetSearchTemplate";
            public static readonly InputGesture ResetSearchTemplateInputGesture = null;

            public const string ExportResult = "Filter.ExportResult";
            public static readonly InputGesture ExportResultInputGesture = null;

            public const string CopyResultToClipboard = "Filter.CopyResultToClipboard";
            public static readonly InputGesture CopyResultToClipboardInputGesture = null;
        }

        public static class Settings
        {
            public const string General = "Settings.General";
            public static readonly InputGesture GeneralInputGesture = new InputGesture(Key.S, ModifierKeys.Alt | ModifierKeys.Control);
        }

        public static class Help
        {
            public const string About = "Help.About";
            public static readonly InputGesture AboutInputGesture = new InputGesture(Key.F1);
        }
    }

    public static class Workspaces
    {
        public const string DefaultWorkspaceName = "Default";
    }
}