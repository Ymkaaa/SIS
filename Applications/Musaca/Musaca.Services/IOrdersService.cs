using Musaca.Models;
using System.Collections.Generic;

namespace Musaca.Services
{
    public interface IOrdersService
    {
        Order CreateOrder(string userId);

        bool AddProduct(Product product, string userId);

        Order CompleteOrder(string orderId, string userId);

        List<Order> GetAllCompletedOrders(string userId);

        Order GetCurrentActiveOrder(string userId);

        List<Product> GetOrderProducts(string orderId);
    }
}
