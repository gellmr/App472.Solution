using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using App472.Domain.Abstract;
using App472.Domain.Entities;
using App472.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace App472.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Products(){
            // Arrange - create the mock repository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ProductID = 1, Name = "p1"},
                new Product{ProductID = 2, Name = "p2"},
                new Product{ProductID = 3, Name = "p3"},
            });
            // Arrange - create a controller
            AdminController target = new AdminController(mock.Object, null);
            // Action
            Product[] result = ((IEnumerable<Product>)target.Index(null).ViewData.Model).ToArray();
            // Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("p1", result[0].Name);
            Assert.AreEqual("p2", result[1].Name);
            Assert.AreEqual("p3", result[2].Name);
        }

        [TestMethod]
        public void Can_Edit_Product()
        {
            // Arrange - create the mock repository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ ProductID = 1, Name = "p1"},
                new Product{ ProductID = 2, Name = "p2"},
                new Product{ ProductID = 3, Name = "p3"}
            });
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object, null);
            // Act
            Product p1 = target.Edit(1, null).ViewData.Model as Product;
            Product p2 = target.Edit(2, null).ViewData.Model as Product;
            Product p3 = target.Edit(3, null).ViewData.Model as Product;
            // Assert
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // Arrange - create the mock repository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ ProductID = 1, Name = "p1"},
                new Product{ ProductID = 2, Name = "p2"},
                new Product{ ProductID = 3, Name = "p3"}
            });
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object, null);
            // Act
            Product result = (Product)target.Edit(4, null).ViewData.Model;
            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            // Arrange - create a product
            Product prod = new Product{ ProductID = 2, Name = "Test" };
            // Arrange - create the mock respository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID = 1, Name = "p1"},
                prod,
                new Product{ ProductID = 3, Name = "p3"}
            });
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object, null);
            // Act - delete the product
            target.Delete(prod.ProductID);
            // Assert - ensure that the repository delete method was called with the correct Product ID
            mock.Verify(m => m.DeleteProduct(prod.ProductID));
        }
    }
}
