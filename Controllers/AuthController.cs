using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GCP_Pratice.Models;

namespace GCP_Pratice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="loginModel">User credentials.</param>
        /// <returns>JWT token if authentication is successful.</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            // For demonstration purposes, we're using hardcoded credentials.
            // In a real application, you would validate against a database with hashed passwords.
            if (loginModel.Username == "admin" && loginModel.Password == "SuperSecurePassword123!") // Use a stronger, different password
            {
                var jwtSigningKey = Environment.GetEnvironmentVariable("JWT_SIGNING_KEY") ?? _configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(jwtSigningKey))
                {
                    return StatusCode(500, "JWT signing key is not configured.");
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, loginModel.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    // Add other claims like roles here
                };

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30), // Token valid for 30 minutes
                    signingCredentials: credentials);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized("Invalid username or password.");
        }
    }
}