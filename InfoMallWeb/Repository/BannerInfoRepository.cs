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
	public class BannerInfoRepository : IBannerRepository
	{
		private readonly ApplicationDbContext _ctx;
		private readonly IImageService _img;
		private readonly IHostingEnvironment _env;
		private readonly ILogger<BannerInfoRepository> _logger;

		public BannerInfoRepository(ApplicationDbContext context,
			IImageService image, 
			IHostingEnvironment env,
			ILogger<BannerInfoRepository> logger)
		{
			_ctx = context;
			_img = image;
			_env = env;
			_logger = logger;
		}

		public async Task<BannerInformationDto> GetBannerByCategoryId(int id)
		{
			if (CategoryExists(id))
			{
				return await _ctx.BannersInformation.Where(b => b.CategoryForTabId == id && !b.ShowBannerOnHome)
													.Include(b => b.Category)
													.Select(b => new BannerInformationDto {
														BannerContent = b.BannerContent,
														BannerId = b.BannerId,
														CategoryForTabId = b.CategoryForTabId,
														OldImage = b.ImageUrl,
														Category = b.Category,
														ExtraInformation = b.ExtraInformation,
														ShowBannerOnHome = b.ShowBannerOnHome
													}).SingleOrDefaultAsync();
			}
			return null;
		}

		public async Task<BannerInformationDto> GetBannerByBannerId(int id)
		{
			if (BannerExists(id))
			{
				return await _ctx.BannersInformation.Where(b => b.BannerId == id)
													.Include(b => b.Category)
													.Select(b => new BannerInformationDto
													{
														BannerContent = b.BannerContent,
														BannerId = b.BannerId,
														CategoryForTabId = b.CategoryForTabId,
														OldImage = b.ImageUrl,
														Category = b.Category,
														ExtraInformation = b.ExtraInformation,
														ShowBannerOnHome = b.ShowBannerOnHome
													}).SingleOrDefaultAsync();
			}
			return null;
		}

		public async Task<List<BannerInformationDto>> GetAllBanners() =>
			await _ctx.BannersInformation.Include(b => b.Category)
													.Select(b => new BannerInformationDto
													{
														BannerContent = b.BannerContent,
														BannerId = b.BannerId,
														CategoryForTabId = b.CategoryForTabId,
														OldImage = b.ImageUrl,
														Category = b.Category,
														ExtraInformation = b.ExtraInformation,
														ShowBannerOnHome = b.ShowBannerOnHome
													}).ToListAsync();

		public async Task AddBanner(BannerInformationDto banner)
		{
			if (banner.File != null)
			{
				BannerInformation bannerToAdd = null;
				string path = null;
				try
				{
					path = _img.CreateImage(banner.File);
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Banner image not created: {ex.Message}");
				}
				if (!string.IsNullOrEmpty(path))
				{
					bannerToAdd = new BannerInformation {
						BannerContent = banner.BannerContent,
						ImageUrl = path,
						CategoryForTabId = banner.CategoryForTabId,
						ExtraInformation = banner.ExtraInformation,
						ShowBannerOnHome = banner.ShowBannerOnHome
					};
				}
				try
				{
					_ctx.BannersInformation.Add(bannerToAdd);
					await _ctx.SaveChangesAsync();
				}
				catch(Exception ex)
				{
					_logger.LogInformation($"Banner record not created: {ex.Message}");
				}
			}
		}

		public async Task UpdateBannerWithId(BannerInformationDto banner)
		{
			if (BannerExists(banner.BannerId))
			{
				BannerInformation bannerToUpdate = null;
				string path = null;
				try
				{
					path = _img.EditImage(banner.File, banner.OldImage);
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Banner image not created: {ex.Message}");
				}

				bannerToUpdate = await _ctx.BannersInformation.Where(b => b.BannerId == banner.BannerId).Select(b => new BannerInformation
				{
					BannerContent = !string.IsNullOrEmpty(banner.BannerContent) ? banner.BannerContent : b.BannerContent,
					ImageUrl = !string.IsNullOrEmpty(path) ? path : b.ImageUrl,
					CategoryForTabId = banner.CategoryForTabId,
					BannerId = b.BannerId,
					ExtraInformation = banner.ExtraInformation,
					ShowBannerOnHome = banner.ShowBannerOnHome

				}).SingleOrDefaultAsync();

				try
				{
					_ctx.BannersInformation.Update(bannerToUpdate);
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

		public void DeleteBanner(int id)
		{
			if (BannerExists(id))
			{
				BannerInformation bannerToRemove = _ctx.BannersInformation.Find(id);
				try
				{
					_ctx.BannersInformation.Remove(bannerToRemove);
					 _ctx.SaveChanges();
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Banner record not deleted: {ex.Message}");
				}

			}
		}


		public bool CategoryExists(int id) => _ctx.CategoriesForTab.Any(c => c.CategoryId == id);
		public bool BannerExists(int id) => _ctx.BannersInformation.Any(b => b.BannerId == id);
	}
}
