using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MinimalApi.Security;

public static class PasswordHasher
{
    public static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

        var hash = Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 256 / 8
            )
        );

        return $"{Convert.ToBase64String(salt)}.{hash}";
    }

    public static bool Verify(string password, string stored)
    {
        var parts = stored.Split('.');
        var salt = Convert.FromBase64String(parts[0]);
        var storedHash = parts[1];

        var hash = Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 256 / 8
            )
        );

        return hash == storedHash;
    }
}

