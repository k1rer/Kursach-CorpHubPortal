using System.Security.Cryptography;

namespace Kursach_CorpHubPortal.Services.Implementations
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public (string salt, string hash) CreateHashAndSalt(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(HashSize);
            }

            return (
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash)
            );
        }

        // Создает хеш пароля с солью
        public string HashPassword(string password)
        {
            var (salt, hash) = CreateHashAndSalt(password);

            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] hashBytes = Convert.FromBase64String(hash);

            byte[] combinedBytes = new byte[SaltSize + HashSize];
            Array.Copy(saltBytes, 0, combinedBytes, 0, SaltSize);
            Array.Copy(hashBytes, 0, combinedBytes, SaltSize, HashSize);

            return Convert.ToBase64String(combinedBytes);
        }

        // Проверяет пароль с хешем
        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);

                byte[] salt = new byte[SaltSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltSize);

                byte[] storedHash = new byte[HashSize];
                Array.Copy(hashBytes, SaltSize, storedHash, 0, HashSize);

                byte[] computedHash;
                using (var pbkdf2 = new Rfc2898DeriveBytes(
                    password,
                    salt,
                    Iterations,
                    HashAlgorithmName.SHA256))
                {
                    computedHash = pbkdf2.GetBytes(HashSize);
                }

                return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
            }
            catch
            {
                return false;
            }
        }

        // Проверяет пароль с отдельно хранящимися солью и хешем
        public bool VerifyWithSalt(string password, string salt, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(salt) || string.IsNullOrEmpty(storedHash))
                return false;

            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] storedHashBytes = Convert.FromBase64String(storedHash);

            byte[] newHash;
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                saltBytes,
                Iterations, 
                HashAlgorithmName.SHA256))
            {
                newHash = pbkdf2.GetBytes(HashSize);
            }

            return CryptographicOperations.FixedTimeEquals(newHash, storedHashBytes);
        }

        // Объединение соли с хешом
        public string CombineSaltAndHash(string salt, string password)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
