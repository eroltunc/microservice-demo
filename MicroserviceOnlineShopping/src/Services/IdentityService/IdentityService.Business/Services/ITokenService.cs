using IdentityService.Core.Results;
using IdentityService.Entity.Models.ResponseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace IdentityService.Business.Services
{
    public interface ITokenService
    {
        IDataResult<TokenResponseModel> GetTokenAsync(string email, string name, string id);
    }
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        private readonly IConfiguration _configuration = configuration;
        public IDataResult<TokenResponseModel> GetTokenAsync(string email,string name,string id)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.NameIdentifier,id)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthenticationConfiguration:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(10);

            var token = new JwtSecurityToken(claims: claims, expires: expiry, signingCredentials: creds, notBefore: DateTime.Now);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            byte[] numbers = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(numbers);
            var refreshToken = Convert.ToBase64String(numbers);

            return new SuccessDataResult<TokenResponseModel>(new TokenResponseModel()
            {
                AccessToken = encodedJwt,
                RefreshToken = refreshToken,
                ExpiresIn = expiry,
                TokenType = "JWT"
            });
        }

       
    }
}
