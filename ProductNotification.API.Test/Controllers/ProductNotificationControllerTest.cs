using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductNotification.API.Controllers;
using ProductNotification.Domain.Entities;
using ProductNotification.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProductNotification.API.Test.Controllers
{
    public class ProductNotificationControllerTest
    {
        private readonly ProductNotificationController _productNotificationController;
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<ILogger<ProductNotificationController>> _logger;

        public ProductNotificationControllerTest()
        {
            this._logger = new Mock<ILogger<ProductNotificationController>>();
            this._productRepository = new Mock<IProductRepository>();

            this._productNotificationController = new ProductNotificationController(
                this._logger.Object,
                this._productRepository.Object
            );
        }

        [Fact(DisplayName = "Retorna status code 200")]
        public async Task GetAsync_DeveRetornaStatusCode200()
        {
            // Arrange
            IEnumerable<Product> products = new List<Product>
            {
                new Product { Codigo = 1, Name = "Fogão", Price = 1000, Situacao = Domain.Enumerations.SituacaoProduto.Disponivel },
                new Product { Codigo = 2, Name = "Geladeira", Price = 2400, Situacao = Domain.Enumerations.SituacaoProduto.Disponivel },
                new Product { Codigo = 3, Name = "Ventilador", Price = 330, Situacao = Domain.Enumerations.SituacaoProduto.Esgotado }
            };

            this._productRepository.Setup(m => m.GetAsync()).Returns(Task.FromResult(products));

            // Act
            IActionResult actionResult = await this._productNotificationController.GetAsync();
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is OkObjectResult);
            Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 404")]
        public async Task GetAsync_DeveRetornaStatusCode404()
        {
            // Act
            IActionResult actionResult = await this._productNotificationController.GetAsync();
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is NotFoundObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status404NotFound, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 500",
              Skip = "Teste ainda não disponível")]
        public void GetAsync_DeveRetornaStatusCode500()
        {

        }

        [Fact(DisplayName = "Retorna status code 200")]
        public async Task GetByIdAsync_DeveRetornaStatusCode200()
        {
            // Arrange
            var product = new Product
            {
                Codigo = 1,
                Name = "Liquidificador",
                Price = 132.00m,
                Situacao =
                Domain.Enumerations.SituacaoProduto.Disponivel
            };

            this._productRepository.Setup(m => m.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(product));

            // Act
            IActionResult actionResult = await this._productNotificationController.GetByIdAsync(1);
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is OkObjectResult);
            Assert.IsType<Product>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 404")]
        public async Task GetByIdAsync_DeveRetornaStatusCode404()
        {
            // Act
            IActionResult actionResult = await this._productNotificationController.GetByIdAsync(999);
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is NotFoundObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status404NotFound, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 400")]
        public async Task GetByIdAsync_DeveRetornaStatusCode400()
        {
            // Act
            IActionResult actionResult = await this._productNotificationController.GetByIdAsync(0);
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is BadRequestObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 500",
              Skip = "Teste ainda não disponível")]
        public void GetByIdAsync_DeveRetornaStatusCode500()
        {

        }

        [Fact(DisplayName = "Retorna status code 200")]
        public async Task PostAsync_DeveRetornaStatusCode200()
        {
            // Arrange
            var product = new Product { 
                Codigo = 1, 
                Name = "Liquidificador", 
                Price = 132.00m, Situacao = 
                Domain.Enumerations.SituacaoProduto.Disponivel 
            };

            this._productRepository.Setup(m => m.InsertAsync(product)).Returns(Task.FromResult(1));

            // Act
            IActionResult actionResult = await this._productNotificationController.PostAsync(product);
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is OkObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 400")]
        public async Task PostAsync_DeveRetornaStatusCode400()
        {
            // Act
            IActionResult actionResult = await this._productNotificationController.PostAsync(new Product());
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is BadRequestObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 502")]
        public async Task PostAsync_DeveRetornaStatusCode502()
        {
            // Arrange
            var product = new Product
            {
                Codigo = 1,
                Name = "Liquidificador",
                Price = 132.00m,
                Situacao =
                Domain.Enumerations.SituacaoProduto.Disponivel
            };

            this._productRepository.Setup(m => m.InsertAsync(product)).Returns(Task.FromResult(0));

            // Act
            IActionResult actionResult = await this._productNotificationController.PostAsync(product);
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is ObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status502BadGateway, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 500",
              Skip = "Teste ainda não disponível")]
        public void PostAsync_DeveRetornaStatusCode500()
        {

        }

        [Fact(DisplayName = "Retorna status code 200")]
        public async Task DeleteAsync_DeveRetornaStatusCode200()
        {
            // Arrange
            var product = new Product
            {
                Codigo = 1,
                Name = "Liquidificador",
                Price = 132.00m,
                Situacao =
                Domain.Enumerations.SituacaoProduto.Disponivel
            };

            this._productRepository.Setup(m => m.DeleteAsync(product)).Returns(Task.FromResult(1));

            // Act
            IActionResult actionResult = await this._productNotificationController.DeleteAsync(product);
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is OkObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 400")]
        public async Task DeleteAsync_DeveRetornaStatusCode400()
        {
            // Act
            IActionResult actionResult = await this._productNotificationController.DeleteAsync(new Product());
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is BadRequestObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 502")]
        public async Task DeleteAsync_DeveRetornaStatusCode502()
        {
            // Arrange
            var product = new Product
            {
                Codigo = 1,
                Name = "Liquidificador",
                Price = 132.00m,
                Situacao =
                Domain.Enumerations.SituacaoProduto.Disponivel
            };

            this._productRepository.Setup(m => m.DeleteAsync(product)).Returns(Task.FromResult(0));

            // Act
            IActionResult actionResult = await this._productNotificationController.DeleteAsync(product);
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is ObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status502BadGateway, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 500",
              Skip = "Teste ainda não disponível")]
        public void DeleteAsync_DeveRetornaStatusCode500()
        {

        }

        [Fact(DisplayName = "Retorna status code 200")]
        public async Task PutAsync_DeveRetornaStatusCode200()
        {
            // Arrange
            var product = new Product
            {
                Codigo = 1,
                Name = "Liquidificador",
                Price = 132.00m,
                Situacao =
                Domain.Enumerations.SituacaoProduto.Disponivel
            };

            this._productRepository.Setup(m => m.UpdateAsync(product)).Returns(Task.FromResult(1));

            // Act
            IActionResult actionResult = await this._productNotificationController.PutAsync(product);
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is OkObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 400")]
        public async Task PutAsync_DeveRetornaStatusCode400()
        {
            // Act
            IActionResult actionResult = await this._productNotificationController.PutAsync(new Product());
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is BadRequestObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 502")]
        public async Task PutAsync_DeveRetornaStatusCode502()
        {
            // Arrange
            var product = new Product
            {
                Codigo = 1,
                Name = "Liquidificador",
                Price = 132.00m,
                Situacao =
                Domain.Enumerations.SituacaoProduto.Disponivel
            };

            this._productRepository.Setup(m => m.UpdateAsync(product)).Returns(Task.FromResult(0));

            // Act
            IActionResult actionResult = await this._productNotificationController.PutAsync(product);
            var okResult = actionResult as ObjectResult;

            // Assert
            Assert.True(okResult is ObjectResult);
            Assert.IsType<string>(okResult.Value);
            Assert.Equal(StatusCodes.Status502BadGateway, okResult.StatusCode);
        }

        [Fact(DisplayName = "Retorna status code 500",
              Skip = "Teste ainda não disponível")]
        public void PutAsync_DeveRetornaStatusCode500()
        {

        }
    }
}