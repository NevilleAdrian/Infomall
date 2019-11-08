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
	public class ContentForTabRepository : IContentForTabRepository
	{
		private readonly ApplicationDbContext _ctx;
		private readonly IImageService _img;
		private readonly IHostingEnvironment _env;
		private readonly ILogger<ContentForTabRepository> _logger;
		private readonly IContentImageRepository _conImage;

		public ContentForTabRepository(ApplicationDbContext context,
			IImageService image,
			IHostingEnvironment env,
			ILogger<ContentForTabRepository> logger,
			IContentImageRepository conImage)
		{
			_ctx = context;
			_img = image;
			_env = env;
			_logger = logger;
			_conImage = conImage;
		}

		public async Task<bool> AddContent(ContentForTabDto newContent)
		{
			string relativePath = null;
			if (newContent.File != null)
			{
				relativePath = _img.CreateImage(newContent.File);
			}

			

			ContentForTab creatingNewContent = new ContentForTab
			{
				CategoryForTabId = newContent.CategoryId,
				Title = newContent.Title,
				ImagePath = relativePath,
				LongDescription = newContent.LongDescription,
				ShortDescription = newContent.ShortDescription,
				ShowOnHome = newContent.ShowOnHome,
				Position = newContent.ContentPosition
			};

			try
			{
				_ctx.ContentsForTab.Add(creatingNewContent);
				await _ctx.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Tab content not added: {ex.Message}");
				return false;
			}
			if (newContent.OtherImage != null)
			{
				List<ContentImageDto> contentImages = new List<ContentImageDto>();
				for (int i = 0; i < newContent.OtherImage.Count; i++)
				{
					contentImages.Add(new ContentImageDto
					{
						File = newContent.OtherImage.ElementAt(i),
						Description = newContent.DescriptionForImage.ElementAt(i),
						ExtraData = newContent.ExtraData.ElementAt(i)
					});
				}
				try
				{
					_conImage.AddContentImages(creatingNewContent.ContentId, contentImages);
				}
				catch(Exception ex)
				{
					_logger.LogInformation($"Could not create other images: {ex.Message}");
				}
			}
			return true;
		}

		public async Task<List<ContentForTabDto>> GetAllContents(bool includeOtherImages)
		{
			if (includeOtherImages)
			{
				return await _ctx.ContentsForTab.Include(c => c.Category)
					.Include(c => c.ContentImages)
					.Select(c => new ContentForTabDto
					{
						CategoryId = c.CategoryForTabId,
						ContentId = c.ContentId,
						ContentPosition = c.Position,
						OldImage = c.ImagePath,
						LongDescription = c.LongDescription,
						ShortDescription = c.ShortDescription,
						ShowOnHome = c.ShowOnHome,
						Title = c.Title,
						CategoryForTab = c.Category,
						ContentImages = c.ContentImages

					}).ToListAsync();
			}
			return await _ctx.ContentsForTab.Include(c => c.Category)
					.Select(c => new ContentForTabDto
					{
						CategoryId = c.CategoryForTabId,
						ContentId = c.ContentId,
						ContentPosition = c.Position,
						OldImage = c.ImagePath,
						LongDescription = c.LongDescription,
						ShortDescription = c.ShortDescription,
						ShowOnHome = c.ShowOnHome,
						Title = c.Title,
						CategoryForTab = c.Category

					}).ToListAsync();
		}

			


		public async Task<ContentForTabDto> GetContentByID(int id, bool includeOtherImages)
		{
			if (ContentExists(id))
			{
				if (includeOtherImages)
				{
					return await _ctx.ContentsForTab.Where(c => c.ContentId == id)
						.Include(c => c.ContentImages)
						.Include(c => c.Category).Select(c => new ContentForTabDto
						{
							CategoryId = c.CategoryForTabId,
							ContentId = c.ContentId,
							ContentPosition = c.Position,
							OldImage = c.ImagePath,
							LongDescription = c.LongDescription,
							ShortDescription = c.ShortDescription,
							ShowOnHome = c.ShowOnHome,
							Title = c.Title,
							CategoryForTab = c.Category,
							ContentImages = c.ContentImages

						}).SingleOrDefaultAsync();
				}
				return await _ctx.ContentsForTab.Where(c => c.ContentId == id).Include(c => c.Category).Select(c => new ContentForTabDto
				{
					CategoryId = c.CategoryForTabId,
					ContentId = c.ContentId,
					ContentPosition = c.Position,
					OldImage = c.ImagePath,
					LongDescription = c.LongDescription,
					ShortDescription = c.ShortDescription,
					ShowOnHome = c.ShowOnHome,
					Title = c.Title,
					CategoryForTab = c.Category

				}).SingleOrDefaultAsync();
			}
			return null;

		}

		public async Task<bool> UpdateContentWithID(ContentForTabDto content)
		{
			if (ContentExists(content.ContentId))
			{
				string path = null;
				if (content.File != null)
				{
					path = _img.EditImage(content.File, content.OldImage);
				}
				var target = await _ctx.ContentsForTab.Where(c => c.ContentId == content.ContentId)
					.Select(c => new ContentForTab
					{
						CategoryForTabId = content.CategoryId,
						ContentId = c.ContentId,
						ImagePath = !string.IsNullOrEmpty(path) ? path : c.ImagePath,
						LongDescription = !string.IsNullOrEmpty(content.LongDescription) ? content.LongDescription : c.LongDescription,
						ShortDescription = !string.IsNullOrEmpty(content.ShortDescription) ? content.ShortDescription : c.ShortDescription,
						ShowOnHome = content.ShowOnHome,
						Title = !string.IsNullOrEmpty(content.Title) ? content.Title : c.Title,
						Position = content.ContentPosition
					}).SingleOrDefaultAsync();
				if (target != null)
				{
					try
					{
						_ctx.ContentsForTab.Update(target);
						await _ctx.SaveChangesAsync();
					}
					catch (Exception ex)
					{
						_logger.LogInformation($"tab content not updated: {ex.Message}");
						return false;
					}
                    if (content.OtherImage != null)
                    {
                        List<ContentImageDto> contentImages = new List<ContentImageDto>();
                        for (int i = 0; i < content.OtherImage.Count; i++)
                        {
                            contentImages.Add(new ContentImageDto
                            {
                                File = content.OtherImage.ElementAt(i),
                                Description = content.DescriptionForImage.ElementAt(i),
                                ExtraData = content.ExtraData.ElementAt(i)
                            });
                        }
                        try
                        {
                            _conImage.AddContentImages(content.ContentId, contentImages);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation($"Could not create other images: {ex.Message}");
                        }
                    }
                    return true;
                }

			}
			return false;
		}

		public async Task DeleteContentWithID(ContentForTabDto content)
		{
			if (ContentExists(content.ContentId))
			{
				var target = await _ctx.ContentsForTab.Where(c => c.ContentId == content.ContentId).SingleOrDefaultAsync();
				if (target != null)
				{
					if (!string.IsNullOrEmpty(target.ImagePath))
					{
                        if(content.ImagesToDelete == null)
                        {
                            content.ImagesToDelete = new List<string>();
                        }
						content.ImagesToDelete.Add(target.ImagePath);
					}
					if (content.ImagesToDelete != null)
					{
						_img.DeleteImages(content.ImagesToDelete.ToList());
					}
					try
					{
						_ctx.ContentsForTab.Remove(target);
						await _ctx.SaveChangesAsync();
						
					}
					catch (DbUpdateException ex)
					{
						_logger.LogInformation($"Error in deleting information: {ex.Message}");
					}


				}
			}

		}

		public bool ContentExists(int id) => _ctx.ContentsForTab.Any(c => c.ContentId == id);
	}
}
