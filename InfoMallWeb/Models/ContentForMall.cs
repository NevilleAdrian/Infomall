using InfoMallWeb.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class ContentForMall
	{
		[Key]
		public int ContentId { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string ShortDescription { get; set; }
		[Required]
		[DataType(DataType.Html)]
		public string LongDescription { get; set; }
		public int NumberOfViews { get; set; }
		public bool ShowOnHome { get; set; }
		public DateTime DatePosted { get; set; }
		[DataType(DataType.ImageUrl)]
		public string ImagePath { get; set; }
        public ContentMallPosition Position { get; set; }
		[ForeignKey("CategoryForInformationId")]
		public CategoryForInformation Category { get; set; }
		public int CategoryForInformationId { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
        public int AuthorId { get; set; }
    }
}