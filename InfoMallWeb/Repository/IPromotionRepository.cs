using InfoMallWeb.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public interface IPromotionRepository
	{
		Task<(bool, bool)> AddPromotion(PromotionDto promoDto, bool sendToAll);
		Task<PromotionDto> GetPromotionById(int id, bool includePromoInfo, bool includeCustomerInfo);
		Task<List<PromotionDto>> GetAllPromotions(bool includePromoInfo, bool includeCustomerInfo);
		Task<bool> UpdatePromotionWithId(PromotionDto promotionDto);
		Task DeletePromotionWithId(int id);
	}
}
