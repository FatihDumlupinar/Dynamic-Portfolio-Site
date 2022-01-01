using DynamicPortfolioSite.Api.Controllers.Base;
using DynamicPortfolioSite.Core.Constants;
using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Entities.Models.Category;
using DynamicPortfolioSite.Entities.Models.Project;
using DynamicPortfolioSite.Repository.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : BaseApiController
    {
        #region Ctor&Fields

        private readonly IUnitOfWork _unitOfWork;

        public ProjectsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region GetById

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var getProjectData = await _unitOfWork.ProjectRepository.GetAsync(i => i.IsActive && i.Id == id);

            if (getProjectData != default)
            {
                List<CategoryModel> categories = new();

                var getAllProjectAndCategoryData = await _unitOfWork.ProjectAndCategoryRepository.GetListAsync(i => i.IsActive && i.ProjectId == id);
                if (getAllProjectAndCategoryData.Any())
                {
                    var onlyCategoryIds = getAllProjectAndCategoryData.Select(i => i.CategoryId);

                    var getAllCategoryData = await _unitOfWork.CategoryRepository.GetListAsync(i => i.IsActive && onlyCategoryIds.Contains(i.Id));

                    categories = getAllCategoryData.Select(i => new CategoryModel()
                    {
                        CategoryId = i.Id,
                        CategoryName = i.Name

                    }).ToList();
                }

                var getCreatedUserData = await _unitOfWork.AppUserRepository.GetAsync(i => i.IsActive && i.Id == getProjectData.CreatedByUserId);

                ProjectModel returnModel = new()
                {
                    Categories = categories,
                    Description = getProjectData.Description,
                    ProjectName = getProjectData.Name,
                    Img = getProjectData.Img,
                    SrcUrl = getProjectData.SrcUrl,
                    CreatedByUser = getCreatedUserData?.UserName,
                    CreatedDate = getProjectData.CreatedDate,
                    ProjectId = getProjectData.Id,
                    LocalizationId = getProjectData.LocalizationId

                };

                return Ok(returnModel);
            }

            return BadRequest(Messages.ProjectDataNotFound);
        }

        #endregion

        #region List

        [HttpGet("list")]
        public async Task<IActionResult> ListAsync([FromQuery] ProjectListParamsModel model)
        {
            var getListData = await _unitOfWork.ProjectRepository.CustomSearchAsync(new ProjectCustomSearchModel()
            {
                CreateDateRange_End = model.CreateDateRange_End,
                CreateDateRange_Start = model.CreateDateRange_Start
            });

            IEnumerable<ProjectListModel> returnModel = getListData.Select(i => new ProjectListModel()
            {
                ProjectId = i.Id,
                ProjectName = i.Name
            });

            return Ok(returnModel);
        }

        #endregion

        #region Add

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] ProjectModel model)
        {
            if (CheckNewCategoryFromModel(ref model))
            {
                var addCategoryListModel = model.Categories.Where(i => i.CategoryId == 0).Select(x => new Category()
                {
                    CreatedByUserId = UserId,
                    CreatedDate = DateTimeNow,
                    IsActive = true,
                    LocalizationId = model.LocalizationId,
                    Name = x.CategoryName

                });

                var returnEntities = await _unitOfWork.CategoryRepository.AddAllAsyncReturnEntities(addCategoryListModel);
                await _unitOfWork.CommitAsync();

                model.Categories.RemoveAll(i => i.CategoryId == 0);

                model.Categories.AddRange(returnEntities.Select(i => new CategoryModel()
                {
                    CategoryId = i.Id,
                    CategoryName = i.Name
                }));
            }

            var projectEntity = await _unitOfWork.ProjectRepository.AddAsyncReturnEntity(new Project()
            {
                CreatedByUserId = UserId,
                CreatedDate = DateTimeNow,
                Description = model.Description,
                Img = model.Img,
                IsActive = true,
                LocalizationId = model.LocalizationId,
                Name = model.ProjectName,
                SrcUrl = model.SrcUrl

            });

            await _unitOfWork.CommitAsync();

            if (model.Categories.Any())
            {
                await _unitOfWork.ProjectAndCategoryRepository.AddAllAsync(model.Categories.Select(i => new ProjectAndCategory()
                {
                    CategoryId = i.CategoryId,
                    CreatedByUserId = UserId,
                    CreatedDate = DateTimeNow,
                    IsActive = true,
                    LocalizationId = model.LocalizationId,
                    ProjectId = projectEntity.Id

                }));

                await _unitOfWork.CommitAsync();
            }

            return Ok();
        }

        #endregion

        #region Edit

        [HttpPost("edit")]
        public async Task<IActionResult> EditAsync([FromBody] ProjectModel model)
        {
            Project getOldProjectModel = await _unitOfWork.ProjectRepository.GetAsync(i => i.IsActive && i.Id == model.ProjectId);
            if (getOldProjectModel == default)
            {
                return BadRequest(Messages.ProjectDataNotFound);
            }

            if (CheckProjectDataIsChange(ref getOldProjectModel, ref model))
            {
                getOldProjectModel.Description = model.Description;
                getOldProjectModel.Img = model.Img;
                getOldProjectModel.LocalizationId = model.LocalizationId;
                getOldProjectModel.Name = model.ProjectName;
                getOldProjectModel.SrcUrl = model.SrcUrl;

                getOldProjectModel.UpdatedByUserId = UserId;
                getOldProjectModel.UpdatedDate = DateTimeNow;

                await _unitOfWork.ProjectRepository.UpdateAsync(getOldProjectModel);
            }

            if (CheckNewCategoryFromModel(ref model))
            {
                var addCategoryListModel = model.Categories.Where(i => i.CategoryId == 0).Select(x => new Category()
                {
                    CreatedByUserId = UserId,
                    CreatedDate = DateTimeNow,
                    IsActive = true,
                    LocalizationId = model.LocalizationId,
                    Name = x.CategoryName

                });

                var returnEntities = await _unitOfWork.CategoryRepository.AddAllAsyncReturnEntities(addCategoryListModel);
                await _unitOfWork.CommitAsync();

                model.Categories.RemoveAll(i => i.CategoryId == 0);

                model.Categories.AddRange(returnEntities.Select(i => new CategoryModel()
                {
                    CategoryId = i.Id,
                    CategoryName = i.Name
                }));
            }

            List<ProjectAndCategory> getAllProjectAndCategoryData = await _unitOfWork.ProjectAndCategoryRepository.GetListAsync(i => i.IsActive && i.ProjectId == model.ProjectId);

            var onlyCategoryIdsFromDb = getAllProjectAndCategoryData.Select(i => i.CategoryId);

            var onlyCategoryIdsFromModel = model.Categories.Select(i => i.CategoryId);

            #region veritabanında olan ama model den gelmeyen (silinecek)

            var diff_FromDb = onlyCategoryIdsFromDb.Except(onlyCategoryIdsFromModel);
            if (diff_FromDb.Any())
            {
                var editProjectAndCategoryListModel = getAllProjectAndCategoryData.Where(i => diff_FromDb.Contains(i.CategoryId)).Select(i => new ProjectAndCategory()
                {
                    CategoryId = i.CategoryId,
                    CreatedByUserId = i.CreatedByUserId,
                    CreatedDate = i.CreatedDate,
                    IsActive = false,
                    LocalizationId = i.LocalizationId,
                    ProjectId = i.ProjectId,
                    Id = i.Id,
                    UpdatedByUserId = UserId,
                    UpdatedDate = DateTimeNow
                });

                await _unitOfWork.ProjectAndCategoryRepository.UpdateAllAsync(editProjectAndCategoryListModel);
            }

            #endregion

            #region model den gelen ama veritabanında olmayan (eklenecek)

            var diff_FromModel = onlyCategoryIdsFromModel.Except(onlyCategoryIdsFromDb);
            if (diff_FromModel.Any())
            {
                var addProjectAndCategoryListModel = model.Categories.Where(i => diff_FromModel.Contains(i.CategoryId)).Select(i => new ProjectAndCategory()
                {
                    CategoryId = i.CategoryId,
                    CreatedByUserId = UserId,
                    CreatedDate = DateTimeNow,
                    IsActive = true,
                    LocalizationId = model.LocalizationId,
                    ProjectId = model.ProjectId
                });

                await _unitOfWork.ProjectAndCategoryRepository.AddAllAsync(addProjectAndCategoryListModel);
            }

            #endregion

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        #endregion

        #region Delete

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            #region Project
            
            var getOldProjectData = await _unitOfWork.ProjectRepository.GetAsync(i => i.IsActive && i.Id == id);
            if (getOldProjectData == default)
            {
                return BadRequest(Messages.BlogDataNotFound);
            }

            getOldProjectData.IsActive = false;
            getOldProjectData.UpdatedByUserId = UserId;
            getOldProjectData.UpdatedDate = DateTimeNow;

            await _unitOfWork.ProjectRepository.UpdateAsync(getOldProjectData);

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

        private static bool CheckProjectDataIsChange(ref Project entity, ref ProjectModel model)
        {
            if (entity.Description != model.Description || entity.Img != model.Img || entity.LocalizationId != model.LocalizationId || entity.Name != model.ProjectName || entity.SrcUrl != model.SrcUrl)
            {
                return true;
            }
            return false;
        }

        private static bool CheckNewCategoryFromModel(ref ProjectModel model)
        {
            if (model.Categories.Any(i => i.CategoryId == 0))
            {
                return true;
            }
            return false;
        }

        #endregion

    }
}
