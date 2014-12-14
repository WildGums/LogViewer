namespace LogViewer.Factories
{
    using Models;

    public interface IProductFactory
    {
        Product CreateNewProductItem(string productFolder);
    }
}