using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.Controllers;
using System.Linq;

namespace Tests.Controllers
{
    [TestClass]
    public class OrderControllerTest
    {

        [TestMethod]
        public void GetOrdersReturnsOrdersForAGivenCompany()
        {
            // Arrange
            var api_controller = new OrderController();

            // Act
            var response1 = api_controller.GetOrders(1);
            var response2 = api_controller.GetOrders(2);

            // Assert
            var numRows1 = response1.Count();
            var numRows2 = response2.Count();
            Assert.AreEqual(3, numRows1);
            Assert.AreEqual(0, numRows2);
            Assert.IsTrue(response1.First().CompanyName.Trim() == "BrainWare Company");
        }
    }
}
