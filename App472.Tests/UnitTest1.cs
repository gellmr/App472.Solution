using App472.Domain.Abstract;
using App472.Domain.Entities;
using App472.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App472.WebUI.Models;
using App472.WebUI.HtmlHelpers;
using System.Web.Mvc;

namespace App472.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Arrange
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ProductID = 1, Name = "P1"},
                new Product{ProductID = 2, Name = "P2"},
                new Product{ProductID = 3, Name = "P3"},
                new Product{ProductID = 4, Name = "P4"},
                new Product{ProductID = 5, Name = "P5"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            // Act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;
            // Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange - define an HTML helper - we need to do this
            // in order to apply the extension method
            HtmlHelper myHelper = null;
            // Arrange - create PagingInfo data
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10,
            };
            // Arrange - set up the delegate using a lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            // Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Assert
            Assert.AreEqual(
                  @"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString()
            );
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 2, Name = "P2" },
                new Product { ProductID = 3, Name = "P3" },
                new Product { ProductID = 4, Name = "P4" },
                new Product { ProductID = 5, Name = "P5" }
            });
            // Arrange
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            // Act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;
            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // Arrange
            // create the mock repository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product { ProductID = 1, Name = "p1", Category="cat1"},
                new Product { ProductID = 2, Name = "p2",    Category="cat2"},
                new Product { ProductID = 3, Name = "p3", Category="cat1"},
                new Product { ProductID = 4, Name = "p4",    Category="cat2"},
                new Product { ProductID = 5, Name = "p5",       Category="cat3"}
            });
            // create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            // Action
            Product[] result = ((ProductsListViewModel)controller.List("cat2",1).Model).Products.ToArray();
            // Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "p2" && result[0].Category == "cat2");
            Assert.IsTrue(result[1].Name == "p4" && result[1].Category == "cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Arrange
            // create the mock repository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "p1", Category = "apple"},
                new Product { ProductID = 2, Name = "p2", Category = "apple"},
                new Product { ProductID = 3, Name = "p3", Category =   "plum"},
                new Product { ProductID = 4, Name = "p4", Category =     "orange"}
            });
            // Arrange - create the controller
            NavController target = new NavController(mock.Object);
            // Act - get the set of categories
            string[] results = ((IEnumerable<string>)target.CategoryMenu().Model).ToArray();
            // Assert
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "apple");
            Assert.AreEqual(results[1], "orange");
            Assert.AreEqual(results[2], "plum");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Arrange
            // create mock repository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product{ProductID = 1, Name = "p1", Category = "apple" },
                new Product{ProductID = 2, Name = "p2", Category = "orange" }
            });
            // Arrange - create the controller
            NavController target = new NavController(mock.Object);
            // Arrange - define the category to select
            string categoryToSelect = "apple";
            // Action
            string result = target.CategoryMenu(categoryToSelect).ViewBag.SelectedCategory;
            // Assert
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            //arrange
            // create mock repository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product { ProductID = 1, Name = "p1", Category = "cat1" },
                new Product { ProductID = 2, Name = "p2", Category = "cat2" },
                new Product { ProductID = 3, Name = "p3", Category = "cat1" },
                new Product { ProductID = 4, Name = "p4", Category = "cat2" },
                new Product { ProductID = 5, Name = "p5", Category = "cat3" }
            });
            // arrange - create a controller and make the page size 3 items
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            // action - test the product counts for differnt categories
            int result_1 = ((ProductsListViewModel)target.List("cat1").Model).PagingInfo.TotalItems;
            int result_2 = ((ProductsListViewModel)target.List("cat2").Model).PagingInfo.TotalItems;
            int result_3 = ((ProductsListViewModel)target.List("cat3").Model).PagingInfo.TotalItems;
            int result_all = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;
            // assert
            Assert.AreEqual(result_1, 2);
            Assert.AreEqual(result_2, 2);
            Assert.AreEqual(result_3,   1);
            Assert.AreEqual(result_all, 5);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Arrange - create mock repository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product{ ProductID = 1, Name = "p1", Category = "apple"}
            }.AsQueryable());
            // Arrange - create a Cart
            Cart cart = new Cart();
            // Arrange - create the controller
            CartController target = new CartController(mock.Object, null, null, null);
            // Act - add a product to the cart
            target.AddToCart(cart, 1, null);
            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            // Arrange - create the mock repository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
                new Product{ ProductID = 1, Name = "p1", Category = "apples"}
            }.AsQueryable());
            // Arrange - create a Cart
            Cart cart = new Cart();
            // Arrange - create the controller
            CartController target = new CartController(mock.Object, null, null, null);
            // Act - add a product to the cart
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");
            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Arrange - create a Cart
            Cart cart = new Cart();
            // Arrange - create the controller
            CartController target = new CartController(null, null, null, null);
            // Act - call the Index action method
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;
            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create an empty cart
            Cart cart = new Cart();
            // Arrange - create shipping details
            ShippingDetails shippingDetails = new ShippingDetails();
            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object, null, null);
            // Act
            ViewResult result = target.Checkout(cart, shippingDetails);
            // Assert - check that ProcessOrder(cart, shippingDetails) was never called.
            mock.Verify(m =>
                m.ProcessOrder( It.IsAny<Cart>(), It.IsAny<ShippingDetails>() ),
                Times.Never()
            );
            // Assert - check that the ViewResult has no name. (Is returning the default view)
            // This means it is redisplaying the data entered by the customer so they can correct errors.
            Assert.AreEqual("", result.ViewName);
            // Assert - check that the ModelState being passed to the view is invalid.
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Arrange - create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object, null, null);
            // Arrange - add an error to the model
            target.ModelState.AddModelError("error", "error");
            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());
            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(
                m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never()
            );
            // Assert - check that the method is returning the default view
            Assert.AreEqual("", result.ViewName);
            // Assert - check that I am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Save_Valid_Changes()
        {
            // Arrange - create mock repository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object, null);
            // Arrange - create a product
            Product product = new Product{Name = "Test"};
            // Act - try to save the product
            ActionResult result = target.Edit(product.ProductID, null);
            // Assert - check that the repository was called
            mock.Verify(m => m.SaveProduct(product));
            // Assert - check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange - create mock repository
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            // Arrange - create the controller
            AdminController target = new AdminController(mock.Object, null);
            // Arrange - create a product
            Product product = new Product {Name = "Test"};
            // Arrange - add an error to the model state
            target.ModelState.AddModelError("error", "error");
            // Act - try to save the product
            ActionResult result = target.Edit(product.ProductID, null);
            // Assert - check that teh repository was not called
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            // Assert - check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
