using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class BannerInformation
	{
		[Key]
		public int BannerId { get; set; }
		[Required]
		public string BannerContent { get; set; }
		public bool ShowBannerOnHome { get; set; }
		public string ExtraInformation { get; set; }
		[Required]
		[DataType(DataType.ImageUrl)]
		public string ImageUrl { get; set; }
		[ForeignKey("CategoryForTabId")]
		public CategoryForTab Category { get; set; }
		public int CategoryForTabId { get; set; }
	}
}