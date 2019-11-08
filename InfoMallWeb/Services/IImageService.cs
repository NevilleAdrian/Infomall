
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace InfoMallWeb.Services
{
    public interface IImageService
    {
        string CreateImage(IFormFile file);
        string EditImage(IFormFile file, string imageUrl);
        void DeleteImage(string ImageUrl);
        bool DeleteImages(List<string> ImageUrls);
    }
}
