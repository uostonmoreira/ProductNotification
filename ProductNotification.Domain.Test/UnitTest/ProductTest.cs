using Microsoft.Extensions.Logging;
using Moq;
using ProductNotification.Domain.Entities;
using Xunit;

namespace ProductNotification.Domain.Test.Entities
{
    public class ProductTest
    {
        private readonly Mock<ILogger<ProductTest>> _logger;

        public ProductTest()
        {
            this._logger = new Mock<ILogger<ProductTest>>();
        }

        [Fact(DisplayName = "Retorna TRUE")]
        public void Valido_DeveRetornaTrue()
        {
            // Arrange
            Product product = new Product
            {
                Codigo = 1,
                Name = "Batedeira",
                Price = 331,
                Situacao = Enumerations.SituacaoProduto.Aguardando
            };

            // Act
            bool result = product.Valido();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Retorna FALSE")]
        public void Valido_DeveRetornaFalse()
        {
            // Act
            bool result = new Product().Valido();

            // Assert
            Assert.False(result);
        }
    }
}
