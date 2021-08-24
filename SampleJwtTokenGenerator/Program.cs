using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SampleJwtTokenGenerator
{
    class Program
    {
        public static void Main(string[] args)
        {
            var userId = "admin@admin.com";
            Console.WriteLine(GenerateJwtToken(
                new List<Claim> {
                new Claim(ClaimTypes.Upn, userId),
                new Claim(ClaimTypes.Email, userId),
                new Claim(ClaimTypes.Name, userId),
                new Claim(ClaimTypes.Role, "Admin"),
                })); ;
            Console.Read();
        }

        public static string GenerateJwtToken(List<Claim> claims)
        {
            var issuer = "SampleJWTTokenGenerator";
            var audience = "AuthSessionDemoWebAPIWithCustomJwt";
            var expiryMinutes = 1440;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("someSecretSaltForSigning"));

            if (claims == null)
                return null;

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expiryMinutes > 0 ? DateTime.Now.AddMinutes(expiryMinutes) : default,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
