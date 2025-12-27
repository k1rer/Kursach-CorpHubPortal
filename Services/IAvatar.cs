namespace Kursach_CorpHubPortal.Services
{
    public interface IAvatar
    {
        public string? ValidateAvatar(IFormFile file);
        public  Task<string> SaveAvatarAsync(int userId, IFormFile file, string? oldPath);
    }
}
