namespace Client.Infrastructure.Validators.MWOs
{
    public class NewMWOUpdateRequestValidator : AbstractValidator<NewMWOUpdateRequest>
    {
        private INewMWOValidatorService _Service;


        public NewMWOUpdateRequestValidator(INewMWOValidatorService service)
        {
            _Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("MWO Name must be defined!");
            RuleFor(x => x.Name).NotNull().WithMessage("MWO Name must be defined!");
            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage(x => $"{x.Name} already Exist!");
            RuleFor(x => x.Type.Id).NotEqual(MWOTypeEnum.None.Id).WithMessage("Type must be defined");
            RuleFor(x => x.Focus.Id).NotEqual(FocusEnum.None.Id).WithMessage("Focus must be defined");
            RuleFor(x => x.PercentageAssetNoProductive).GreaterThan(0).When(x => x.IsAssetProductive == false).WithMessage("Taxes No Productive must be defined!");
            RuleFor(x => x.PercentageTaxForAlterations).GreaterThan(0).WithMessage("Percentage Tax for Alterations must be defined!");
            RuleFor(x => x.PercentageEngineering).GreaterThan(0).WithMessage("Percentage Capitalized Salaries  must be defined!");
            RuleFor(x => x.PercentageContingency).GreaterThan(0).WithMessage("Percentage Contingency must be defined!");
        }

        async Task<bool> ReviewIfNameExist(NewMWOUpdateRequest mwo, string name, CancellationToken cancellationToken)
        {

            var result = await _Service.ValidateMWONameExist(mwo.MWOId, mwo.Name);
            return !result;
        }
    }
}
