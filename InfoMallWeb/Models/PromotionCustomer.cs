using InfoMallWeb.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfoMallWeb.Models
{
	public class PromotionCustomer
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int PromotionCustomerId { get; set; }

		[ForeignKey("PromotionId")]
		public Promotion Promotion { get; set; }
		public int PromotionId { get; set; }

        public bool HasPaid { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public double Price { get; set; }
        public string PromotionDetail { get; set; }
        
        public PaymentType PaymentType { get; set; }

        [ForeignKey("CustomerId")]
		public Customer Customer { get; set; }
		public string CustomerId { get; set; }
		
	}
}