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
	public class CustomerProductRepository : ICustomerProductRepository
	{
		private readonly ApplicationDbContext _ctx;
		private readonly ICustomerRepository _cusRepository;
		private readonly ILogger<CustomerProductRepository> _logger;
		private readonly IEmailSender _email;

		public CustomerProductRepository(ApplicationDbContext context,
			ILogger<CustomerProductRepository> logger,
			ICustomerRepository cusRepository,
			IEmailSender email)
		{
			_ctx = context;
			_logger = logger;
			_cusRepository = cusRepository;
			_email = email;
		}

		public async Task<(bool, bool)> AddCustomerProduct(CustumerProductDto customerProductDto)
		{
			bool mailSent = false;
			CustomerDto customer = new CustomerDto
			{
				CustomersWant = customerProductDto.CustomerDecription,
				UserId = customerProductDto.UserId
			};
			(bool, bool, string) result = await _cusRepository.AddCustomer(customer);
			if (result.Item1)
			{
				if (result.Item2)
				{
					mailSent = true;
				}
				CustomerProduct customerProduct = new CustomerProduct
				{
					CustomerDecription = $"{customerProductDto.Service} \n {customerProductDto.CustomerDecription}",
					CustomerHasPaid = customerProductDto.CustomerHasPaid,
					CustomerId = result.Item3,
					ExpectedEndDate = customerProductDto.ExpectedEndDate,
					ExpectedStartDate = customerProductDto.ExpectedStartDate,
					PaymentType = customerProductDto.PaymentType,
					Stage = customerProductDto.Stage,
				};

				try
				{
					_ctx.CustomerProducts.Add(customerProduct);
					await _ctx.SaveChangesAsync();
					return (true, mailSent);
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"could not add customer product: {ex.Message}");
					return (false, mailSent);
				}

			}
			return (false, mailSent);
		}

		public async Task<CustomerProduct> GetCustomerProductById(int id)
		{
			if (CustomerProductExists(id))
			{
				CustomerProduct customerProduct = await _ctx.CustomerProducts.Where(c => c.CustomerProductId == id)
																			.Include(c => c.Customer).ThenInclude(u => u.User)
                                                                            .SingleOrDefaultAsync();

				return customerProduct;
			}
			return null;
		}

		public async Task<List<CustomerProduct>> GetAllCustomerProducts() => await _ctx.CustomerProducts.Include(c => c.Customer).ThenInclude(u => u.User).ToListAsync();

		public async Task UpdateCustomerProductWithId(CustomerProduct customerProduct)
		{
			if (CustomerProductExists(customerProduct.CustomerProductId))
			{
				if (!string.IsNullOrEmpty(customerProduct.ExtraInformation))
				{
					try
					{
						CustomerReturnDto customer = await _cusRepository.GetCustomerByCustomerId(customerProduct.CustomerId);
						await _email.SendEmailAsync(customer.UserEmail, "AN UPDATE TO YOUR PRODUCT FROM INFOMALL", customerProduct.ExtraInformation);

					}
					catch (Exception ex)
					{
						_logger.LogInformation($"Cannot not get customer or send email: {ex.Message}");
					}
				}
				try
				{
                    CustomerProduct cp = await _ctx.CustomerProducts.Where(c => c.CustomerProductId == customerProduct.CustomerProductId)
                        .Select(c => new CustomerProduct
                        {
                            ActualEndDate = c.ActualEndDate,
                            ActualStartDate = c.ActualStartDate,
                            CustomerDecription = c.CustomerDecription,
                            CustomerId = c.CustomerId,
                            PaymentType = customerProduct.PaymentType,
                            ExtraInformation = !string.IsNullOrEmpty(customerProduct.ExtraInformation) ? customerProduct.ExtraInformation : c.ExtraInformation,
                            Price = customerProduct.Price,
                            Stage = customerProduct.Stage,
                            ExpectedEndDate = !c.CustomerHasPaid && customerProduct.CustomerHasPaid ? DateTime.Now.AddDays(14) : c.ExpectedEndDate,
                            ExpectedStartDate = !c.CustomerHasPaid && customerProduct.CustomerHasPaid ? DateTime.Now : c.ExpectedStartDate,
                            CustomerHasPaid = customerProduct.CustomerHasPaid,
                            CustomerProductId = c.CustomerProductId
                        }).SingleOrDefaultAsync();
					_ctx.Update(cp);
					await _ctx.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"could not update customer product: {ex.Message}");
				}
			}
		}

		public void DeleteCustomerProductWithId(int id)
		{
			if (CustomerProductExists(id))
			{
				try
				{
					CustomerProduct customerProduct = _ctx.CustomerProducts.Find(id);
					_ctx.CustomerProducts.Remove(customerProduct);
					_ctx.SaveChanges();
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"could not delete customer product: {ex.Message}");
				}
			}
		}

		public bool CustomerProductExists(int id) => _ctx.CustomerProducts.Any(c => c.CustomerProductId == id);
	}
}