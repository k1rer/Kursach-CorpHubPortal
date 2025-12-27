using Kursach_CorpHubPortal.Data;
using Kursach_CorpHubPortal.Data.Entities;
using Kursach_CorpHubPortal.Data.Enums;
using Kursach_CorpHubPortal.Model.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kursach_CorpHubPortal.Services.Implementations
{
    public class Account : IAccount
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Account(IPasswordHasher passwordHasher, ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _passwordHasher = passwordHasher;
            _context = applicationDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(LoginDTO loginData)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginData.Email
                                       && u.PhoneNumber == NormalizePhoneNumber(loginData.PhoneNumber));
            if (user == null) return false;

            bool isPasswordValid = _passwordHasher.VerifyWithSalt(
                loginData.Password,
                user.PasswordSalt,
                user.PasswordHash
            );
            if (!isPasswordValid) return false;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("ProfilePictureUrl", user.ProfilePictureUrl ?? "/img/default-avatar.png")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("HttpContext не доступен.");
            }

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = loginData.RememberMe,

                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return true;
        }

        public async Task<(bool success, string message)> RegisterAsync(RegisterDTO model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email || u.PhoneNumber == model.PhoneNumber))
                return (false, "Пользователь с такими данными уже существует");

            var (salt, hash) = _passwordHasher.CreateHashAndSalt(model.Password);

            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Email = model.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                PhoneNumber = NormalizePhoneNumber(model.PhoneNumber),
                Role = model.Role,
                Status = UserStatus.Active,
                DepartmentId = model.DepartmentId,
                PositionId = model.PositionId,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return (true, "Регистрация успешна");
        }

        private string NormalizePhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return string.Empty;

            var cleaned = new string(phone.Where(char.IsDigit).ToArray());

            if (cleaned.StartsWith("8") && cleaned.Length == 11)
            {
                cleaned = "7" + cleaned.Substring(1);
            }

            if (cleaned.Length == 10)
            {
                cleaned = "7" + cleaned;
            }

            return cleaned;
        }
    }
}
