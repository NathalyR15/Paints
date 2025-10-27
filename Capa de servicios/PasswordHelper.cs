using System;
using System.Security.Cryptography;

public static class PasswordHelper
{
    public static string HashPassword(string password, int iterations = 100_000)
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] salt = new byte[16];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        return $"{iterations}${Convert.ToHexString(salt)}${Convert.ToHexString(hash)}";
    }

    public static bool VerifyPassword(string password, string stored)
    {
        if (string.IsNullOrEmpty(stored)) return false;
        var parts = stored.Split('$');
        if (parts.Length != 3) return false;

        int iterations = int.Parse(parts[0]);
        byte[] salt = Convert.FromHexString(parts[1]);
        byte[] storedHash = Convert.FromHexString(parts[2]);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        byte[] computed = pbkdf2.GetBytes(storedHash.Length);

        return CryptographicOperations.FixedTimeEquals(computed, storedHash);
    }
}
