using InfoMallWeb.Data;
using InfoMallWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public class CategoryForTabRepository : ICategoryForTabRepository
	{
		private readonly ApplicationDbContext _ctx;
		private readonly ILogger<CategoryForTabRepository> _logger;

		public CategoryForTabRepository(ApplicationDbContext context, ILogger<CategoryForTabRepository> logger)
		{
			_ctx = context;
			_logger = logger;
		}

		public async Task AddCategory(CategoryForTab category)
		{
			try
			{
				_ctx.CategoriesForTab.Add(category);
				await _ctx.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Category for Information record not created: {ex.Message}");
			}
		}

		public async Task<CategoryForTab> GetCategoryById(int id, bool includeContents, bool includeBanners)
		{
			if (CategoryForTabExists(id))
			{
				if (includeContents && includeBanners)
				{
					return await _ctx.CategoriesForTab.Include(c => c.ContentForTabs)
                        .ThenInclude(o => o.ContentImages).Include(c => c.Banners).SingleOrDefaultAsync(c => c.CategoryId == id);
				}
				else if (includeContents)
				{
					return await _ctx.CategoriesForTab.Include(c => c.ContentForTabs).ThenInclude(o => o.ContentImages)
                        .SingleOrDefaultAsync(c => c.CategoryId == id);

				}
				else if (includeBanners)
				{
					return await _ctx.CategoriesForTab.Include(c => c.Banners).SingleOrDefaultAsync(c => c.CategoryId == id);

				}
				return await _ctx.CategoriesForTab.SingleOrDefaultAsync(c => c.CategoryId == id);

			}
			return null;
		}

		public async Task<CategoryForTab> GetCategoryById(int id)
		{
			if (CategoryForTabExists(id))
			{
				
				return await _ctx.CategoriesForTab.Include(c => c.ContentForTabs).ThenInclude(o => o.ContentImages).Include(c => c.Promotions).ThenInclude(p => p.PromotionsInformation).Include(c => c.Banners).SingleOrDefaultAsync(c => c.CategoryId == id);

			}
			return null;
		}

		public async Task<List<CategoryForTab>> GetAllCategories(bool includeContents, bool includeBanners)
		{

			if (includeContents && includeBanners)
			{
				return await _ctx.CategoriesForTab.Include(c => c.ContentForTabs).Include(c => c.Banners).ToListAsync();
			}
			else if (includeContents)
			{
				return await _ctx.CategoriesForTab.Include(c => c.ContentForTabs).ToListAsync();

			}
			else if (includeBanners)
			{
				return await _ctx.CategoriesForTab.Include(c => c.Banners).ToListAsync();
			}
			return await _ctx.CategoriesForTab.ToListAsync();
		}
		public async Task<List<CategoryForTab>> GetAllCategories()
		{
			return await _ctx.CategoriesForTab.Include(c => c.ContentForTabs).Include(c => c.Promotions).Include(c => c.Banners).ToListAsync();
		}
		public async Task UpdateCategoryById(CategoryForTab category)
		{
			if (CategoryForTabExists(category.CategoryId))
			{
				try
				{
					_ctx.CategoriesForTab.Update(category);
					await _ctx.SaveChangesAsync();
                    return;
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Category record not updated: {ex.Message}");
				}

			}
			throw new Exception();
		}

		public void DeleteCategoryById(int id)
		{
			if (CategoryForTabExists(id))
			{
				CategoryForTab categoryToRemove = _ctx.CategoriesForTab.Find(id);
				try
				{
					_ctx.CategoriesForTab.Remove(categoryToRemove);
					_ctx.SaveChanges();
                    return;
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Category record not deleted: {ex.Message}");
				}

			}
			throw new Exception();
		}

		public bool CategoryForTabExists(int id) => _ctx.CategoriesForTab.Any(c => c.CategoryId == id);
	}
}
