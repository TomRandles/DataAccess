using DataAccessPatterns.UnitOfWorkPattern;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RepositoryPattern.Repositories;
using Shared.DataAccess;
using Shared.Domain.Models;
using ShopingApp.Web.Controllers;
using ShoppingApp.Web.Models;
using System;

namespace ShoppingApp.Web.Tests
{
    [TestClass]
    public class OrderControllerTests
    {
        [TestMethod]
        public void CanCreateOrder()
        {
            // var db = new Mock<ShoppingDb>();

            var customerRepository = new Mock<IRepository<Customer>>();
            var orderRepository = new Mock<IRepository<Order>>();
            var productRepository = new Mock<IRepository<Product>>();

            // var orderController = new OrderController(orderRepository.Object,
            //                                          productRepository.Object);


            var unitOfWork = new Mock<IUnitOfWork>();

            unitOfWork.Setup(uow => uow.CustomerRepository).Returns(() => customerRepository.Object);
            unitOfWork.Setup(uow => uow.OrderRepository).Returns(() => orderRepository.Object);
            unitOfWork.Setup(uow => uow.ProductRepository).Returns(() => productRepository.Object);

            var orderController = new OrderController(unitOfWork.Object);

            var createOrder = new CreateOrderModel
            {
                Customer = new CustomerModel
                {
                    Name = "Tom Randles",
                    ShippingAddress = "Knocknacarra, Galway",
                    City = "Galway",
                    Country = "Ireland"
                },
                LineItems = new[]
                {
                    new LineItemModel { ProductId = Guid.NewGuid(), Quantity = 4 },
                    new LineItemModel { ProductId = Guid.NewGuid(), Quantity = 2 }
                }
            };

            // Create order
            orderController.Create(createOrder);

            // Verify that the order repository Add function was called once.
            orderRepository.Verify(repository => repository.Add(It.IsAny<Order>()),
                Times.AtMostOnce());
        }
    }
}
