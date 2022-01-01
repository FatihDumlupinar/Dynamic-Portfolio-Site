using DynamicPortfolioSite.Api.Controllers.Base;
using DynamicPortfolioSite.Core.Constants;
using DynamicPortfolioSite.Entities.Enms;
using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Entities.Models.About;
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
    public class AboutController : BaseApiController
    {
        #region Ctor&Fields

        private readonly IUnitOfWork _unitOfWork;

        public AboutController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region GetAsync

        [HttpGet("get")]
        public async Task<IActionResult> GetAsync(int localizationId = (int)LocalizationEnm.Tr)
        {
            #region About

            var aboutEntity = await _unitOfWork.AboutRepository.GetAsync(i => i.IsActive && i.LocalizationId == localizationId);
            if (aboutEntity == default)
            {
                return NotFound(Messages.AboutDataNotFound);
            }

            #endregion

            AboutEditModel returnModel = new() { Text = aboutEntity.Text, LocalizationId = aboutEntity.LocalizationId };

            await Task.WhenAll(new Task[]{
                Task.Run(async ()=>{
                    var workEntities = await _unitOfWork.WorkRepository.GetListAsync(i => i.IsActive && i.LocalizationId == localizationId && i.AboutId == aboutEntity.Id);
                   if (workEntities != default)
                   {
                       returnModel.Works = workEntities.Select(i => new AboutWorkListModel()
                       {
                           CompanyName = i.CompanyName,
                           DateRange = i.DateRange,
                           Description = i.Description,
                           JobName = i.JobName,
                           RowNumber = i.RowNumber,
                           WorkId = i.RowNumber
                       }).ToList();
                   }
                }),
                Task.Run(async ()=>{
                    var educationEntities = await _unitOfWork.EducationRepository.GetListAsync(i => i.IsActive && i.LocalizationId == localizationId && i.AboutId == aboutEntity.Id);
                   if (educationEntities != default)
                   {
                       returnModel.Educations = educationEntities.Select(i => new AboutEducationListModel()
                       {
                           DateRange = i.DateRange,
                           Degree = i.Degree,
                           Description = i.Description,
                           RowNumber = i.RowNumber,
                           SchoolName = i.SchoolName
                       }).ToList();
                   }
                }),
                Task.Run(async ()=>{
                     var skillEntities = await _unitOfWork.SkillRepository.GetListAsync(i => i.IsActive && i.LocalizationId == localizationId && i.AboutId == aboutEntity.Id);
                   if (skillEntities != default)
                   {
                       returnModel.Skills = skillEntities.Select(i => new AboutSkillListModel()
                       {
                           Description = i.Description,
                           Name = i.Name,
                           Rate = i.Rate,
                           RowNumber = i.RowNumber
                       }).ToList();
                   }
                })
            });

            return Ok(returnModel);
        }

        #endregion

        #region EditAsync

        [HttpPost("edit")]
        public async Task<IActionResult> EditAsync(AboutEditModel model)
        {
            #region About

            About oldAboutData = await _unitOfWork.AboutRepository.GetAsync(i => i.IsActive && i.LocalizationId == model.LocalizationId);
            if (oldAboutData == default)
            {
                return NotFound(Messages.AboutDataNotFound);
            }

            if (CheckAboutIsChange(ref oldAboutData, ref model))
            {
                oldAboutData.Text = model.Text;
                oldAboutData.UpdatedDate = DateTimeNow;
                oldAboutData.UpdatedByUserId = UserId;

                await _unitOfWork.AboutRepository.UpdateAsync(oldAboutData);
                await _unitOfWork.CommitAsync();
            }

            #endregion

            Parallel.Invoke(

                async () =>
                {
                    #region Work

                    List<Work> oldWorkListData = await _unitOfWork.WorkRepository.GetListAsync(i => i.IsActive && i.LocalizationId == model.LocalizationId && i.AboutId == oldAboutData.Id);

                    if (oldWorkListData.Any())
                    {
                        if (CheckWorkIsChange(ref oldWorkListData, ref model))
                        {
                            List<Work> editWorkListModel = new();
                            AboutWorkListModel aboutWorkModel = default;

                            foreach (var work in oldWorkListData)
                            {
                                aboutWorkModel = model.Works.SingleOrDefault(i => i.WorkId == work.Id);
                                if (aboutWorkModel != default)
                                {
                                    work.CompanyName = aboutWorkModel.CompanyName;
                                    work.JobName = aboutWorkModel.JobName;
                                    work.RowNumber = aboutWorkModel.RowNumber;
                                    work.DateRange = aboutWorkModel.DateRange;
                                    work.Description = aboutWorkModel.Description;
                                }
                                else
                                {
                                    work.IsActive = false;
                                }

                                work.UpdatedByUserId = UserId;
                                work.UpdatedDate = DateTimeNow;

                                editWorkListModel.Add(work);

                            }

                            await _unitOfWork.WorkRepository.UpdateAllAsync(editWorkListModel);
                            await _unitOfWork.CommitAsync();
                        }
                    }

                    if (CheckNewWork(ref model))
                    {
                        await _unitOfWork.WorkRepository.AddAllAsync(model.Works.Where(i => i.WorkId == 0).Select(i => new Work()
                        {
                            AboutId = oldAboutData.Id,
                            CreatedByUserId = UserId,
                            CompanyName = i.CompanyName,
                            CreatedDate = DateTimeNow,
                            DateRange = i.DateRange,
                            Description = i.Description,
                            IsActive = true,
                            JobName = i.JobName,
                            LocalizationId = model.LocalizationId,
                            RowNumber = i.RowNumber

                        }));
                        await _unitOfWork.CommitAsync();
                    }

                    #endregion
                },

                async () =>
                {
                    #region Education

                    List<Education> oldEducationListData = await _unitOfWork.EducationRepository.GetListAsync(i => i.IsActive && i.LocalizationId == model.LocalizationId && i.AboutId == oldAboutData.Id);

                    if (oldEducationListData.Any())
                    {
                        if (CheckEducationIsChange(ref oldEducationListData, ref model))
                        {
                            List<Education> editEducationListModel = new();
                            AboutEducationListModel aboutEducationModel = default;

                            foreach (var education in oldEducationListData)
                            {
                                aboutEducationModel = model.Educations.SingleOrDefault(i => i.EducationId == education.Id);
                                if (aboutEducationModel != default)
                                {
                                    education.DateRange = aboutEducationModel.DateRange;
                                    education.Degree = aboutEducationModel.Degree;
                                    education.Description = aboutEducationModel.Description;
                                    education.RowNumber = aboutEducationModel.RowNumber;
                                    education.SchoolName = aboutEducationModel.SchoolName;

                                }
                                else
                                {
                                    education.IsActive = false;
                                }

                                education.UpdatedByUserId = UserId;
                                education.UpdatedDate = DateTimeNow;

                                editEducationListModel.Add(education);
                            }

                            await _unitOfWork.EducationRepository.UpdateAllAsync(editEducationListModel);
                            await _unitOfWork.CommitAsync();
                        }
                    }

                    if (CheckIsNewEducation(ref model))
                    {
                        await _unitOfWork.EducationRepository.AddAllAsync(model.Educations.Where(i => i.EducationId == 0).Select(i => new Education()
                        {
                            AboutId = oldAboutData.Id,
                            CreatedByUserId = UserId,
                            CreatedDate = DateTimeNow,
                            DateRange = i.DateRange,
                            Degree = i.Degree,
                            Description = i.Description,
                            IsActive = true,
                            LocalizationId = model.LocalizationId,
                            RowNumber = i.RowNumber,
                            SchoolName = i.SchoolName

                        }));
                        await _unitOfWork.CommitAsync();
                    }

                    #endregion
                },

                async () =>
                {
                    #region Skills

                    List<Skill> oldSkillListData = await _unitOfWork.SkillRepository.GetListAsync(i => i.IsActive && i.LocalizationId == model.LocalizationId && i.AboutId == oldAboutData.Id);

                    if (oldSkillListData.Any())
                    {
                        if (CheckSkillIsChange(ref oldSkillListData, ref model))
                        {
                            List<Skill> editSkillListModel = new();
                            AboutSkillListModel aboutSkillModel = default;

                            foreach (var skill in oldSkillListData)
                            {
                                aboutSkillModel = model.Skills.SingleOrDefault(i => i.SkillId == skill.Id);
                                if (aboutSkillModel != default)
                                {
                                    skill.Description = aboutSkillModel.Description;
                                    skill.Name = aboutSkillModel.Name;
                                    skill.Rate = aboutSkillModel.Rate;
                                    skill.RowNumber = aboutSkillModel.RowNumber;
                                }
                                else
                                {
                                    skill.IsActive = false;
                                }

                                skill.UpdatedByUserId = UserId;
                                skill.UpdatedDate = DateTimeNow;

                                editSkillListModel.Add(skill);
                            }

                            await _unitOfWork.SkillRepository.UpdateAllAsync(editSkillListModel);
                            await _unitOfWork.CommitAsync();
                        }
                    }

                    if (CheckIsNewSkill(ref model))
                    {
                        await _unitOfWork.SkillRepository.AddAllAsync(model.Skills.Where(i => i.SkillId == 0).Select(i => new Skill()
                        {
                            AboutId = oldAboutData.Id,
                            CreatedByUserId = UserId,
                            CreatedDate = DateTimeNow,
                            Description = i.Description,
                            IsActive = true,
                            LocalizationId = model.LocalizationId,
                            Name = i.Name,
                            Rate = i.Rate,
                            RowNumber = i.RowNumber
                        }));
                        await _unitOfWork.CommitAsync();
                    }

                    #endregion
                }

                );

            return Ok();
        }

        #endregion

        #region Functions

        private static bool CheckAboutIsChange(ref About entity, ref AboutEditModel model)
        {
            if (entity.Text != model.Text)
            {
                return true;
            }
            return false;
        }

        private static bool CheckWorkIsChange(ref List<Work> entity, ref AboutEditModel model)
        {
            Work checkData = default;

            foreach (var work in model.Works)
            {
                checkData = entity.SingleOrDefault(i => i.Id == work.WorkId);
                if (checkData.JobName != work.JobName || checkData.RowNumber != work.RowNumber || checkData.CompanyName != work.CompanyName || checkData.DateRange != work.DateRange || checkData.Description != work.Description)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CheckNewWork(ref AboutEditModel model)
        {
            if (model.Works.Any(i => i.WorkId == 0))
            {
                return true;
            }

            return false;
        }

        private static bool CheckEducationIsChange(ref List<Education> entity, ref AboutEditModel model)
        {
            Education checkData = default;

            foreach (var education in model.Educations)
            {
                checkData = entity.SingleOrDefault(i => i.Id == education.EducationId);
                if (checkData.DateRange != education.DateRange || checkData.Degree != education.Degree || checkData.Description != education.Description || checkData.RowNumber != education.RowNumber || checkData.SchoolName != education.SchoolName)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CheckIsNewEducation(ref AboutEditModel model)
        {
            if (model.Educations.Any(i => i.EducationId == 0))
            {
                return true;
            }
            return false;
        }

        private static bool CheckSkillIsChange(ref List<Skill> entity, ref AboutEditModel model)
        {
            Skill checkData = default;

            foreach (var skill in model.Skills)
            {
                checkData = entity.SingleOrDefault(i => i.Id == skill.SkillId);
                if (checkData.Name != skill.Name || checkData.Rate != skill.Rate || checkData.RowNumber != skill.RowNumber || checkData.Description != skill.Description)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CheckIsNewSkill(ref AboutEditModel model)
        {
            if (model.Skills.Any(i => i.SkillId == 0))
            {
                return true;
            }
            return false;
        }

        #endregion

    }
}
