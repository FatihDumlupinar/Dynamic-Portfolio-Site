using DynamicPortfolioSite.Entities.Models.Contact;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DynamicPortfolioSite.Api.Validations
{
    public class ContactModelValidator : AbstractValidator<ContactModel>
    {
        public ContactModelValidator(IStringLocalizer<ContactModelValidator> _localizer)
        {
            RuleFor(e => e.Subject)
                .NotEmpty()
                .WithMessage(e => _localizer["SubjectIsRequired"])
                .NotNull()
                .WithMessage(e => _localizer["SubjectIsRequired"]);

            RuleFor(e => e.SenderEmail)
                .NotEmpty()
                .WithMessage(e => _localizer["SenderEmailIsRequired"])
                .NotNull()
                .WithMessage(e => _localizer["SenderEmailIsRequired"])
                .EmailAddress()
                .WithMessage(e => _localizer["SenderEmailIsMustEmailAddress"]);

            RuleFor(e => e.Text)
                .NotEmpty()
                .WithMessage(e => _localizer["TextIsRequired"])
                .NotNull()
                .WithMessage(e => _localizer["TextIsRequired"])
                .MaximumLength(1000)
                .WithMessage(e => _localizer["TextMaximumLength"]);

        }

    }
}
