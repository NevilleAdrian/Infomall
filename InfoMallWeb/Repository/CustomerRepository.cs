using InfoMallWeb.Data;
using InfoMallWeb.Dtos;
using InfoMallWeb.Models;
using InfoMallWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly ApplicationDbContext _ctx;
		private readonly ILogger<CustomerRepository> _logger;
		private readonly IEmailSender _email;
		private readonly UserManager<ApplicationUser> _userManager;

		public CustomerRepository(ApplicationDbContext context,
			ILogger<CustomerRepository> logger,
			IEmailSender email,
			UserManager<ApplicationUser> userManager)
		{
			_ctx = context;
			_logger = logger;
			_email = email;
			_userManager = userManager;
		}

		public async Task<(bool, bool, string)> AddCustomer(CustomerDto customerDto)
		{
			Customer customer = new Customer
			{
				UserId = customerDto.UserId
			};
			try
			{
				_ctx.Customers.Add(customer);
				await _ctx.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"could not add customer: {ex.Message}");
				return (false, false, null);
			}
			try
			{
				var user = await _userManager.Users.Where(u => u.Id == customer.UserId).SingleOrDefaultAsync();
				string userEmail = user.Email;
                string userName = user.FirstName;
                string message = $"Dear {userName}, \n\nThank you for being our customer. We are indeed grateful.\nYou said you want this:\n '{customerDto.CustomersWant}' \n\nWarm wishes,\nChioma Mercy.";
				await _email.SendEmailAsync(userEmail, "APPRECIATION FROM INFOMALL", message);
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"could not send email: {ex.Message}");
				return (true, false, customer.CustomerId);
			}

			return (true, true, customer.CustomerId);

		}

		public async Task<CustomerReturnDto> GetCustomerByCustomerId(string id, bool includeMyPromotion = false, bool includeMyProducts = false)
		{
			if (CustomerExists(id))
			{
				if (includeMyProducts && includeMyPromotion)
				{
					Customer customer = await _ctx.Customers.Include(c => c.User)
												.Include(c => c.CustomerProducts)
												.Include(c => c.MyPromotions).SingleOrDefaultAsync();
					var user = await _userManager.Users.Where(u => u.Id == customer.UserId).SingleOrDefaultAsync();
					string userEmail = user.Email;

					CustomerReturnDto customerReturn = new CustomerReturnDto
					{
						CustomerId = customer.CustomerId,
						UserEmail = userEmail,
						UserId = customer.UserId,
						CustomerProducts = customer.CustomerProducts,
						PromotionCustomers = customer.MyPromotions
					};

					return customerReturn;
				}
				else if (includeMyProducts)
				{
					Customer customer = await _ctx.Customers.Include(c => c.User)
												.Include(c => c.CustomerProducts)
												.SingleOrDefaultAsync();

					var user = await _userManager.Users.Where(u => u.Id == customer.UserId).SingleOrDefaultAsync();
					string userEmail = user.Email;

					CustomerReturnDto customerReturn = new CustomerReturnDto
					{
						CustomerId = customer.CustomerId,
						UserEmail = userEmail,
						UserId = customer.UserId,
						CustomerProducts = customer.CustomerProducts
					};

					return customerReturn;
				}
				else if (includeMyPromotion)
				{
					Customer customer = await _ctx.Customers.Include(c => c.User)
												.Include(c => c.MyPromotions)
												.SingleOrDefaultAsync();
					var user = await _userManager.Users.Where(u => u.Id == customer.UserId).SingleOrDefaultAsync();
					string userEmail = user.Email;
					
					CustomerReturnDto customerReturn = new CustomerReturnDto
					{
						CustomerId = customer.CustomerId,
						UserEmail = userEmail,
						UserId = customer.UserId,
						PromotionCustomers = customer.MyPromotions
					};

					return customerReturn;
				}
				else {
					Customer customer = await _ctx.Customers.Include(c => c.User)
												.SingleOrDefaultAsync();

					var user = await _userManager.Users.Where(u => u.Id == customer.UserId).SingleOrDefaultAsync();
					string userEmail = user.Email;

					CustomerReturnDto customerReturn = new CustomerReturnDto
					{
						CustomerId = customer.CustomerId,
						UserEmail = userEmail,
						UserId = customer.UserId
					};

					return customerReturn;
				}
				
			}
			return null;
		}

		public async Task<List<CustomerReturnDto>> GetAllCustomers(bool includeMyPromotion = false, bool includeMyProducts = false)
		{

			if (includeMyProducts && includeMyPromotion)
			{
				return await _ctx.Customers.Include(c => c.User)
												.Include(c => c.CustomerProducts)
												.Include(c => c.MyPromotions)
												.Select(c => new CustomerReturnDto
												{
													CustomerId = c.CustomerId,
													CustomerProducts = c.CustomerProducts,
													PromotionCustomers = c.MyPromotions,
													UserEmail = _userManager.Users.Where(u => u.Id == c.UserId).SingleOrDefault().Email,
													UserId = c.UserId
												}).ToListAsync();
			}
			else if (includeMyProducts)
			{
				return await _ctx.Customers.Include(c => c.User)
											.Include(c => c.CustomerProducts)
											.Select(c => new CustomerReturnDto
											{
												CustomerId = c.CustomerId,
												PromotionCustomers = c.MyPromotions,
												UserEmail = _userManager.Users.Where(u => u.Id == c.UserId).SingleOrDefault().Email,
												UserId = c.UserId
											}).ToListAsync();
			}
			else if (includeMyPromotion)
			{
				return await _ctx.Customers.Include(c => c.User)
											.Include(c => c.MyPromotions)
											.Select(c => new CustomerReturnDto
											{
												CustomerId = c.CustomerId,
												PromotionCustomers = c.MyPromotions,
												UserEmail = _userManager.Users.Where(u => u.Id == c.UserId).SingleOrDefault().Email,
												UserId = c.UserId
											}).ToListAsync();
			}
			return await _ctx.Customers.Include(c => c.User)
											.Select(c => new CustomerReturnDto
											{
												CustomerId = c.CustomerId,
												UserEmail = _userManager.Users.Where(u => u.Id == c.UserId).SingleOrDefault().Email,
												UserId = c.UserId
											}).ToListAsync();
		}

		public async Task<bool> DeleteCustomerById(string id)
		{
			if (CustomerExists(id))
			{
				try
				{
					Customer customer = await _ctx.Customers.FindAsync(id);
					_ctx.Remove(customer);
					await _ctx.SaveChangesAsync();
					return true;
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"could not send email: {ex.Message}");
					return false;
				}
			}
			return false;
		}

		public bool CustomerExists(string id) => _ctx.Customers.Any(c => c.CustomerId == id);
		public bool PromotionCustomerExists(int id) => _ctx.PromotionCustomers.Any(c => c.PromotionCustomerId == id);
		public bool CustomerProductExists(int id) => _ctx.CustomerProducts.Any(c => c.CustomerProductId == id);


	}

	public interface ICustomerRepository
	{
		Task<(bool, bool, string)> AddCustomer(CustomerDto customerDto);
		Task<CustomerReturnDto> GetCustomerByCustomerId(string id, bool includeMyPromotion = false, bool includeMyProducts = false);
	}
}
