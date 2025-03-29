using System.Text;

namespace UrlShortener.Helpers;

public class HashHelper
{
    private string GenerateSalt()
    {
        var data = new byte[128 / 8];

        using (var generator = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            generator.GetBytes(data);
            return Convert.ToBase64String(data);
        }
    }

    public string HashPassword(string password)
    {
        var salt = GenerateSalt();

        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var saltedPassword = password + salt;
            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashedBytes = sha256.ComputeHash(saltedPasswordBytes);

            var passwordHash = Convert.ToBase64String(hashedBytes);

            return string.Join(";", salt, passwordHash);
        }
    }

    public bool VerifyPassword(string password, string hash)
    {
        var parts = hash.Split(';');
        var salt = parts[0];
        var passwordHash = parts[1];

        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var saltedPassword = password + salt;
            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashedBytes = sha256.ComputeHash(saltedPasswordBytes);
            var passwordHashToValidate = Convert.ToBase64String(hashedBytes);

            return passwordHashToValidate == passwordHash ? true : false;
        }
    }
}