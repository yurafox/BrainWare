using System.Collections.Generic;
using System.Web.Http;
using Web.Infrastructure;
using Web.Models;

namespace Web.Controllers
{
    public class OrderController : ApiController
    {
        [HttpGet]
        public IEnumerable<Order> GetOrders(int id)
        {
            var data = new OrderService();
            return data.GetOrdersForCompany(id = 1);
        }
    }
}
