using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace InfoMallWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool UserLikesMail { get; set; }
        [Required(ErrorMessage = "Please provide your first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide your last name")]
        public string LastName { get; set; }
    }
}
