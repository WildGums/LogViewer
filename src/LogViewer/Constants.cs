// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2014 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace LogViewer
{
    using Orc.Squirrel;

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
                    new UpdateChannel("Beta", "http://downloads.sesolutions.net.au/logviewer/beta")
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
        }

        public static class Filter
        {
            public const string ResetSearchTemplate = "Filter.ResetSearchTemplate";
        }

        public static class Settings
        {
            public const string General = "Settings.General";
        }

        public static class Help
        {
            public const string About = "Help.About";
        }
    }

    public static class Workspaces
    {
        public const string DefaultWorkspaceName = "Default";
    }
}