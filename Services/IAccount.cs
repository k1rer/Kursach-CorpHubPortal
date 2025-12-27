using Microsoft.Identity.Client;
using Kursach_CorpHubPortal.Model.DTO;

namespace Kursach_CorpHubPortal.Services
{
    public interface IAccount
    {
        public Task<bool> LoginAsync(LoginDTO loginData);
        public Task<(bool success, string message)> RegisterAsync(RegisterDTO model);
    }
}
