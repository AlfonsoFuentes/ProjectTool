using Shared.Models.SapAdjust;

namespace Client.Infrastructure.Validators.SapAdjusts
{
    public class UpdateSapAdjustValidator : AbstractValidator<UpdateSapAdjustRequest>
    {
        public UpdateSapAdjustValidator()
        {
            RuleFor(x => x.PecentageActual).LessThan(1).WithMessage("Review Software Actual Data ");
            RuleFor(x => x.PecentageCommitment).LessThan(1).WithMessage("Review Software Commitment Data ");
            RuleFor(x => x.PecentagePotencial).LessThan(1).WithMessage("Review Software Potential Data ");

            RuleFor(x => x.ImageData).NotEmpty().WithMessage("Must Add Sap Image ");
        }
    }
}
