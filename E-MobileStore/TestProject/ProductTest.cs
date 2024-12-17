using Microsoft.Extensions.Logging;
using Moq;
using Store.ApiService.Services.Interfaces;
using Store.API.Controllers;
using Store.Infrastructure.DTOs;
using Store.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Store.API;

namespace TestProject
{
    [TestFixture]
    public class ProductTets
    {
        private Mock<IProductService> _mockProductService;
        private Mock<IRedisService> _mockRedisService;
        private ProductsController _controller;
        private Mock<ILogger<ProductsController>> _mockLogger;
        [SetUp]
        public void Setup()
        {
            _mockProductService = new Mock<IProductService>();
            _mockRedisService = new Mock<IRedisService>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController
                (
                _mockLogger.Object,
                _mockProductService.Object,
                _mockRedisService.Object
                );

        }

        [Test]
        public async Task AddOrUpdateProduct_ShouldReturnOK_WhenProductIsAddOrUpdate()
        {
            var product = new ProductDTO
            {
                CategoryId = 1,
                CreatedBy = "admin",
                CreatedDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdatedBy = "admin",
                Description = "testProduct",
                Id = Guid.NewGuid().ToString(),
                IsActive = true,
                IsDeleted = true,
                Name = "TestProduct",
                Price = 100,
                PriceSale = 0,
                Quantity = 0,
                ShortDesc = "test"
            };
            _mockProductService.Setup(service => service.AddOrUpdateProduct(It.IsAny<ProductDTO>())).ReturnsAsync("Product added or updated successfully");
            _mockRedisService.Setup(redis => redis.RemovePatternAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
            var result = await _controller.AddProduct(product);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual((int)HttpStatusCode.OK, okResult.StatusCode);
            var response = okResult.Value as BaseApiResponse;
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual("Product added or updated successfully", response.Message);
            _mockRedisService.Verify(redis => redis.RemovePatternAsync("productCate:*"), Times.Once);
            Assert.Pass();
        }
        [Test]
        public async Task AddOrUpdateProduct_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var product = new ProductDTO
            {
                CategoryId = 1,
                CreatedBy = "admin",
                CreatedDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                UpdatedBy = "admin",
                Description = "testProduct",
                Id = Guid.NewGuid().ToString(),
                IsActive = true,
                IsDeleted = true,
                Name = "TestProduct",
                Price = 100,
                PriceSale = 0,
                Quantity = 0,
                ShortDesc = "test"
            };
            _mockProductService.Setup(service => service.AddOrUpdateProduct(It.IsAny<ProductDTO>()))
                .ThrowsAsync(new Exception("An error occurred"));
            var result = await _controller.AddProduct(product);
            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            var response = badRequestResult.Value as BaseApiResponse;
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual("An error occurred", response.ErrorMessages[0]);
            _mockRedisService.Verify(redis => redis.RemovePatternAsync(It.IsAny<string>()), Times.Never);
        }
        [Test]
        public async Task DeleteProduct_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var productUrl = "test-product";

            _mockProductService.Setup(service => service.DeleteProduct(It.IsAny<string>()))
                .Throws(new Exception("An error occurred"));
            var result = await _controller.DeleteProduct(productUrl);
            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            var response = badRequestResult.Value as BaseApiResponse;
            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual("An error occurred", response.ErrorMessages[0]);
            _mockRedisService.Verify(redis => redis.RemovePatternAsync(It.IsAny<string>()), Times.Never);
            _mockProductService.Verify(service => service.DeleteProduct(It.IsAny<string>()), Times.Once);
        }
        [Test]
        public async Task DeleteProduct_ShouldReturnOk_WhenDeleteSuccess()
        {
            // Arrange
            var productUrl = "test-product";
            _mockRedisService
               .Setup(r => r.RemovePatternAsync("productCate:*"))
               .Returns(Task.CompletedTask);
            var result = await _controller.DeleteProduct(productUrl);
            // Assert
            var okRequestResult = result as OkObjectResult;
            var response = okRequestResult.Value as BaseApiResponse;
            Assert.IsNotNull(okRequestResult);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.IsSuccess);
            _mockRedisService.Verify(redis => redis.RemovePatternAsync(It.IsAny<string>()), Times.Once);
            _mockProductService.Verify(redis => redis.DeleteProduct(It.IsAny<string>()), Times.Once);
        }
    }
}