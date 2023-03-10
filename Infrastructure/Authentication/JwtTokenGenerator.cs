using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Application.Interface.SPI;

using Domain;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJWTTokenGenerator
    {
        private readonly JwtSettings _configuration;
        private readonly IDateTimeService _dateTimeService;
        public JwtTokenGenerator(IOptions<JwtSettings> configuration, IDateTimeService dateTimeService)
        {
            _configuration = configuration.Value;
            _dateTimeService = dateTimeService;
        }

        public string CreateToken(LoginDTO user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration.Issuer,
                Audience = _configuration.Audience,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Email),
                    new Claim(ClaimTypes.Role, user.Role),
                    // new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName)
                }),
                Expires = _dateTimeService.UtcNow.AddMinutes(_configuration.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public JwtSecurityToken ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration.Issuer, // configuration[JwtSettings.SectionName + ":Issuer"],
                    ValidAudience = _configuration.Audience, // configuration[JwtSettings.SectionName + ":Audience"]
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret))
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}