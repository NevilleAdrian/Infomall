using InfoMallWeb.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public interface IContentForMallRepository
	{
		Task<bool> AddContent(ContentForMallDto newContent);
		Task<List<ContentForMallDto>> GetAllContents();
		Task<ContentForMallDto> GetContentByID(int id, bool didViewContent);
		Task<bool> UpdateContentWithID(ContentForMallDto content);
		Task<(bool, string)> DeleteContentWithID(ContentForMallDto content);
	}
}
