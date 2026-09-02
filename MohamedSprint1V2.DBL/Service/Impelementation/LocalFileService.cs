using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using MohamedSprint1V2.DLL.Service.Abstraction;

namespace MohamedSprint1V2.DLL.Service.Impelementation
{
    public class LocalFileService : IFileService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public LocalFileService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<Response<string?>> UploadImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return new Response<string?>( null, "No image file provided.",false);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return new Response<string?>(null, "Invalid image file type. Only .jpg, .jpeg, .png, and .webp are allowed.", false);

            }
            if(imageFile.Length > 2 * 1024 * 1024) // 2MB limit
            {
                return new Response<string?>(null, "Image file size exceeds the 2MB limit.", false);
            }
            var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images", "products");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return new Response<string?>( "/images/products/" + uniqueFileName, null, true);
        }
        public async Task<Response<bool>> DeleteImageAsync(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return new Response<bool>(false, "Image path is null or empty.", false);
            var fullPath = Path.Combine(webHostEnvironment.WebRootPath, imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(fullPath))
                return new Response<bool>(false, "Image file does not exist.", false);
            try
            {
                File.Delete(fullPath);
                return new Response<bool>(true, null, true);
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, $"Error deleting image file: {ex.Message}", false);
            }
        }
    }
}
