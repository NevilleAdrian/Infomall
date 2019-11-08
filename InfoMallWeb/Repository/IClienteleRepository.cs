using InfoMallWeb.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public interface IClienteleRepository
	{
		Task<ClienteleDto> GetClienteleById(int id);
		Task<List<ClienteleDto>> GetAllClientele();
		Task AddClientele(ClienteleDto clienteleDto);
		Task UpdateClienteleWithId(ClienteleDto clienteleDto);
		void DeleteClienteleWithId(int id);
	}
}
