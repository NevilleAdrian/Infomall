using InfoMallWeb.Data;
using InfoMallWeb.Dtos;
using InfoMallWeb.Models;
using InfoMallWeb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public class PromotionCustomerRepository : IPromotionCustomerRepository
	{
		private readonly ApplicationDbContext _ctx;
		private readonly ICustomerRepository _cusRepository;
		private readonly ILogger<PromotionCustomerRepository> _logger;
		private readonly IEmailSender _email;

		public PromotionCustomerRepository(ApplicationDbContext context,
			ILogger<PromotionCustomerRepository> logger,
			ICustomerRepository cusRepository,
			IEmailSender email)
		{
			_ctx = context;
			_logger = logger;
			_cusRepository = cusRepository;
			_email = email;
		}
		public async Task<(bool, bool)> AddCustomerProduct(PromotionCustomerDto promotionCustomerDto)
		{
			bool mailSent = false;
			CustomerDto customer = new CustomerDto
			{
				CustomersWant = promotionCustomerDto.CustomersWant,
				UserId = promotionCustomerDto.UserId
			};
			(bool, bool, string) result = await _cusRepository.AddCustomer(customer);
			if (result.Item1)
			{
				if (result.Item2)
				{
					mailSent = true;
				}
				PromotionCustomer promotionCustomer = new PromotionCustomer
				{
					CustomerId = result.Item3,
					PromotionId = promotionCustomerDto.PromotionId,
                    HasPaid = false,
                    Price = promotionCustomerDto.Price,
                    PromotionDetail = promotionCustomerDto.PromotionDetail,
                    PaymentType = Enums.PaymentType.None
				};

				try
				{
					_ctx.PromotionCustomers.Add(promotionCustomer);
					await _ctx.SaveChangesAsync();
					return (true, mailSent);
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"could not add customer promotion: {ex.Message}");
					return (false, mailSent);
				}

			}
			return (false, mailSent);
		}

		public async Task<PromotionCustomer> GetCustomerProductById(int id)
		{
			if (PromtionCustomerExists(id))
			{
				return await _ctx.PromotionCustomers.Include(p => p.Promotion).ThenInclude(pI => pI.PromotionsInformation)
                                                    .Include(p => p.Customer).ThenInclude(c => c.User)
                                                    .Include(p => p.Customer).ThenInclude(c => c.MyPromotions)
													.SingleOrDefaultAsync(p => p.PromotionCustomerId == id);
			}
			return null;
		}

        public async Task<List<PromotionCustomer>> GetAllCustomerProductsByUserId()
        {
                return await _ctx.PromotionCustomers.Include(p => p.Promotion).ThenInclude(pI => pI.PromotionsInformation)
                                                    .Include(p => p.Customer).ThenInclude(c => c.User)
                                                    .ToListAsync();
        }

        public async Task<List<PromotionCustomer>> GetAllCustomerProduct()
		{
			return await _ctx.PromotionCustomers.Include(p => p.Promotion).ThenInclude(pI => pI.PromotionsInformation)
												.Include(p => p.Customer).ThenInclude(c => c.User)
												.ToListAsync();
		
		}

        public async Task UpdatePromotionCustomerWithId(PromotionCustomerDto promotionCustomer)
        {
            if (PromtionCustomerExists(promotionCustomer.PromotionCustomerId))
            {
                try
                {
                    PromotionCustomer promotion = await _ctx.PromotionCustomers
                        .Where(p => p.PromotionCustomerId == promotionCustomer.PromotionCustomerId)
                        .Select(p => new PromotionCustomer {
                            CustomerId = p.CustomerId,
                            Price = promotionCustomer.Price,
                            PromotionId = p.PromotionId,
                            HasPaid = promotionCustomer.HasPaid,
                            PaymentType = promotionCustomer.PaymentType,
                            ActualEndDate = promotionCustomer.ActualEndDate,
                            PromotionDetail = !string.IsNullOrEmpty(promotionCustomer.PromotionDetail) ? promotionCustomer.PromotionDetail : p.PromotionDetail,
                            ActualStartDate = promotionCustomer.ActualStartDate,
                            ExpectedStartDate = promotionCustomer.HasPaid && !p.HasPaid ? DateTime.Now : promotionCustomer.ExpectedStartDate,
                            ExpectedEndDate = promotionCustomer.HasPaid && !p.HasPaid ? DateTime.Now.AddDays(14) :  promotionCustomer.ExpectedEndDate,
                            PromotionCustomerId = p.PromotionCustomerId
                           
                        }).SingleOrDefaultAsync();

                    _ctx.Update(promotion);
                    await _ctx.SaveChangesAsync();
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"could not delete promotion customer: {ex.Message}");
                }
            }
            throw new Exception();
        }

        public async Task DeletePromotionCustomerWithId(int id)
		{
			if (PromtionCustomerExists(id))
			{
				try
				{
					PromotionCustomer promotionCustomer = await _ctx.PromotionCustomers.FindAsync(id);
					_ctx.Remove(promotionCustomer);
					await _ctx.SaveChangesAsync();
                    return;
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"could not delete promotion customer: {ex.Message}");
				}
			}
            throw new Exception();
		}

		public bool PromtionCustomerExists(int id) => _ctx.PromotionCustomers.Any(p => p.PromotionCustomerId == id);
	}
}