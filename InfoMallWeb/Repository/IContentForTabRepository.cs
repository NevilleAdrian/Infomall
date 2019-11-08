using InfoMallWeb.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public interface IContentForTabRepository
	{
		Task<bool> AddContent(ContentForTabDto newContent);
		Task<List<ContentForTabDto>> GetAllContents(bool includeOtherImages);
		Task<ContentForTabDto> GetContentByID(int id, bool includeOtherImages);
        Task<bool> UpdateContentWithID(ContentForTabDto content);
		Task DeleteContentWithID(ContentForTabDto content);
	}
}
