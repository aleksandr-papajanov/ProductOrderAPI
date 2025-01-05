using System.Security.Cryptography;

namespace ProductOrderApi.Helpers
{
    internal class PasswordHelper
    {
        private const int SALT_SIZE = 16;
        private const int HASH_SIZE = 32;
        private const int ITERATIONS = 10000;

        public static string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[SALT_SIZE];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(HASH_SIZE);

            byte[] hashBytes = new byte[SALT_SIZE + HASH_SIZE];
            Array.Copy(salt, 0, hashBytes, 0, SALT_SIZE);
            Array.Copy(hash, 0, hashBytes, SALT_SIZE, HASH_SIZE);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[SALT_SIZE];
            Array.Copy(hashBytes, 0, salt, 0, SALT_SIZE);

            byte[] originalHash = new byte[HASH_SIZE];
            Array.Copy(hashBytes, SALT_SIZE, originalHash, 0, HASH_SIZE);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS, HashAlgorithmName.SHA256);
            byte[] newHash = pbkdf2.GetBytes(HASH_SIZE);

            return CryptographicOperations.FixedTimeEquals(originalHash, newHash);
        }
    }
}
