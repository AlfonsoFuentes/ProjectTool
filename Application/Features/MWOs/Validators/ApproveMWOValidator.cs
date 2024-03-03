using Application.Interfaces;
using Client.Infrastructure.Managers.CostCenter;
using FluentValidation;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Validators
{
    internal class ApproveMWOValidator:AbstractValidator<ApproveMWORequest>
    {
        private IMWORepository _repository;

        public ApproveMWOValidator(IMWORepository repository)
        {
            _repository = repository;
            
            RuleFor(x=>x.CostCenter.Id).NotEqual(CostCenterEnum.None.Id).WithMessage("Cost Center must be defined");

            RuleFor(x => x.MWONumber).MustAsync(ReviewIfNumberExist).WithMessage("MWO Number already exist");
            RuleFor(x => x.CostCenter).NotEqual(CostCenterEnum.None).WithMessage("Cost Center must be defined");

            RuleFor(x => x.MWONumber).NotNull().WithMessage("MWO Number must be defined");
            RuleFor(x => x.MWONumber).NotEmpty().WithMessage("MWO Number must be defined");
            RuleFor(x => x.MWONumber).Length(5).WithMessage("MWO Number must have 5 digits");
            RuleFor(customer => customer.MWONumber).Matches("^[0-9]*$").WithMessage("MWO Number must be number!");
            RuleFor(x => x.IsAbleToApproved).NotEqual(false).WithMessage("MWO must have items");
            RuleFor(x => x.PercentageTaxForAlterations).NotEqual(0).WithMessage("Must define Tax For Alterations");
            RuleFor(x => x.PercentageEngineering).NotEqual(0).WithMessage("Must define Percentage for Engineering");
            RuleFor(x => x.PercentageContingency).NotEqual(0).WithMessage("Must define Percentage for Contingency");
            RuleFor(x => x.PercentageAssetNoProductive).NotEqual(0).When(x => !x.IsAssetProductive).WithMessage("Must define Tax For Items");
        }
        async Task<bool> ReviewIfNumberExist(string cecnumber,CancellationToken cancellationToken)
        {
            return !(await _repository.ReviewIfNumberExist(cecnumber));
        }
    }
}
