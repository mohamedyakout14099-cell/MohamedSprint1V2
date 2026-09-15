using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MohamedSprint1V2.DLL.Service.Abstraction
{
    public interface IFileService
    {
        Task<Response<string?>> UploadImageAsync(IFormFile? imageFile);
        Task<Response<bool>> DeleteImageAsync(string imagePath);
    }
}
