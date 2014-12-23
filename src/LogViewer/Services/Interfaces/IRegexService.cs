namespace LogViewer.Services
{
    public interface IRegexService
    {
        string ConvertToRegex(string templateString, bool matchCase, bool matchWholeWord);
    }
}