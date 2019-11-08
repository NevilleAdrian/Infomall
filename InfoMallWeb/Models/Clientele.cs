using InfoMallWeb.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class Clientele
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ClienteleId { get; set; }

		[Required]
		[DataType(DataType.ImageUrl)]
		public string ImageUrl { get; set; }

		public ClientelePriority Priority { get; set; }
	}
}