using InfoMallWeb.Data;
using InfoMallWeb.Dtos;
using InfoMallWeb.Models;
using InfoMallWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public class PromotionRepository : IPromotionRepository
	{
		private readonly ApplicationDbContext _ctx;
		private readonly IImageService _img;
		private readonly IHostingEnvironment _env;
		private readonly ILogger<PromotionRepository> _logger;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IEmailSender _email;



		public PromotionRepository(ApplicationDbContext context,
			IImageService image,
			IHostingEnvironment env,
			ILogger<PromotionRepository> logger,
			UserManager<ApplicationUser> userManager,
			IEmailSender email)
		{
			_ctx = context;
			_img = image;
			_env = env;
			_logger = logger;
			_userManager = userManager;
			_email = email;
		}

		public async Task<(bool, bool)> AddPromotion(PromotionDto promoDto, bool sendToAll)
		{
			string path = null;
			if (promoDto.File != null)
			{
				path = _img.CreateImage(promoDto.File);
			}
			Promotion promotion = new Promotion
			{
				CategoryForTabId = promoDto.CategoryForTabId,
				ImagePath = path,
				PromotionAvailable = promoDto.PromotionAvailable,
				PromotionName = promoDto.PromotionName,
                PromotionDescription = promoDto.PromotionDescription,
                PromotionExtra = promoDto.PromotionExtra
			};
			try
			{
				_ctx.Promotions.Add(promotion);
				await _ctx.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Promotion record not created: {ex.Message}");
				return (false, false);

			}
			if (sendToAll)
			{
				var usersEmail = await _userManager.Users.Where(u => u.UserLikesMail).Select(u => u.Email).ToListAsync();
				if (usersEmail.Count > 0)
				{
					string subject = "WE ARE EXCITED TO TELL YOU ABOUT OUR PROMOTION";
					string message = $"Dear Customer,\n{promotion.PromotionName}\n{promotion.PromotionsInformation} \n\nWarm regards,\nNwisu.";
					try
					{
						await _email.SendEmailToAllAsync(usersEmail, subject, message);
					}
					catch (Exception ex)
					{
						_logger.LogInformation($"Cannot send mail: {ex.Message}");
						return (true, false);
					}
				}
				return (true, false);
			}
			return (true, true);
		}

		public async Task<PromotionDto> GetPromotionById(int id, bool includePromoInfo, bool includeCustomerInfo)
		{
			if (PromotionExists(id))
			{
				if (includePromoInfo && includeCustomerInfo)
				{
					return await _ctx.Promotions.Where(p => p.PromotionAvailable && p.PromotionId == id)
												.Include(p => p.PromotionsInformation)
												.Include(p => p.PromotionCustomers)
												.Select(p => new PromotionDto
												{
													CategoryForTabId = p.CategoryForTabId,
													OldImage = p.ImagePath,
													PromotionAvailable = p.PromotionAvailable,
													PromotionCustomers = p.PromotionCustomers,
													PromotionId = p.PromotionId,
													PromotionName = p.PromotionName,
													PromotionsInformation = p.PromotionsInformation,
                                                    PromotionDescription = p.PromotionDescription,
                                                    PromotionExtra = p.PromotionExtra
                                                }).SingleOrDefaultAsync();
				}
				else if (includePromoInfo)
				{
					return await _ctx.Promotions.Where(p => p.PromotionAvailable && p.PromotionId == id)
												.Include(p => p.PromotionsInformation)
												.Select(p => new PromotionDto
												{
													CategoryForTabId = p.CategoryForTabId,
													OldImage = p.ImagePath,
													PromotionAvailable = p.PromotionAvailable,
													PromotionId = p.PromotionId,
													PromotionName = p.PromotionName,
													PromotionsInformation = p.PromotionsInformation,
                                                    PromotionDescription = p.PromotionDescription,
                                                    PromotionExtra = p.PromotionExtra
                                                }).SingleOrDefaultAsync();
				}
				else if (includeCustomerInfo)
				{
					return await _ctx.Promotions.Where(p => p.PromotionAvailable && p.PromotionId == id)
												.Include(p => p.PromotionCustomers)
												.Select(p => new PromotionDto
												{
													CategoryForTabId = p.CategoryForTabId,
													OldImage = p.ImagePath,
													PromotionAvailable = p.PromotionAvailable,
													PromotionCustomers = p.PromotionCustomers,
													PromotionId = p.PromotionId,
													PromotionName = p.PromotionName,
                                                    PromotionDescription = p.PromotionDescription,
                                                    PromotionExtra = p.PromotionExtra
                                                }).SingleOrDefaultAsync();
				}
				else
				{
					return await _ctx.Promotions.Where(p => p.PromotionAvailable && p.PromotionId == id)
												.Select(p => new PromotionDto
												{
													CategoryForTabId = p.CategoryForTabId,
													OldImage = p.ImagePath,
													PromotionAvailable = p.PromotionAvailable,
													PromotionId = p.PromotionId,
													PromotionName = p.PromotionName,
                                                    PromotionDescription = p.PromotionDescription,
                                                    PromotionExtra = p.PromotionExtra
                                                }).SingleOrDefaultAsync();
				}
			}
			return null;
		}

		public async Task<List<PromotionDto>> GetAllPromotions(bool includePromoInfo, bool includeCustomerInfo)
		{
			
			if (includePromoInfo && includeCustomerInfo)
			{
				return await _ctx.Promotions.Where(p => p.PromotionAvailable)
											.Include(p => p.PromotionsInformation)
											.Include(p => p.PromotionCustomers)
											.Select(p => new PromotionDto
											{
												CategoryForTabId = p.CategoryForTabId,
												OldImage = p.ImagePath,
												PromotionAvailable = p.PromotionAvailable,
												PromotionCustomers = p.PromotionCustomers,
												PromotionId = p.PromotionId,
												PromotionName = p.PromotionName,
												PromotionsInformation = p.PromotionsInformation,
                                                PromotionDescription = p.PromotionDescription,
                                                PromotionExtra = p.PromotionExtra
                                            }).ToListAsync();
			}
			else if (includePromoInfo)
			{
				return await _ctx.Promotions.Where(p => p.PromotionAvailable)
											.Include(p => p.PromotionsInformation)
											.Select(p => new PromotionDto
											{
												CategoryForTabId = p.CategoryForTabId,
												OldImage = p.ImagePath,
												PromotionAvailable = p.PromotionAvailable,
												PromotionId = p.PromotionId,
												PromotionName = p.PromotionName,
												PromotionsInformation = p.PromotionsInformation,
                                                PromotionDescription = p.PromotionDescription,
                                                PromotionExtra = p.PromotionExtra
                                            }).ToListAsync();
			}
			else if (includeCustomerInfo)
			{
				return await _ctx.Promotions.Where(p => p.PromotionAvailable)
											.Include(p => p.PromotionCustomers)
											.Select(p => new PromotionDto
											{
												CategoryForTabId = p.CategoryForTabId,
												OldImage = p.ImagePath,
												PromotionAvailable = p.PromotionAvailable,
												PromotionCustomers = p.PromotionCustomers,
												PromotionId = p.PromotionId,
												PromotionName = p.PromotionName,
                                                PromotionDescription = p.PromotionDescription,
                                                PromotionExtra = p.PromotionExtra
                                            }).ToListAsync();
			}
			else
			{
				return await _ctx.Promotions.Where(p => p.PromotionAvailable)
											.Select(p => new PromotionDto
											{
												CategoryForTabId = p.CategoryForTabId,
												OldImage = p.ImagePath,
												PromotionAvailable = p.PromotionAvailable,
												PromotionId = p.PromotionId,
												PromotionName = p.PromotionName,
                                                PromotionDescription = p.PromotionDescription,
                                                PromotionExtra = p.PromotionExtra
                                            }).ToListAsync();
			}
			
		}

		public async Task<bool> UpdatePromotionWithId(PromotionDto promotionDto)
		{
			if (PromotionExists(promotionDto.PromotionId))
			{
				string path = null;
				if (promotionDto.File != null)
				{
					path = _img.EditImage(promotionDto.File, promotionDto.OldImage);
				}
				Promotion promotion = await _ctx.Promotions.Where(p => p.PromotionId == promotionDto.PromotionId)
															.Select(p => new Promotion
															{
																CategoryForTabId = promotionDto.CategoryForTabId,
																ImagePath = !string.IsNullOrEmpty(path) ? path : p.ImagePath,
																PromotionAvailable = promotionDto.PromotionAvailable,
																PromotionId = p.PromotionId,
																PromotionName = promotionDto.PromotionName,
                                                                PromotionDescription = promotionDto.PromotionDescription,
                                                                PromotionExtra = promotionDto.PromotionExtra
                                                            }).SingleOrDefaultAsync();
				try
				{
					_ctx.Update(promotion);
					await _ctx.SaveChangesAsync();
					return true;
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"The promotion could not be updated {ex.Message}");
				}
				
			}
			return false;
		}

		public async Task DeletePromotionWithId(int id)
		{
			if (PromotionExists(id))
			{
				Promotion promotion = await _ctx.Promotions.FindAsync(id);
				try
				{
					_ctx.Remove(promotion);
					await _ctx.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"The promotion could not be updated {ex.Message}");

				}
			}
		}

		public bool PromotionExists(int id) => _ctx.Promotions.Any(p => p.PromotionId == id);

	}
}