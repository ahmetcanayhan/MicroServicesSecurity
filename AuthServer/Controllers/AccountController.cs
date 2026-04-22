using AuthServer.Models;
using Duende.IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly HttpClient client;
        IConfiguration configuration;

        public AccountController(UserManager<IdentityUser> userManager, IHttpClientFactory factory, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.client = factory.CreateClient();
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto model)
        {
            // IdentityServer (AuthServer) yapısına lokal olarak istek atacak.
            var discovery = await client.GetDiscoveryDocumentAsync(configuration.GetValue<string>("AuthServerUrl"));
            if (discovery.IsError)
            
                return StatusCode(500, discovery.Error);

                var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = discovery.TokenEndpoint,
                    ClientId = "sso_app",
                    ClientSecret = "sso_app_secret_key",
                    UserName = model.UserName,
                    Password = model.Password,
                    Scope = "app.read app.write openid profile"
                });

                if (tokenResponse.IsError)
                    return Unauthorized(new { error = tokenResponse.ErrorDescription ?? "Login attempt failed!" });

                    return Ok(new {access_token = tokenResponse.AccessToken, expires_in = tokenResponse.ExpiresIn });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
        {
            var user = new IdentityUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true // test ortamı için direkt onaylı
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok(new { message = "Registiration succeeded." });
            }
            return BadRequest(new
            {
                message = "Register attempt failed!",
                errors = result.Errors.Select(e => e.Description)
            });
        }

        [HttpPost("connect")]
        public async Task<IActionResult> ConnectAsync([FromBody] ConnectDto model)
        {
            var discovery = await client.GetDiscoveryDocumentAsync(configuration.GetValue<string>("AuthServerUrl"));
            if (discovery.IsError)

                return StatusCode(500, discovery.Error);

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = model.ClientId,
                ClientSecret = model.ClientSecret,
                Scope = "all.backup"
            });

            if (tokenResponse.IsError)
                return Unauthorized(new { error = tokenResponse.ErrorDescription ?? "Connect attempt failed!" });

            return Ok(new { access_token = tokenResponse.AccessToken, expires_in = tokenResponse.ExpiresIn });
        }
    }
}
