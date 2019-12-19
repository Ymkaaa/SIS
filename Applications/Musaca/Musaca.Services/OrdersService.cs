using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Musaca.Data;
using Musaca.Models;

namespace Musaca.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly MusacaDbContext context;

        public OrdersService(MusacaDbContext context)
        {
            this.context = context;
        }

        public Order CreateOrder(string userId)
        {
            Order order = this.context.Orders.FirstOrDefault(x => x.CashierId == userId && x.Status == OrderStatus.Active);

            if (order != null)
            {
                return order;
            }

            order = new Order()
            {
                CashierId = userId,
                IssuedOn = DateTime.UtcNow
            };

            this.context.Orders.Add(order);
            this.context.SaveChanges();

            return order;
        }

        public bool AddProduct(Product product, string userId)
        {
            Order order = this.GetCurrentActiveOrder(userId);

            if (order == null)
            {
                order = this.CreateOrder(userId);
            }

            this.context.OrderProducts.Add(new OrderProduct()
            {
                ProductId = product.Id,
                OrderId = order.Id
            });
            
            this.context.SaveChanges();

            return true;
        }

        public Order CompleteOrder(string orderId, string userId)
        {
            Order orderFromDb = this.context.Orders.SingleOrDefault(o => o.Id == orderId);

            orderFromDb.Status = OrderStatus.Completed;

            this.context.Update(orderFromDb);
            this.context.SaveChanges();

            return orderFromDb;
        }
        
        public List<Order> GetAllCompletedOrders(string userId)
        {
            return this.context.Orders.Where(o => o.CashierId == userId && o.Status == OrderStatus.Completed).ToList();
        }

        public Order GetCurrentActiveOrder(string userId)
        {
            User user = this.context.Users.Include(x => x.Orders).First(x => x.Id == userId);

            return user.Orders.SingleOrDefault(x => x.Status == OrderStatus.Active);
        }

        public List<Product> GetOrderProducts(string orderId)
        {
            List<string> productIds = this.context.OrderProducts.Where(x => x.OrderId == orderId).Select(x => x.ProductId).ToList();
            List<Product> products = new List<Product>();

            foreach (string id in productIds)
            {
                products.Add(this.context.Products.SingleOrDefault(p => p.Id == id));
            }

            return products;
        }
    }
}
