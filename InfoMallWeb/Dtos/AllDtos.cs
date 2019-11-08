using InfoMallWeb.Enums;
using InfoMallWeb.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace InfoMallWeb.Dtos
{
	public abstract class ContentBase
	{
		public int ContentId { get; set; }
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public string LongDescription { get; set; }
		public bool ShowOnHome { get; set; }
		public string OldImage { get; set; }
		public ICollection<string> ImagesToDelete { get; set; }
		public IFormFile File { get; set; }
		public int CategoryId { get; set; }
		public List<ContentImage> ContentImages { get; set; }
		public ICollection<IFormFile> OtherImage { get; set; }
		public ICollection<string> DescriptionForImage { get; set; }
		public ICollection<string> ExtraData { get; set; }
	}
	public class ContentForMallDto : ContentBase
	{
		public DateTime DatePosted { get; set; }
		public int AuthorId { get; set; }
		public Author Author { get; set; }
        public int NoOfViews { get; set; }
		public CategoryForInformation CategoryForInformation { get; set; }
		public ContentMallPosition ContentMallPosition { get; set; }
	}

	public class ContentForTabDto : ContentBase
	{
		public CategoryForTab CategoryForTab { get; set; }
		public ContentPosition ContentPosition { get; set; }
	}

	public class CustomerDto
	{
		public string CustomersWant { get; set; }
		public string UserId { get; set; }
	}

	public class PromotionCustomerDto
	{
        public int PromotionCustomerId { get; set; }
        public int PromotionId { get; set; }
		public string CustomersWant { get; set; }
		public string UserId { get; set; }
        public bool HasPaid { get; set; }
        public double Price { get; set; }
        public string PromotionDetail { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }

        public PaymentType PaymentType { get; set; }
    }

	public class CustomerReturnDto
	{
		public string CustomerId { get; set; }
		public string UserEmail { get; set; }
		public string UserId { get; set; }
		public ICollection<CustomerProduct> CustomerProducts { get; set; }
		public ICollection<PromotionCustomer> PromotionCustomers { get; set; }
	}

	public class CustumerProductDto
	{
		public int CustomerProductId { get; set; }
		public string CustomerDecription { get; set; }
		public string Service { get; set; }
		public double Price { get; set; }
		public bool CustomerHasPaid { get; set; }
		public DateTime ExpectedStartDate { get; set; }
		public DateTime ExpectedEndDate { get; set; }
		public ProductStage Stage { get; set; }
		public PaymentType PaymentType { get; set; }
		public string UserId { get; set; }
	}

	public class ProductDto
	{
		public string Title { get; set; }
		public string ProductShortInformation { get; set; }
		public string ProductLongInformation { get; set; }
		public string OldImage { get; set; }
		public IFormFile File { get; set; }
		public int CategoryForTabId { get; set; }
	}

	public class PromotionDto
	{
		public int PromotionId { get; set; }
		public string PromotionName { get; set; }
        public string PromotionExtra { get; set; }
        public string PromotionDescription { get; set; }
        public bool PromotionAvailable { get; set; }
		public string OldImage { get; set; }
        public IFormFile File { get; set; }
		public int CategoryForTabId { get; set; }
		public ICollection<PromotionInformation> PromotionsInformation { get; set; }
		public ICollection<PromotionCustomer> PromotionCustomers { get; set; }
	}

	public class PromotionInformationDto
	{
		public int PromotionInformationId { get; set; }
		public string Title { get; set; }
		public string PromotionInformationContent { get; set; }
		public string OldImage { get; set; }
		public IFormFile File { get; set; }
        public double Price { get; set; }
        public int PromotionId { get; set; }
		public Promotion Promotion { get; set; }
	}

	public class BannerInformationDto
	{
		public int BannerId { get; set; }
		public string BannerContent { get; set; }
		public string OldImage { get; set; }
		public IFormFile File { get; set; }
		public int CategoryForTabId { get; set; }
		public CategoryForTab Category { get; set; }
		public bool ShowBannerOnHome { get; set; }
		public string ExtraInformation { get; set; }
	}

	public class SendUserEmailDto
	{
		public string Subject { get; set; }
		public string Message { get; set; }
		public string UserId { get; set; }
	}

	public class ClienteleDto
	{
		public int ClienteleId { get; set; }
		public string OldImage { get; set; }
		public IFormFile File { get; set; }
		public ClientelePriority Priority { get; set; }
	}

	public class ContentImageDto
	{
		public int ContentImageId { get; set; }
		public string Description { get; set; }
		public string ExtraData { get; set; }
		public IFormFile File { get; set; }
		public string OldImage { get; set; }
		public ContentForTab ContentForTab { get; set; }
		public int ContentForTabId { get; set; }
	}

	public class ContentPageDto
	{
		public BannerInformation BannerInformation { get; set; }
		public CategoryForTab Category { get; set; }
	}

	public class HomePageDto
	{
        public List<CategoryForTab> TabCategories { get; set; }
        public List<CategoryForInformation> InfoCategories { get; set; }
        public List<ClienteleDto> Clienteles { get; set; }
    }


}
