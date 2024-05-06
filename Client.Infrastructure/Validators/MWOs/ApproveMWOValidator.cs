

using FluentValidation;
using Shared.Enums.CostCenter;

namespace Client.Infrastructure.Validators.MWOs
{

    public class ApproveMWOValidator : AbstractValidator<ApproveMWORequest>
    {
        private IMWOValidatorService _Service;

        public ApproveMWOValidator(IMWOValidatorService service)
        {
            _Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage(x => $"Name must be defined");
            RuleFor(x => x.Name).NotNull().WithMessage(x => $"Name must be defined");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(x => $"{x.Name} already Exist!");

            RuleFor(x => x.MWONumber).NotNull().WithMessage("MWO Number must be defined");
            RuleFor(x => x.MWONumber).NotEmpty().WithMessage("MWO Number must be defined");
            RuleFor(x => x.MWONumber).Length(5).WithMessage("MWO Number must have 5 digits");
            RuleFor(x => x.MWONumber).MustAsync(ReviewIfNumberExist).When(x => !string.IsNullOrEmpty(x.MWONumber)).WithMessage(x => $"{x.MWONumber} Number already exist");
            RuleFor(customer => customer.MWONumber).Matches("^[0-9]*$").WithMessage("MWO Number must be number!");

            RuleFor(x => x.CostCenter.Id).NotEqual(CostCenterEnum.None.Id).WithMessage("Cost Center must be defined");
            RuleFor(x => x.PercentageTaxForAlterations).NotEqual(0).WithMessage("Must define Tax For Alterations");


            RuleFor(x => x.IsAbleToApproved).NotEqual(false).WithMessage("MWO must have items");

            RuleFor(x => x.PercentageEngineering).NotEqual(0).WithMessage("Must define Percentage for Engineering");
            RuleFor(x => x.PercentageContingency).NotEqual(0).WithMessage("Must define Percentage for Contingency");
            RuleFor(x => x.PercentageAssetNoProductive).NotEqual(0).When(x => !x.IsAssetProductive).WithMessage("Must define Tax For Items");
        }
        async Task<bool> ReviewIfNumberExist(ApproveMWORequest mwo, string cecnumber, CancellationToken cancellationToken)
        {
            return !await _Service.ValidateMWONumberExist(mwo.Id,cecnumber);
        }
        async Task<bool> ReviewIfNameExist(ApproveMWORequest mwo, string name, CancellationToken cancellationToken)
        {

            var result = await _Service.ValidateMWONameExist(mwo.Id, name);
            return !result;
        }
    }
}
