using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class CategoryForInformation
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CategoryId { get; set; }

		[Required]
		public string CategoryName { get; set; }

		public string CategoryInformation { get; set; }

		public List<ContentForMall> ContentsForInformation { get; set; }
	}
}