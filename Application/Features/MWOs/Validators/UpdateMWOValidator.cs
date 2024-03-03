using Application.Interfaces;
using FluentValidation;
using Shared.Models.MWO;
using Shared.Models.MWOTypes;

namespace Application.Features.MWOs.Validators
{
    public class UpdateMWOValidator : AbstractValidator<UpdateMWORequest>
    {
        private readonly IMWORepository _repository;

        public UpdateMWOValidator(IMWORepository repository)
        {
            this._repository = repository;
            RuleFor(x => x.Name).NotEmpty().NotNull().WithMessage("MWO Name must be defined!");
            RuleFor(x => x).MustAsync(ReviewIfNameExist).WithMessage(x=>$"{x.Name} already Exist!");
            RuleFor(x => x.Type.Id).NotEqual(MWOTypeEnum.None.Id).WithMessage("Type must be defined");
            RuleFor(x => x.PercentageAssetNoProductive).GreaterThan(0).When(x => x.IsAssetProductive == false).WithMessage("Taxes No Productive must be defined!");
            RuleFor(x => x.PercentageTaxForAlterations).GreaterThan(0).WithMessage("Percentage Tax for Alterations must be defined!");
            RuleFor(x => x.PercentageEngineering).GreaterThan(0).WithMessage("Percentage Capitalized Salaries  must be defined!");
            RuleFor(x => x.PercentageContingency).GreaterThan(0).WithMessage("Percentage Contingency must be defined!");
        }

        async Task<bool> ReviewIfNameExist(UpdateMWORequest mwo, CancellationToken cancellationToken)
        {

            var result = await _repository.ReviewIfNameExist(mwo.Id,mwo.Name);
            return !result;
        }
    }
}
