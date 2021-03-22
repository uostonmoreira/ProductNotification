using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductNotification.Domain.Interfaces.Repository;
using ProductNotification.Domain.Interfaces.Services;
using System;
using System.Threading.Tasks;

namespace ProductNotification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthController(ILogger<AuthController> logger, ITokenService tokenService, IUserRepository userRepository)
        {
            this._logger = logger;
            this._tokenService = tokenService;
            this._userRepository = userRepository;
        }

        [HttpGet("{user}/{password}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync(string user, string password)
        {
            _logger.LogDebug($"Recebendo requisição: {user} em: {DateTime.Now}");

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                return BadRequest("Parâmetros de entrada não informados");

            _logger.LogDebug($"Iniciando persistência de dados em: {DateTime.Now}");
            var result = await this._userRepository.GetByUserPassAsync(user, password);
            _logger.LogDebug($"Finalizando persistência de dados em: {DateTime.Now}");

            if (result == null)
                return NotFound("Nenhum registro encontrado");

            result.Password = string.Empty;

            return Ok(_tokenService.GenerateToken(result));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles = "adm,master")]
        [AllowAnonymous]
        public async Task<IActionResult> PostAsync([FromQuery] Domain.Entities.User user)
        {
            _logger.LogDebug($"Recebendo requisição: {user} em: {DateTime.Now}");

            if (!user.Valido())
                return BadRequest("Parâmetros de entrada não informados");

            _logger.LogDebug($"Iniciando persistência de dados em: {DateTime.Now}");
            var result = await this._userRepository.InsertAsync(user);
            _logger.LogDebug($"Finalizando persistência de dados em: {DateTime.Now}");

            if (result == 0)
                return StatusCode(502, "Ocorreu um erro ao tentar processar sua requisição");

            return Ok("Usuário cadastrado com sucesso!!!");
        }
    }
}