using Microsoft.AspNetCore.Http;

namespace InfoMallWeb.Models
{
    public class AuthorDto : Author
    {
        public IFormFile File { get; set; }
    }
}
