using DynamicPortfolioSite.Api.Controllers.Base;
using DynamicPortfolioSite.Core.Constants;
using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Entities.Models.Blog;
using DynamicPortfolioSite.Repository.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : BaseApiController
    {
        #region Ctor&Fields

        private readonly IUnitOfWork _unitOfWork;

        public BlogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region GetById

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var getData = await _unitOfWork.BlogPostRepository.GetAsync(i => i.IsActive && i.Id == id);
            if (getData != default)
            {
                var getCreatedUserData = await _unitOfWork.AppUserRepository.GetAsync(i => i.IsActive && i.Id == getData.CreatedByUserId);

                BlogModel returnModel = new()
                {
                    BlogId = getData.Id,
                    ProfileImg = getData.ProfileImg,
                    ShortDescription = getData.ShortDescription,
                    Title = getData.Title,
                    Url = getData.Url,
                    CreatedByUser = getCreatedUserData?.UserName,
                    CreatedDate = getData.CreatedDate

                };

                return Ok(returnModel);
            }
            return BadRequest(Messages.BlogDataNotFound);
        }

        #endregion

        #region List

        [HttpGet("list")]
        public async Task<IActionResult> ListAsync([FromQuery] BlogListParamsModel model)
        {
            var getListData = await _unitOfWork.BlogPostRepository.CustomSearchAsync(new BlogCustomSearchModel()
            {
                CreateDateRange_End = model.CreateDateRange_End,
                CreateDateRange_Start = model.CreateDateRange_Start
            });

            IEnumerable<BlogListModel> returnModel = getListData.Select(i => new BlogListModel()
            {
                BlogId = i.Id,
                Title = i.Title
            });

            return Ok(returnModel);
        }

        #endregion

        #region Add

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] BlogModel model)
        {
            await _unitOfWork.BlogPostRepository.AddAsync(new BlogPost()
            {
                CreatedByUserId = UserId,
                CreatedDate = DateTimeNow,
                IsActive = true,
                LocalizationId = model.LocalizationId,
                ProfileImg = model.ProfileImg,
                ShortDescription = model.ShortDescription,
                Title = model.Title,
                Url = model.Url
            });

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        #endregion

        #region Edit

        [HttpPost("edit")]
        public async Task<IActionResult> EditAsync([FromBody] BlogModel model)
        {
            var getOldData = await _unitOfWork.BlogPostRepository.GetAsync(i => i.IsActive && i.Id == model.BlogId);
            if (getOldData == default)
            {
                return BadRequest(Messages.BlogDataNotFound);
            }

            if (BlogDataIsChange(ref getOldData,ref model))
            {
                getOldData.UpdatedByUserId = UserId;
                getOldData.UpdatedDate = DateTimeNow;

                getOldData.LocalizationId = model.LocalizationId;
                getOldData.ProfileImg = model.ProfileImg;
                getOldData.ShortDescription = model.ShortDescription;
                getOldData.Title = model.Title;
                getOldData.Url = model.Url;

                await _unitOfWork.BlogPostRepository.UpdateAsync(getOldData);
                await _unitOfWork.CommitAsync();
            }

            return Ok();
        }

        #endregion

        #region Delete

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var getOldData = await _unitOfWork.BlogPostRepository.GetAsync(i => i.IsActive && i.Id == id);
            if (getOldData == default)
            {
                return BadRequest(Messages.BlogDataNotFound);
            }
            
            getOldData.IsActive = false;
            getOldData.UpdatedByUserId = UserId;
            getOldData.UpdatedDate = DateTimeNow;

            await _unitOfWork.BlogPostRepository.UpdateAsync(getOldData);
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        #endregion

        #region Functions

        private static bool BlogDataIsChange(ref BlogPost entity, ref BlogModel model)
        {
            if (entity.ProfileImg != model.ProfileImg || entity.LocalizationId != model.LocalizationId || entity.ShortDescription != model.ShortDescription || entity.Title != model.Title || entity.Url != model.Url)
            {
                return true;
            }

            return false;
        }


        #endregion

    }
}
