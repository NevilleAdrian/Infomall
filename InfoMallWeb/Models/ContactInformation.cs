using InfoMallWeb.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class ContactInformation
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ContactId { get; set; }

		[Required]
		public string Message { get; set; }
        
        public string Subject { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }
}