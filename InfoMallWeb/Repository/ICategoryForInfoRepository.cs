using InfoMallWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public interface ICategoryForInfoRepository
	{
		Task AddCategory(CategoryForInformation category);
		Task<CategoryForInformation> GetCategoryById(int id, bool includeContents);
		Task<List<CategoryForInformation>> GetAllCategories(bool includeContents);
		Task UpdateCategoryById(CategoryForInformation category);
		void DeleteCategoryById(int id);
	}
}
