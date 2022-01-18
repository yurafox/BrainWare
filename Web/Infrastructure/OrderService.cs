using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Web.Models;
using System.Linq;

namespace Web.Infrastructure
{
    public class OrderService : IOrderService
    {
        private readonly Database _db = new Database();

        private List<Order> GetOrders(int companyId) {
            var sql =
                "SELECT c.name, o.description, o.order_id " +
                "FROM [company] c " +
                "INNER JOIN [order] o on c.company_id=o.company_id " +
                "WHERE c.company_id = @cid";

            var sqlParams = new SqlParameter[] {
                new SqlParameter("cid", companyId)
            };

            return _db.ExecuteReader(
                sql,
                sqlParams,
                x => {
                    return new Order()
                    {
                        CompanyName = x.GetString(0),
                        Description = x.GetString(1),
                        OrderId = x.GetInt32(2),
                        OrderProducts = new List<OrderProduct>()
                    };
                }
            ).ToList();
        }

        private List<OrderProduct> GetOrderProducts() {
            var sql =
                "SELECT op.price, op.order_id, op.product_id, op.quantity, p.name, p.price " +
                "FROM [orderproduct] op " +
                "INNER JOIN [product] p on op.product_id=p.product_id";

            return _db.ExecuteReader(
                sql,
                null,
                x => {
                    return new OrderProduct()
                    {
                        OrderId = x.GetInt32(1),
                        ProductId = x.GetInt32(2),
                        Price = x.GetDecimal(0),
                        Quantity = x.GetInt32(3),
                        Product = new Product()
                        {
                            Name = x.GetString(4),
                            Price = x.GetDecimal(5)
                        }
                    };
                }
            ).ToList();
        }

        public List<Order> GetOrdersForCompany(int companyId)
        {
            var orders = GetOrders(companyId);
            var orderproducts = GetOrderProducts();

            return orders.Select(o =>
            {
                var orderProductsList = orderproducts.Where(x => x.OrderId == o.OrderId).ToList();
                o.OrderProducts = orderProductsList;
                o.OrderTotal = orderProductsList.Sum(x => x.Price * x.Quantity);
                return o;
            }).ToList();
        }
    }
}