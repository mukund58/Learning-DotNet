using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace _20_curd_mastery.Auth;

public class TokenService
{
    private readonly string _key;

    public TokenService(string key)
    {
        _key = key;
    }
    // public string Generate(string username, string role)
    public string Generate(string username)
    {
        // var claims = new[]
        // {
        //     new Claim(ClaimTypes.Name, username),
        //     new Claim(ClaimTypes.Role, role)
        // };

        return new JwtSecurityTokenHandler().WriteToken(
            new JwtSecurityToken(
                claims: new[] { new Claim(ClaimTypes.Name, username) },
                // claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                    SecurityAlgorithms.HmacSha256)
            ));
    }
}
