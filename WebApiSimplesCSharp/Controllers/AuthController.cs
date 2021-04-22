using System;
using System.Threading.Tasks;
using HelpersExtensions.JwtAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApiSimplesCSharp.Constants.LogEvents;
using WebApiSimplesCSharp.Exceptions;
using WebApiSimplesCSharp.Models;
using WebApiSimplesCSharp.Models.Auth;
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
		private readonly IAuthService<AuthUserInfo> authService;
		private readonly IOptions<TokenSettings> tokenSettingsOptions;
		private readonly ILogger<AuthController> logger;

		public AuthController(
			IConsultaUsuarioService consultaUsuarioService,
			IAuthService<AuthUserInfo> authService,
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
				var authUserInfo = new AuthUserInfo { Id = usuario.Id, Nome = usuario.Nome, Login = usuario.Login };
				var token = authService.GenerateToken(authUserInfo, expiracaoToken);

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
			var authUserInfo = authService.GetAuthUserData();

			if (authUserInfo is null) {
				return Unauthorized();
			}

			logger.LogInformation(AcessoLogEvents.TokenAtualizado, "Atualizando token para '{UsuarioId}')", authUserInfo?.Id);

			var tokenSettings = tokenSettingsOptions.Value;
			var expiracaoToken = DateTime.UtcNow.AddMinutes(tokenSettings.Expires);
			var token = authService.GenerateToken(authUserInfo!, expiracaoToken);

			return new TokenInfoViewModel
			{
				Token = token,
				Expires = expiracaoToken,
			};
		}

		[HttpGet("user")]
		public ActionResult<AuthUserViewModel> GetAuthUser()
		{
			var authUserInfo = authService.GetAuthUserData();

			if (authUserInfo is null) {
				return Unauthorized();
			}

			return new AuthUserViewModel
			{
				Id = authUserInfo.Id,
				Nome = authUserInfo.Nome,
				Login = authUserInfo.Login,
			};
		}


		[HttpPost("change-password")]
		public async Task<ActionResult> ChangePassword(ChangePasswordInputModel changePassword, [FromServices] IManutencaoUsuarioService manutencaoUsuarioService)
		{
			var authUserInfo = authService.GetAuthUserData();

			if (authUserInfo is null) {
				return Unauthorized();
			}

			var usuarioAtual = consultaUsuarioService.GetById(authUserInfo.Id)
				?? throw new Exception($"Erro obtendo usuário '{authUserInfo.Id}'!");

			if (!usuarioAtual.CheckSenha(changePassword.SenhaAtual)) {
				throw new CredenciaisInvalidasException("Senha atual inválida!");
			}

			await manutencaoUsuarioService.AlterarSenha(authUserInfo.Id, changePassword.NovaSenha);

			return NoContent();
		}

	}
}
