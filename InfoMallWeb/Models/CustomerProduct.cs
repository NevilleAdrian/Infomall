using InfoMallWeb.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class CustomerProduct
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CustomerProductId { get; set; }

		[Required]
		public string CustomerDecription { get; set; }

		public bool CustomerHasPaid { get; set; }

		public string ExtraInformation { get; set; }

		public double Price { get; set; }

		public DateTime ExpectedStartDate { get; set; }
		public DateTime ExpectedEndDate { get; set; }
		public DateTime ActualStartDate { get; set; }
		public DateTime ActualEndDate { get; set; }

		public ProductStage Stage { get; set; }

		public PaymentType PaymentType { get; set; }

		[ForeignKey("CustomerId")]
		public Customer Customer { get; set; }
		public string CustomerId { get; set; }
	}
}