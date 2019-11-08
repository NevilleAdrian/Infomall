using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class CategoryForTab
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CategoryId { get; set; }

		[Required]
		public string CategoryName { get; set; }

        public string CategoryInformation { get; set; }

        public ICollection<ContentForTab> ContentForTabs { get; set; }
		public ICollection<Promotion> Promotions { get; set; }
		public ICollection<BannerInformation> Banners { get; set; }

	}
}
