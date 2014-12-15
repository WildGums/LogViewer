namespace LogViewer.Services
{
    using Models;

    public interface IProductService
    {
        Product CreateNewProductItem(string productFolder);
    }
}