using Client.Infrastructure.Managers.CostCenter;

namespace Client.Infrastructure.Validators.MWOs
{
    public class ApproveMWOValidator : AbstractValidator<ApproveMWORequest>
    {


        public ApproveMWOValidator()
        {

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

    }
}
