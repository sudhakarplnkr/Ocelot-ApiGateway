namespace AuthenticationApi.Controllers
{
    using AuthenticationApi.Model;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IOptions<Audience> settings;

        public LoginController(IOptions<Audience> settings)
        {
            this.settings = settings;
        }

        // POST api/authentication
        [HttpPost, Route("")]
        public User Post([FromBody] Login login)
        {
            var userClaim = GenerateToken(login.Username);

            return userClaim;
        }

        private User GenerateToken(string username)
        {
            var now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.Value.Secret));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = settings.Value.Iss,
                ValidateAudience = true,
                ValidAudience = settings.Value.Aud,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,

            };

            var jwt = new JwtSecurityToken(
                issuer: settings.Value.Iss,
                audience: settings.Value.Aud,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responseJson = new User
            {
                Token = encodedJwt,
                ExpiresIn = (int)TimeSpan.FromMinutes(2).TotalSeconds,
                Name = username,
                Email = $"{username}@abc.de"
            };

            return responseJson;
        }
    }
}