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
	public class ContentForMallRepository : IContentForMallRepository
	{
		private readonly ApplicationDbContext _ctx;
		private readonly IImageService _img;
		private readonly IHostingEnvironment _env;
		private readonly ILogger<ContentForMallRepository> _logger;

		public ContentForMallRepository(ApplicationDbContext context,
			IImageService image,
			IHostingEnvironment env,
			ILogger<ContentForMallRepository> logger)
		{
			_ctx = context;
			_img = image;
			_env = env;
			_logger = logger;
		}

		public async Task<bool> AddContent(ContentForMallDto newContent)
		{
			string relativePath = null;
			if (newContent.File != null)
			{
				relativePath = _img.CreateImage(newContent.File);
			}


			ContentForMall creatingNewContent = new ContentForMall
			{
				CategoryForInformationId = newContent.CategoryId,
				DatePosted = DateTime.Now,
				Title = newContent.Title,
				ImagePath = relativePath,
				LongDescription = newContent.LongDescription,
				ShortDescription = newContent.ShortDescription,
				ShowOnHome = newContent.ShowOnHome,
				Position = newContent.ContentMallPosition,
                AuthorId = newContent.AuthorId
			};

			try
			{
				_ctx.ContentsForMall.Add(creatingNewContent);
				await _ctx.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Mall content not added: {ex.Message}");
				return false;
			}
		}

		public async Task<List<ContentForMallDto>> GetAllContents() =>
		
			await _ctx.ContentsForMall.Include(c => c.Category).Include(c => c.Author).Select(c => new ContentForMallDto {
				CategoryId = c.CategoryForInformationId,
				ContentId = c.ContentId,
				ContentMallPosition = c.Position,
				OldImage = c.ImagePath,
				LongDescription = c.LongDescription,
				ShortDescription = c.ShortDescription,
				ShowOnHome = c.ShowOnHome,
				Title = c.Title,
				CategoryForInformation = c.Category,
				DatePosted = c.DatePosted,
				NoOfViews = c.NumberOfViews,
                Author = c.Author

			}).ToListAsync();
		

		public async Task<ContentForMallDto> GetContentByID(int id, bool didViewContent)
		{
			if (ContentExists(id))
			{
				ContentForMall c = await _ctx.ContentsForMall.Where(co => co.ContentId == id).Include(co => co.Category).Include(co => co.Author).SingleOrDefaultAsync();
				ContentForMallDto contentForMall = null;
				if (didViewContent)
				{
					c.NumberOfViews++;
					try
					{
						_ctx.Update(c);
						await _ctx.SaveChangesAsync();
					}
					catch (Exception ex)
					{
						_logger.LogInformation($"Cannot update number of views: {ex.Message}");
						contentForMall = new ContentForMallDto
						{
							CategoryId = c.CategoryForInformationId,
							ContentId = c.ContentId,
							ContentMallPosition = c.Position,
							OldImage = c.ImagePath,
							LongDescription = c.LongDescription,
							ShortDescription = c.ShortDescription,
							ShowOnHome = c.ShowOnHome,
							Title = c.Title,
							CategoryForInformation = c.Category,
							DatePosted = c.DatePosted,
							NoOfViews = c.NumberOfViews,
                            Author = c.Author

                        };
						return contentForMall;
					}
				}
				contentForMall = new ContentForMallDto
				{
					CategoryId = c.CategoryForInformationId,
					ContentId = c.ContentId,
					ContentMallPosition = c.Position,
					OldImage = c.ImagePath,
					LongDescription = c.LongDescription,
					ShortDescription = c.ShortDescription,
					ShowOnHome = c.ShowOnHome,
					Title = c.Title,
					CategoryForInformation = c.Category,
					DatePosted = c.DatePosted,
					NoOfViews = c.NumberOfViews,
                    Author = c.Author

                };
				return contentForMall;
			}
			return null;

		}

		public async Task<bool> UpdateContentWithID(ContentForMallDto content)
		{
			if (ContentExists(content.ContentId))
			{
				string path = null;
				if (content.File != null)
				{
					path = _img.EditImage(content.File, content.OldImage);
				}
				var target = await _ctx.ContentsForMall.Where(c => c.ContentId == content.ContentId)
					.Select(c => new ContentForMall
					{
						CategoryForInformationId = content.CategoryId,
						ContentId = c.ContentId,
						DatePosted = c.DatePosted,
						ImagePath = !string.IsNullOrEmpty(path) ? path : c.ImagePath,
						LongDescription = !string.IsNullOrEmpty(content.LongDescription) ? content.LongDescription : c.LongDescription,
						ShortDescription = !string.IsNullOrEmpty(content.ShortDescription) ? content.ShortDescription : c.ShortDescription,
						ShowOnHome = content.ShowOnHome,
						Title = !string.IsNullOrEmpty(content.Title) ? content.Title : c.Title,
						NumberOfViews = c.NumberOfViews,
						Position = content.ContentMallPosition,
                        AuthorId = content.AuthorId
                    }).SingleOrDefaultAsync();
				if (target != null)
				{
					try
					{
						_ctx.ContentsForMall.Update(target);
						await _ctx.SaveChangesAsync();
						return true;
					}
					catch (Exception ex)
					{
						_logger.LogInformation($"Mall content not updated: {ex.Message}");
						return false;
					}

				}

			}
			return false;
		}
		
		public async Task<(bool, string)> DeleteContentWithID(ContentForMallDto content)
		{
			if (ContentExists(content.ContentId))
			{
				var target = await _ctx.ContentsForMall.Where(c => c.ContentId == content.ContentId).SingleOrDefaultAsync();
				if (target != null)
				{
					if (!string.IsNullOrEmpty(target.ImagePath))
					{
						content.ImagesToDelete.Add(target.ImagePath);
					}
					if (content.ImagesToDelete != null)
					{
						_img.DeleteImages(content.ImagesToDelete.ToList());
					}
					try
					{
						_ctx.Remove(target);
						await _ctx.SaveChangesAsync();
						return (true, "200");
					}
					catch (DbUpdateException)
					{
						return (false, "500");
					}


				}
			}
			return (false, "404");

		}

		public bool ContentExists(int id) => _ctx.ContentsForMall.Any(c => c.ContentId == id);
	}
}