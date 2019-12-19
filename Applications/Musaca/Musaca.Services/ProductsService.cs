using Musaca.Data;
using Musaca.Models;
using System.Collections.Generic;
using System.Linq;

namespace Musaca.Services
{
    public class ProductsService : IProductsService
    {
        private readonly MusacaDbContext context;

        public ProductsService(MusacaDbContext context)
        {
            this.context = context;
        }

        public Product Create(Product product)
        {
            this.context.Add(product);
            this.context.SaveChanges();

            return product;
        }

        public Product GetByName(string name)
        {
            return this.context.Products.SingleOrDefault(p => p.Name == name);
        }

        public List<Product> GetAll()
        {
            return this.context.Products.ToList();
        }
    }
}
