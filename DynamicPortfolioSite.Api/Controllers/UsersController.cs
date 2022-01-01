using DynamicPortfolioSite.Api.Controllers.Base;
using DynamicPortfolioSite.Core.Utilities.Helpers;
using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Entities.Models.User;
using DynamicPortfolioSite.Repository.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPortfolioSite.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        #region Ctor&Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<UsersController> _localizer;

        public UsersController(IUnitOfWork unitOfWork, IStringLocalizer<UsersController> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        #endregion

        #region Add

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromBody] UserModel model)
        {
            var checkUserNameAndEmail = await _unitOfWork.AppUserRepository.GetAsync(i => i.IsActive && (i.UserName == model.UserName || i.Email == model.Email));
            if (checkUserNameAndEmail != default)
            {
                return BadRequest(_localizer["UserAlreadyAdded"]);
            }

            await _unitOfWork.AppUserRepository.AddAsync(new AppUser()
            {
                CreatedByUserId = UserId,
                CreatedDate = DateTimeNow,
                Email = model.Email,
                IsActive = true,
                PasswordHash = HashingHelper.CreateMD5Hash(model.Password),
                UserImg = model.UserImg,
                UserName = model.UserName
            });

            await _unitOfWork.CommitAsync();

            return Ok(_localizer["UserAdded"]);
        }

        #endregion

        #region Edit

        [HttpPost("edit")]
        public async Task<IActionResult> EditAsync([FromBody] UserModel model)
        {
            var getOldData = await _unitOfWork.AppUserRepository.GetAsync(i => i.IsActive && i.Id == model.UserId);

            if (getOldData == default)
            {
                return NotFound(_localizer["UserNotFound"]);
            }

            if (getOldData.Email != model.Email)
            {
                var checkEmail = await _unitOfWork.AppUserRepository.GetAsync(i => i.IsActive && i.Email == model.Email);

                if (checkEmail != default)
                {
                    return BadRequest(_localizer["UserAlreadyAdded"]);
                }
            }

            if (getOldData.UserName != model.UserName)
            {
                var checkUserName = await _unitOfWork.AppUserRepository.GetAsync(i => i.IsActive && i.UserName == model.UserName);

                if (checkUserName != default)
                {
                    return BadRequest(_localizer["UserAlreadyAdded"]);
                }
            }

            getOldData.Email = model.Email;

            if (string.IsNullOrEmpty(model.Password))
            {
                getOldData.PasswordHash = HashingHelper.CreateMD5Hash(model.Password);
            }

            getOldData.UserImg = model.UserImg;
            getOldData.UserName = model.UserName;

            await _unitOfWork.AppUserRepository.UpdateAsync(getOldData);

            await _unitOfWork.CommitAsync();

            return Ok(_localizer["UserUpdated"]);
        }

        #endregion

        #region GetById

        [HttpPost("getbyid/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var getData = await _unitOfWork.AppUserRepository.GetAsync(i => i.IsActive && i.Id == id);
            if (getData == default)
            {
                return NotFound(_localizer["UserNotFound"]);
            }

            string createdUserName = "", UpdatedUserName = "";

            if (getData.CreatedByUserId != default)
            {
                var getCreatedUserData = await _unitOfWork.AppUserRepository.GetAsync(i => i.IsActive && i.Id == getData.CreatedByUserId);
                if (getCreatedUserData != default)
                {
                    createdUserName = getCreatedUserData.UserName;
                }
            }

            if (getData.UpdatedByUserId != default)
            {
                var getUpdatedUserData = await _unitOfWork.AppUserRepository.GetAsync(i => i.IsActive && i.Id == getData.UpdatedByUserId);
                if (getUpdatedUserData != default)
                {
                    UpdatedUserName = getUpdatedUserData.UserName;
                }
            }

            UserModel returnModel = new()
            {
                CreatedByUser = createdUserName,
                CreatedDate = getData.CreatedDate,
                Email = getData.Email,
                Password = "",
                UpdatedByUser = UpdatedUserName,
                UpdatedDate = getData.UpdatedDate,
                UserId = getData.Id,
                UserImg = getData.UserImg,
                UserName = getData.UserName
            };

            return Ok(returnModel);
        }

        #endregion

        #region Delete

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var getOldData = await _unitOfWork.AppUserRepository.GetAsync(i => i.IsActive && i.Id == id);
            if (getOldData == default)
            {
                return NotFound(_localizer["UserNotFound"]);
            }

            getOldData.IsActive = false;
            getOldData.UpdatedByUserId = UserId;
            getOldData.UpdatedDate = DateTimeNow;

            await _unitOfWork.AppUserRepository.UpdateAsync(getOldData);
            await _unitOfWork.CommitAsync();

            return Ok(_localizer["UserDeleted"]);
        }

        #endregion

        #region List

        [HttpGet("list")]
        public async Task<IActionResult> ListAsync([FromQuery] UserListParamsModel model)
        {
            var getListData = await _unitOfWork.AppUserRepository.CustomSearchAsync(new UserSearchModel()
            {
                CreateDateRange_End = model.CreateDateRange_End,
                CreateDateRange_Start = model.CreateDateRange_Start
            });

            List<AppUser> userList = new();

            if (getListData.Any(i => i.CreatedByUserId != default || i.UpdatedByUserId != default))
            {
                var onlyUserIds = getListData.Select(i => i.CreatedByUserId).ToList();
                onlyUserIds.AddRange(getListData.Where(i => i.UpdatedByUserId != default).Select(i => (int)i.UpdatedByUserId));

                List<AppUser> getAllUserData = await _unitOfWork.AppUserRepository.GetListAsync(i => i.IsActive && onlyUserIds.Contains(i.Id));

                userList = getAllUserData;
            }

            List<UserListModel> returnModel = getListData.Select(i => new UserListModel()
            {
                UserId = i.Id,
                Email = i.Email,
                UserImg = i.UserImg,
                UserName = i.UserName,
                CreatedDate = i.CreatedDate,
                UpdatedDate = i.UpdatedDate,
                CreatedByUser = FindUser(ref userList, i.CreatedByUserId),
                UpdatedByUser = FindUser(ref userList, (int)i.UpdatedByUserId)
            }).ToList();

            return Ok(returnModel);
        }

        #endregion

        #region Functions

        private static string FindUser(ref List<AppUser> userList, int userid)
        {
            if (userid != default)
            {
                if (userList.Any(i => i.Id == userid))
                {
                    return userList.FirstOrDefault(i => i.Id == userid).UserName;
                }
            }
            return "";
        }

        #endregion

    }
}
