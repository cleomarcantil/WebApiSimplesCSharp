using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApiSimplesCSharp.Constants.LogEvents;
using WebApiSimplesCSharp.Exceptions;
using WebApiSimplesCSharp.Models.Auth;
using WebApiSimplesCSharp.Services.Auth;
using WebApiSimplesCSharp.Services.Usuarios;
using WebApiSimplesCSharp.Settings;

namespace WebApiSimplesCSharp.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IConsultaUsuarioService consultaUsuarioService;
		private readonly IAuthService authService;
		private readonly IOptions<TokenSettings> tokenSettingsOptions;
		private readonly ILogger<AuthController> logger;

		public AuthController(
			IConsultaUsuarioService consultaUsuarioService,
			IAuthService authService,
			IOptions<TokenSettings> tokenSettingsOptions,
			ILogger<AuthController> logger)
		{
			this.consultaUsuarioService = consultaUsuarioService;
			this.authService = authService;
			this.tokenSettingsOptions = tokenSettingsOptions;
			this.logger = logger;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public TokenInfoViewModel Login([FromBody] CredenciaisInputModel credenciais)
		{
			try {
				logger.LogInformation(AcessoLogEvents.Autenticando, "Autenticando '{Login}'", credenciais.Login);

				var usuario = consultaUsuarioService.GetByLogin(credenciais.Login)
					?? throw new CredenciaisInvalidasException($"Usuário não encontrado: '{credenciais.Login}'!");

				if (!usuario.CheckSenha(credenciais.Senha)) {
					throw new CredenciaisInvalidasException("Credenciais inválidas!");
				}

				var tokenSettings = tokenSettingsOptions.Value;
				var expiracaoToken = DateTime.UtcNow.AddMinutes(tokenSettings.Expires);
				var token = authService.GenerateToken(usuario.Id, expiracaoToken, tokenSettings.Key, tokenSettings.Issuer, tokenSettings.Audience);

				logger.LogInformation(AcessoLogEvents.Autenticado, "'{Login}' authenticado", credenciais.Login);

				return new TokenInfoViewModel
				{
					Token = token,
					Expires = expiracaoToken,
				};
			} catch (Exception ex) {
				logger.LogError(AcessoLogEvents.Erro, ex, "Erro de autenticação para '{Login}'", credenciais.Login);
				throw;
			}
		}


		[HttpPost("refresh-token")]
		public ActionResult<TokenInfoViewModel> RefreshToken()
		{
			var usuarioId = authService.GetCurrentUserId();

			if (usuarioId is null) {
				return Unauthorized();
			}

			logger.LogInformation(AcessoLogEvents.TokenAtualizado, "Atualizando token para '{UsuarioId}')", usuarioId);

			var tokenSettings = tokenSettingsOptions.Value;
			var expiracaoToken = DateTime.UtcNow.AddMinutes(tokenSettings.Expires);
			var token = authService.GenerateToken(usuarioId.Value, expiracaoToken, tokenSettings.Key, tokenSettings.Issuer, tokenSettings.Audience);

			return new TokenInfoViewModel
			{
				Token = token,
				Expires = expiracaoToken,
			};
		}

		[HttpGet("user")]
		public ActionResult<AuthUserViewModel> GetAuthUser()
		{
			var usuarioAtualId = authService.GetCurrentUserId();

			if (usuarioAtualId is null) {
				return Unauthorized();
			}

			var usuarioAtual = consultaUsuarioService.GetById(usuarioAtualId.Value)
				?? throw new Exception($"Erro obtendo usuário '{usuarioAtualId}'!");

			return new AuthUserViewModel
			{
				Id = usuarioAtual.Id,
				Nome = usuarioAtual.Nome,
				Login = usuarioAtual.Login,
			};
		}

	}

}
