using DynamicPortfolioSite.Api.Controllers.Base;
using DynamicPortfolioSite.Entities.Entities;
using DynamicPortfolioSite.Entities.Models.Contact;
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
    public class ContactController : BaseApiController
    {
        #region Ctor&Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ContactController> _localizer;

        public ContactController(IUnitOfWork unitOfWork, IStringLocalizer<ContactController> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        #endregion

        #region Add

        [HttpPost("add")]
        [AllowAnonymous]
        public async Task<IActionResult> AddAsync([FromBody] ContactModel model)
        {
            await _unitOfWork.ContactRepository.AddAsync(new Contact()
            {
                CreatedDate = DateTimeNow,
                IsActive = true,
                IsRead = false,
                SenderEmail = model.SenderEmail,
                Subject = model.Subject,
                Text = model.Text
            });

            await _unitOfWork.CommitAsync();

            return Ok(_localizer["ContactAdded"]);
        }

        #endregion

        #region List

        [HttpGet("list")]
        public async Task<IActionResult> ListAsync([FromQuery] ContactListParamsModel model)
        {
            var getListData = await _unitOfWork.ContactRepository.CustomSearchAsync(new ContactSearchModel()
            {
                CreateDateRange_End = model.CreateDateRange_End,
                CreateDateRange_Start = model.CreateDateRange_Start
            });

            List<ContactListModel> returnModel = getListData.Select(i => new ContactListModel()
            {
                Id = i.Id,
                IsRead = i.IsRead,
                SenderEmail = i.SenderEmail,
                Subject = i.Subject,
                CreatedDate = i.CreatedDate
            }).ToList();

            return Ok(returnModel);
        }

        #endregion

        #region GetById

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            var getData = await _unitOfWork.ContactRepository.GetAsync(i => i.Id == id && i.IsRead);

            if (getData == default)
            {
                return NotFound(_localizer["ContactDataNotFound"]);
            }

            ContactModel returnModel = new()
            {
                Id = getData.Id,
                IsRead = true,
                SenderEmail = getData.SenderEmail,
                Subject = getData.Subject,
                Text = getData.Text
            };

            getData.IsRead = true;

            await _unitOfWork.ContactRepository.UpdateAsync(getData);
            await _unitOfWork.CommitAsync();

            return Ok(returnModel);
        }

        #endregion

        #region Delete

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> DeleteAsync([FromQuery] int id)
        {
            var getData = await _unitOfWork.ContactRepository.GetAsync(i => i.Id == id && i.IsRead);

            if (getData == default)
            {
                return NotFound(_localizer["ContactDataNotFound"]);
            }

            getData.IsActive = false;
            getData.UpdatedDate = DateTimeNow;
            getData.UpdatedByUserId = UserId;

            await _unitOfWork.ContactRepository.UpdateAsync(getData);
            await _unitOfWork.CommitAsync();

            return Ok(_localizer["ContactDeleted"]);
        }

        #endregion
    }
}
