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
	public class CategoryForInfoRepository : ICategoryForInfoRepository
	{
		private readonly ApplicationDbContext _ctx;
		private readonly ILogger<CategoryForInfoRepository> _logger;

		public CategoryForInfoRepository(ApplicationDbContext context, ILogger<CategoryForInfoRepository> logger)
		{
			_ctx = context;
			_logger = logger;
		}

		public async Task AddCategory(CategoryForInformation category)
		{
			try
			{
				_ctx.CategoriesForInformation.Add(category);
				await _ctx.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Category for Information record not created: {ex.Message}");
			}
		}

		public async Task<CategoryForInformation> GetCategoryById(int id, bool includeContents)
		{
			if (CategoryForInformationExists(id))
			{
				if (includeContents)
				{
					return await _ctx.CategoriesForInformation.Include(c => c.ContentsForInformation).ThenInclude(x => x.Author).SingleOrDefaultAsync(c => c.CategoryId == id);
				}
				return await _ctx.CategoriesForInformation.SingleOrDefaultAsync(c => c.CategoryId == id);

			}
			return null;
		}

		public async Task<List<CategoryForInformation>> GetAllCategories(bool includeContents)
		{
			
				if (includeContents)
				{
					return await _ctx.CategoriesForInformation.Include(c => c.ContentsForInformation).ThenInclude(x => x.Author).ToListAsync();
				}
				return await _ctx.CategoriesForInformation.ToListAsync();
		}

		public async Task UpdateCategoryById(CategoryForInformation category)
		{
			if (CategoryForInformationExists(category.CategoryId))
			{
				try
				{
					_ctx.CategoriesForInformation.Update(category);
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
			if (CategoryForInformationExists(id))
			{
				CategoryForInformation categoryToRemove = _ctx.CategoriesForInformation.Find(id);
				try
				{
					_ctx.CategoriesForInformation.Remove(categoryToRemove);
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

		public bool CategoryForInformationExists(int id) => _ctx.CategoriesForInformation.Any(c => c.CategoryId == id);
	}
}
