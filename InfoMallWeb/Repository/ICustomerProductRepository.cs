using InfoMallWeb.Dtos;
using InfoMallWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public interface ICustomerProductRepository
	{
		Task<(bool, bool)> AddCustomerProduct(CustumerProductDto customerProductDto);
		Task<CustomerProduct> GetCustomerProductById(int id);
		Task<List<CustomerProduct>> GetAllCustomerProducts();
		Task UpdateCustomerProductWithId(CustomerProduct customerProduct);
		void DeleteCustomerProductWithId(int id);
	}
}
