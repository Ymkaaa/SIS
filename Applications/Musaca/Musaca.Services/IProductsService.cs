using Musaca.Models;
using System.Collections.Generic;

namespace Musaca.Services
{
    public interface IProductsService
    {
        Product Create(Product product);

        Product GetByName(string name);

        List<Product> GetAll();
    }
}
