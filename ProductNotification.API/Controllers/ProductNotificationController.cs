using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductNotification.Application.Interfaces;
using ProductNotification.Domain.Entities;
using ProductNotification.Domain.Interfaces.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductNotification.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductNotificationController : ControllerBase
    {
        private readonly ILogger<ProductNotificationController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IProductApplication _productApplication;

        public ProductNotificationController(
                                                ILogger<ProductNotificationController> logger,
                                                IProductRepository productRepository,
                                                IProductApplication productApplication
                                            )
        {
            this._logger = logger;
            this._productRepository = productRepository;
            this._productApplication = productApplication;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> GetAsync()
        {
            _logger.LogDebug($"Iniciando requisição de busca em: {DateTime.Now}");
            var result = (await _productRepository.GetAsync()).ToList();
            _logger.LogDebug($"Finalizando requisição de busca em: {DateTime.Now}");

            if (result.Count() == 0)
                return NotFound("Nenhum registro encontrado");

            return Ok(result);
        }

        [HttpGet("{codigo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> GetByIdAsync(int codigo)
        {
            if (codigo == 0)
                return BadRequest("Parâmetros de entrada não informado");

            _logger.LogDebug($"Iniciando requisição de busca em: {DateTime.Now}");
            var result = await _productRepository.GetByIdAsync(codigo);
            _logger.LogDebug($"Finalizando requisição de busca em: {DateTime.Now}");

            if (result == null)
                return NotFound("Nenhum registro encontrado");

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "adm,master")]
        public async Task<IActionResult> PostAsync([FromQuery] Product product)
        {
            _logger.LogDebug($"Recebendo requisição: {product} em: {DateTime.Now}");

            if (!product.Valido())
                return BadRequest("Parâmetros de entrada não informados");

            _logger.LogDebug($"Iniciando persistência de dados em: {DateTime.Now}");
            var result = await this._productApplication.InsertAsync(product);
            _logger.LogDebug($"Finalizando persistência de dados em: {DateTime.Now}");

            if (result == 0)
                return StatusCode(502, "Ocorreu um erro ao tentar processar sua requisição");

            return Ok("Produto cadastrado com sucesso!!!");
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "adm,master")]
        public async Task<IActionResult> DeleteAsync(Product product)
        {
            _logger.LogDebug($"Recebendo requisição: {product} em: {DateTime.Now}");

            if (!product.Valido())
                return BadRequest("Parâmetros de entrada não informados");

            _logger.LogDebug($"Iniciando persistência de dados em: {DateTime.Now}");
            var result = await _productRepository.DeleteAsync(product);
            _logger.LogDebug($"Finalizando persistência de dados em: {DateTime.Now}");

            if (result == 0)
                return StatusCode(502, "Ocorreu um erro ao tentar processar sua requisição");

            return Ok("Produto excluído com sucesso!!!");
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "adm,master")]
        public async Task<IActionResult> PutAsync(Product product)
        {
            _logger.LogDebug($"Recebendo requisição: {product} em: {DateTime.Now}");

            if (!product.Valido())
                return BadRequest("Parâmetros de entrada não informados");

            _logger.LogDebug($"Iniciando persistência de dados em: {DateTime.Now}");
            var result = await _productRepository.UpdateAsync(product);
            _logger.LogDebug($"Finalizando persistência de dados em: {DateTime.Now}");

            if (result == 0)
                return StatusCode(502, "Ocorreu um erro ao tentar processar sua requisição");

            return Ok("Produto atualizado com sucesso!!!");
        }
    }
}