using InfoMallWeb.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class ContentForTab
	{
		[Key]
		public int ContentId { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string ShortDescription { get; set; }
		public bool ShowOnHome { get; set; }
		[Required]
		[DataType(DataType.Html)]
		public string LongDescription { get; set; }
		[DataType(DataType.ImageUrl)]
		public string ImagePath { get; set; }
		public ContentPosition Position { get; set; }
		[ForeignKey("CategoryForTabId")]
		public CategoryForTab Category { get; set; }
		public int CategoryForTabId { get; set; }

		public List<ContentImage> ContentImages { get; set; }
	}
}