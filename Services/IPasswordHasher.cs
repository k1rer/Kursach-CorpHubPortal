namespace Kursach_CorpHubPortal.Services
{
    public interface IPasswordHasher
    {
        // Отдельно создает соль и хеш пароля
        (string salt, string hash) CreateHashAndSalt(string password);

        // Объединение соли с хешом
        public string CombineSaltAndHash(string salt, string password);

        // Создает хеш пароля с солью
        string HashPassword(string password);

        // Проверяет пароль с хешем
        bool VerifyPassword(string password, string hashedPassword);

        // Проверяет пароль с отдельно хранящимися солью и хешем
        bool VerifyWithSalt(string password, string salt, string storedHash);
    }
}
