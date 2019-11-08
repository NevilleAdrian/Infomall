using InfoMallWeb.Data;
using InfoMallWeb.Dtos;
using InfoMallWeb.Models;
using InfoMallWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public class PromotionInfoRepository : IPromotionInfoRepository
	{
		private readonly ApplicationDbContext _ctx;
		private readonly IImageService _img;
		private readonly IHostingEnvironment _env;
		private readonly ILogger<PromotionInfoRepository> _logger;

		public PromotionInfoRepository(ApplicationDbContext context,
			IImageService image,
			IHostingEnvironment env,
			ILogger<PromotionInfoRepository> logger)
		{
			_ctx = context;
			_img = image;
			_env = env;
			_logger = logger;
		}

		public async Task<PromotionInformationDto> GetPromotionInformationById(int id)
		{
			if (PromotionInformationExists(id))
			{
				return await _ctx.PromotionsInformation.Include(p => p.Promotion).Where(b => b.PromotionInformationId == id)
														.Select(p => new PromotionInformationDto {
															OldImage = p.ImagePath,
															Promotion = p.Promotion,
															PromotionInformationId = p.PromotionInformationId,
															PromotionId = p.PromotionId,
															PromotionInformationContent = p.PromotionInformationContent,
															Title = p.Title,
                                                            Price = p.Price
														}).SingleOrDefaultAsync();
			}
			return null;
		}

		public async Task<List<PromotionInformationDto>> PromotionInformation() =>
			await _ctx.PromotionsInformation.Include(p => p.Promotion)
													.Select(p => new PromotionInformationDto {
														OldImage = p.ImagePath,
														Promotion = p.Promotion,
														PromotionInformationId = p.PromotionInformationId,
														PromotionId = p.PromotionId,
														PromotionInformationContent = p.PromotionInformationContent,
														Title = p.Title,
                                                        Price = p.Price
                                                    }).ToListAsync();

		public async Task AddPromotionInformation(PromotionInformationDto promotionInformationDto)
		{
			if (promotionInformationDto.File != null)
			{
				PromotionInformation promotionInformation = null;
				string path = null;
				try
				{
					path = _img.CreateImage(promotionInformationDto.File);
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Banner image not created: {ex.Message}");
				}

				promotionInformation = new PromotionInformation
				{
					ImagePath = path,
					PromotionId = promotionInformationDto.PromotionId,
					PromotionInformationContent = promotionInformationDto.PromotionInformationContent,
					Title = promotionInformationDto.Title,
                    Price = promotionInformationDto.Price
                };
				
				try
				{
					_ctx.PromotionsInformation.Add(promotionInformation);
					await _ctx.SaveChangesAsync();
                    return;
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Banner record not created: {ex.Message}");
				}
			}
			throw new Exception();
		}

		public async Task UpdatPromotionInformationWithId(PromotionInformationDto promotionInformationDto)
		{
			if (PromotionInformationExists(promotionInformationDto.PromotionInformationId))
			{
				PromotionInformation promotionInformation = null;
				string path = null;
				try
				{
					path = _img.EditImage(promotionInformationDto.File, promotionInformationDto.OldImage);
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Product information image not created: {ex.Message}");
				}

				promotionInformation = await _ctx.PromotionsInformation.Where(p => p.PromotionInformationId == promotionInformationDto.PromotionInformationId)
																	.Select(p => new PromotionInformation
																	{
																		ImagePath = !string.IsNullOrEmpty(path) ? path : p.ImagePath,
																		PromotionId = promotionInformationDto.PromotionId,
																		PromotionInformationId = p.PromotionInformationId,
																		PromotionInformationContent = promotionInformationDto.PromotionInformationContent,
																		Title = promotionInformationDto.Title,
                                                                        Price = promotionInformationDto.Price
                                                                    }).SingleOrDefaultAsync();

				try
				{
					_ctx.Update(promotionInformation);
					await _ctx.SaveChangesAsync();
                    return;
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Promotion information record not updated: {ex.Message}");
				}

			}
			throw new Exception();
		}

		public async Task DeletePromotionInformationById(int id)
		{
			if (PromotionInformationExists(id))
			{
				PromotionInformation promoInfo = await _ctx.PromotionsInformation.FindAsync(id);
				try
				{
					_ctx.PromotionsInformation.Remove(promoInfo);
					await _ctx.SaveChangesAsync();
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Promotion information record not deleted: {ex.Message}");
				}

			}
			throw new Exception();
		}

		
		public bool PromotionInformationExists(int id) => _ctx.PromotionsInformation.Any(p => p.PromotionInformationId == id);
	}
}