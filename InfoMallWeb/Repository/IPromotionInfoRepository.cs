using InfoMallWeb.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public interface IPromotionInfoRepository
	{
		Task<PromotionInformationDto> GetPromotionInformationById(int id);
		Task<List<PromotionInformationDto>> PromotionInformation();
		Task AddPromotionInformation(PromotionInformationDto promotionInformationDto);
		Task UpdatPromotionInformationWithId(PromotionInformationDto promotionInformationDto);
		Task DeletePromotionInformationById(int id);
	}
}
