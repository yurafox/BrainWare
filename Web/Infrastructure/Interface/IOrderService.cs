using System.Collections.Generic;
using Web.Models;

namespace Web.Infrastructure
{
    public interface IOrderService
    {
        List<Order> GetOrdersForCompany(int companyId);
    }
}
