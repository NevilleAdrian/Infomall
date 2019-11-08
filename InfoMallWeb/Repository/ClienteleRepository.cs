using InfoMallWeb.Data;
using InfoMallWeb.Dtos;
using InfoMallWeb.Models;
using InfoMallWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoMallWeb.Repository
{
	public class ClienteleRepository : IClienteleRepository
	{
		private readonly ApplicationDbContext _ctx;
		private readonly IImageService _img;
		private readonly IHostingEnvironment _env;
		private readonly ILogger<ClienteleRepository> _logger;

		public ClienteleRepository(ApplicationDbContext context,
			IImageService image,
			IHostingEnvironment env,
			ILogger<ClienteleRepository> logger)
		{
			_ctx = context;
			_img = image;
			_env = env;
			_logger = logger;
		}

		public async Task<ClienteleDto> GetClienteleById(int id)
		{
			if (ClienteleExists(id))
			{
				return await _ctx.Clienteles.Where(c => c.ClienteleId == id)
											.Select(c => new ClienteleDto {
												ClienteleId = c.ClienteleId,
												OldImage = c.ImageUrl,
												Priority = c.Priority
											}).SingleOrDefaultAsync();
			}
			return null;
		}
		
		public async Task<List<ClienteleDto>> GetAllClientele() =>
			await _ctx.Clienteles.Select(c => new ClienteleDto
										{
											ClienteleId = c.ClienteleId,
											OldImage = c.ImageUrl,
											Priority = c.Priority
										}).ToListAsync();

		public async Task AddClientele(ClienteleDto clienteleDto)
		{
			if (clienteleDto.File != null)
			{
				Clientele clientele = null;
				string path = null;
				try
				{
					path = _img.CreateImage(clienteleDto.File);
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Banner image not created: {ex.Message}");
				}
				if (!string.IsNullOrEmpty(path))
				{
					clientele = new Clientele
					{
						ImageUrl = path,
						Priority = clienteleDto.Priority
					};
				}
				try
				{
					_ctx.Clienteles.Add(clientele);
					await _ctx.SaveChangesAsync();
                    return;
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Clientele record not created: {ex.Message}");
				}
			}
            throw new Exception();

        }

        public async Task UpdateClienteleWithId(ClienteleDto clienteleDto)
		{
			if (ClienteleExists(clienteleDto.ClienteleId))
			{
				Clientele clientele = null;
				string path = null;
                if(clienteleDto.File != null)
                {
                    try
                    {
                        path = _img.EditImage(clienteleDto.File, clienteleDto.OldImage);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation($"Clientele image not created: {ex.Message}");
                    }
                }

				clientele = await _ctx.Clienteles.Where(c => c.ClienteleId == clienteleDto.ClienteleId)
									.Select(c => new Clientele
									{
										ClienteleId = c.ClienteleId,
										ImageUrl = !string.IsNullOrEmpty(path) ? path : c.ImageUrl,
										Priority = clienteleDto.Priority
                                    }).SingleOrDefaultAsync();

				try
				{
					_ctx.Update(clientele);
					await _ctx.SaveChangesAsync();
                    return;
				}
				catch (Exception ex)
				{
					_logger.LogInformation($"Clientele record not updated: {ex.Message}");
				}

			}
			throw new Exception();
		}

		public void DeleteClienteleWithId(int id)
		{
			if (ClienteleExists(id))
			{
				Clientele clientele = _ctx.Clienteles.Where(c => c.ClienteleId == id).SingleOrDefault();
				try
				{
                    _img.DeleteImage(clientele.ImageUrl);
                    _ctx.Clienteles.Remove(clientele);
					_ctx.SaveChanges();
                    return;
				}
				catch (Exception ex)
				{
                    
					_logger.LogInformation($"Clientele record not deleted: {ex.Message}");
				}

			}
			throw new Exception();
		}


		public bool ClienteleExists(int id) => _ctx.Clienteles.Any(c => c.ClienteleId == id);

	}
}