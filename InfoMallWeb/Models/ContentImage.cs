using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class ContentImage
	{
		[Key]
		public int ContentImageId { get; set; }
		[Required]
		public string Description { get; set; }
		public string ExtraData { get; set; }
		public string CarImagePath { get; set; }
		[ForeignKey("ContentForTabId")]
		public ContentForTab ContentForTab { get; set; }
		public int ContentForTabId { get; set; }
	}
}