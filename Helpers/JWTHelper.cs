using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UrlShortener.Helpers;

public class JWTHelper
{
    private readonly byte[] secret;

    public JWTHelper(string secretKey)
    {
        secret = Encoding.ASCII.GetBytes(secretKey);
    }

    public string CreateToken(string email)
    {
        var claims = new ClaimsIdentity();

        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, email));

        var tokenDescription = new SecurityTokenDescriptor()
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(this.secret), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var createdToken = tokenHandler.CreateToken(tokenDescription);

        return tokenHandler.WriteToken(createdToken);
    }
}
