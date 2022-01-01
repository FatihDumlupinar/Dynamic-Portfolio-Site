using DynamicPortfolioSite.Api.Controllers.Base;
using DynamicPortfolioSite.Core.Constants;
using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Entities.Models.Category;
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
    public class ProjectCategoriesController : BaseApiController
    {
        #region Ctor&Fields

        private readonly IUnitOfWork _unitOfWork;

        public ProjectCategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region GetById

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var getCategoryData = await _unitOfWork.CategoryRepository.GetAsync(i => i.IsActive && i.Id == id);
            if (getCategoryData == default)
            {
                return BadRequest(Messages.ProjectDataNotFound);
            }

            CategoryModel returnModel = new()
            {
                CategoryId = getCategoryData.Id,
                CategoryName = getCategoryData.Name

            };

            return Ok(returnModel);
        }

        #endregion

        #region List

        [HttpGet("list")]
        public async Task<IActionResult> ListAsync()
        {
            var getListData = await _unitOfWork.CategoryRepository.GetListAsync(i => i.IsActive);

            IEnumerable<CategoryListModel> returnModel = getListData.Select(i => new CategoryListModel()
            {
                CategoryId = i.Id,
                CategoryName = i.Name
            });

            return Ok(returnModel);
        }

        #endregion

        #region Add

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] CategoryModel model)
        {
            await _unitOfWork.CategoryRepository.AddAsync(new Category()
            {
                CreatedByUserId = UserId,
                CreatedDate = DateTimeNow,
                IsActive = true,
                LocalizationId = model.LocalizationId,
                Name = model.CategoryName
            });

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        #endregion

        #region Edit

        [HttpPost("edit")]
        public async Task<IActionResult> EditAsync([FromBody] CategoryModel model)
        {
            Category getOldCategoryModel = await _unitOfWork.CategoryRepository.GetAsync(i => i.IsActive && i.Id == model.CategoryId);
            if (getOldCategoryModel == default)
            {
                return BadRequest(Messages.ProjectDataNotFound);
            }

            if (CheckCategoryDataIsChange(ref getOldCategoryModel, ref model))
            {
                getOldCategoryModel.LocalizationId = model.LocalizationId;
                getOldCategoryModel.Name = model.CategoryName;

                getOldCategoryModel.UpdatedByUserId = UserId;
                getOldCategoryModel.UpdatedDate = DateTimeNow;

                await _unitOfWork.CategoryRepository.UpdateAsync(getOldCategoryModel);
            }

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        #endregion

        #region Delete

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            #region Category
            
            Category getOldCategoryModel = await _unitOfWork.CategoryRepository.GetAsync(i => i.IsActive && i.Id == id);
            if (getOldCategoryModel == default)
            {
                return BadRequest(Messages.ProjectDataNotFound);
            }

            getOldCategoryModel.IsActive = false;
            await _unitOfWork.CategoryRepository.UpdateAsync(getOldCategoryModel);

            #endregion

            #region ProjectAndCategory

            var getAllOldProjectAndCategoryData = await _unitOfWork.ProjectAndCategoryRepository.GetListAsync(i => i.IsActive && i.ProjectId == id);

            if (getAllOldProjectAndCategoryData.Any())
            {
                var editProjectAndCategoryListModel = getAllOldProjectAndCategoryData.Select(i => new ProjectAndCategory()
                {
                    CategoryId = i.CategoryId,
                    CreatedByUserId = i.CreatedByUserId,
                    CreatedDate = i.CreatedDate,
                    IsActive = false,
                    LocalizationId = i.LocalizationId,
                    Id = i.Id,
                    ProjectId = i.ProjectId,
                    UpdatedByUserId = UserId,
                    UpdatedDate = DateTimeNow

                });

                await _unitOfWork.ProjectAndCategoryRepository.UpdateAllAsync(editProjectAndCategoryListModel);
            }

            #endregion

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        #endregion

        #region Functions

        private static bool CheckCategoryDataIsChange(ref Category entity, ref CategoryModel model)
        {
            if (entity.LocalizationId != model.LocalizationId || entity.Name != model.CategoryName)
            {
                return true;
            }
            return false;
        }


        #endregion

    }
}
