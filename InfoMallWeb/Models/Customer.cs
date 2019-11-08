using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class Customer
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string CustomerId { get; set; }

		[ForeignKey("UserId")]
		public ApplicationUser User { get; set; }
		public string UserId { get; set; }

		public List<CustomerProduct> CustomerProducts { get; set; }
		public List<PromotionCustomer> MyPromotions { get; set; }
	}
}