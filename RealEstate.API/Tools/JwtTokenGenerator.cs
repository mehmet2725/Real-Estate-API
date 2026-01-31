using Microsoft.IdentityModel.Tokens;
using RealEstate.Entity.Concrete;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace RealEstate.API.Tools;

public class JwtTokenGenerator
{
    // Token üretici metodumuz
    public static string GenerateToken(AppUser user, string role, IConfiguration configuration)
    {
        // Tokenin içne gömeceğimiz bilgiler (claims)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Role, role)
        };

        // appsettingsjsondan anahtarı al
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]!));

        // Şifreleme algoritmasını seçelim
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // tokenın geçerlilik süresini ayarla
        var expireMinutes = int.Parse(configuration["JwtSettings:ExpirationMinutes"]!);
        var expireDate = DateTime.UtcNow.AddMinutes(expireMinutes);

        // Token'i oluştur
        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expireDate,
            signingCredentials: creds
        );

        // string olarak geri döndür
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
