using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class PromotionInformation
	{
		[Key]
		public int PromotionInformationId { get; set; }
		[Required]

		public string Title { get; set; }
		[Required]
		public string PromotionInformationContent { get; set; }

		
		[DataType(DataType.ImageUrl)]
		public string ImagePath { get; set; }

        public double Price { get; set; }

        [ForeignKey("PromotionId")]
		public Promotion Promotion { get; set; }
		public int PromotionId { get; set; }
	}
}