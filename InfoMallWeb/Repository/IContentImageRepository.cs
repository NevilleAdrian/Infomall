using InfoMallWeb.Dtos;
using InfoMallWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public interface IContentImageRepository
	{
		void AddContentImages(int contentForTabId, List<ContentImageDto> contentImage);
		string EditContentImage(ContentImageDto contentImage);
		bool DeleteContentImage(ContentImage contentImage);
		bool DeleteContentImages(ICollection<ContentImage> contentImages);
		bool AddContentImage(ContentImageDto contentImage);
        Task<List<ContentImage>> GetContentImages();
        Task<ContentImageDto> GetContentImageById(int id);
    }
}