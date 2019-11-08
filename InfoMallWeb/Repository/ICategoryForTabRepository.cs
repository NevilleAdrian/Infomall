using InfoMallWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public interface ICategoryForTabRepository
	{
		Task AddCategory(CategoryForTab category);
		Task<CategoryForTab> GetCategoryById(int id, bool includeContents, bool includeBanners);
		Task<List<CategoryForTab>> GetAllCategories(bool includeContents, bool includeBanners);
		Task<CategoryForTab> GetCategoryById(int id);
		Task<List<CategoryForTab>> GetAllCategories();
		Task UpdateCategoryById(CategoryForTab category);
		void DeleteCategoryById(int id);
	}
}
