using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class Promotion
	{
		[Key]
		public int PromotionId { get; set; }

		[Required]
		public string PromotionName { get; set; }
		public string PromotionExtra { get; set; }
		public string PromotionDescription { get; set; }

		public bool PromotionAvailable { get; set; }

		[DataType(DataType.ImageUrl)]
		public string ImagePath { get; set; }

		[ForeignKey("CategoryForTabId")]
		public CategoryForTab Category { get; set; }
		public int CategoryForTabId { get; set; }
		
		public ICollection<PromotionInformation> PromotionsInformation { get; set; }
		public ICollection<PromotionCustomer> PromotionCustomers { get; set; }
	}
}