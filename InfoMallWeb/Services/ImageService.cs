using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace InfoMallWeb.Services
{
    public class ImageService : IImageService
    {
        private const string default_Path = "images";
        private IHostingEnvironment _env;
        public static Random random = new Random(55);
        public ImageService(IHostingEnvironment env)
        {
            _env = env;
        }
        public string CreateImage(IFormFile file)
        {
            string relativePath = "";
            if (file != null)
            {
                int randomId = random.Next(56, 1000);
                var fileName = $"{randomId}{Path.GetFileName(file.FileName)}";
                relativePath = Path.Combine(default_Path, fileName);
                var absolutePath = Path.Combine(_env.WebRootPath, relativePath);

                using (FileStream stream = new FileStream(absolutePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

            }
            return relativePath;
        }

        public string EditImage(IFormFile file, string imageUrl)
        {
            string relativePath = "";
            if (file != null)
            {
                if (imageUrl != null)
                {
                    var oldPath = Path.Combine(_env.WebRootPath, imageUrl);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                int randomId = random.Next(56, 1000);
                var fileName = $"{randomId}{Path.GetFileName(file.FileName)}";
                relativePath = Path.Combine(default_Path, fileName);
                var absolutePath = Path.Combine(_env.WebRootPath, relativePath);

                using (FileStream stream = new FileStream(absolutePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

            }
            return relativePath;
        }

        public void DeleteImage(string ImageUrl)
        {
            if (string.IsNullOrEmpty(ImageUrl))
            {
                ImageUrl = "";
            }
            var oldPath = Path.Combine(_env.WebRootPath, ImageUrl);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }
        }

        public bool DeleteImages(List<string> ImageUrls)
        {
            foreach(string img in ImageUrls)
            {
                if (!string.IsNullOrEmpty(img))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, img);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }
                
            }
            return true;
            
        }

    }
}
