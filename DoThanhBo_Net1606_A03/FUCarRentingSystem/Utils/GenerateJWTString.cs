using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using BusinessObjects.Models;

namespace FUCarRentingSystem.Utils
{
    public static class GenerateJWTString
    {
        public static string GenerateJsonWebToken(this Customer customer, string secretKey, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(ClaimTypes.SerialNumber ,customer.CustomerId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, customer.CustomerName),
            new Claim(ClaimTypes.Role, "Customer"),
        };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: now.AddHours(1),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string GenerateJsonWebTokenForAdmin(this string email, string secretKey, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(ClaimTypes.Email ,email),
            new Claim(ClaimTypes.Role, "Admin"),
        };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: now.AddHours(1),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
