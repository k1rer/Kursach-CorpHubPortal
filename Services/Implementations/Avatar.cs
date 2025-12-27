using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Kursach_CorpHubPortal.Services.Implementations
{
    public class Avatar : IAvatar
    {
        private const int MaxAvatarSizeBytes = 10 * 1024 * 1024;
        private const int MinAvatarWidth = 200;
        private const int MinAvatarHeight = 200;
        private const int TargetAvatarSize = 400;
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };


        public string? ValidateAvatar(IFormFile file)
        {
            if (file == null || file.Length == 0) return "файл не выбран";

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtensions.Contains(extension))
            {
                return "недопустимый формат (разрешены JPG, PNG)";
            }

            if (file.Length > MaxAvatarSizeBytes)
            {
                return "слишком большой вес (макс. 10 МБ)";
            }

            try
            {
                using var stream = file.OpenReadStream();

                var imageInfo = SixLabors.ImageSharp.Image.Identify(stream);

                if (imageInfo == null) return "файл поврежден";

                if (imageInfo.Width < MinAvatarWidth || imageInfo.Height < MinAvatarHeight)
                {
                    return "слишком малое разрешение";
                }
            }
            catch
            {
                return "ошибка чтения формата изображения";
            }

            return null;
        }

        public async Task<string> SaveAvatarAsync(int userId, IFormFile file, string? oldPath)
        {
            // Подготовка путей
            var fileName = $"avatar_{userId}_{DateTime.Now.Ticks}.jpg";
            var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");

            if (!Directory.Exists(webRootPath)) Directory.CreateDirectory(webRootPath);

            var filePath = Path.Combine(webRootPath, fileName);

            // Обработка изображения
            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                // "Шакалим" (Stretch) — принудительно растягиваем в квадрат 400x400
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(TargetAvatarSize, TargetAvatarSize),
                    Mode = ResizeMode.Stretch
                }));

                // Сохраняем в JPEG (85% качество) для идеального баланса веса и поддержки браузерами
                await image.SaveAsJpegAsync(filePath, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder { Quality = 85 });
            }

            // Удаление старой аватарки
            if (!string.IsNullOrEmpty(oldPath))
            {
                var oldPhysicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldPath.TrimStart('/'));
                if (File.Exists(oldPhysicalPath))
                {
                    try { File.Delete(oldPhysicalPath); } catch { /* логируем, но не прерываем работу */ }
                }
            }

            return $"/uploads/avatars/{fileName}";
        }
    }
}
