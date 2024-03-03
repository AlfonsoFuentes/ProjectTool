using Application.Interfaces;
using FluentValidation;
using Shared.Models.MWO;
using Shared.Models.MWOTypes;

namespace Application.Features.MWOs.Validators
{
    internal class UpdateMWOMinimalValidator : AbstractValidator<UpdateMWOMinimalRequest>
    {
        private IMWORepository _repository;

        public UpdateMWOMinimalValidator(IMWORepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Type.Id).NotEqual(MWOTypeEnum.None.Id).WithMessage("MWO Type must be defined");

            RuleFor(x => x).MustAsync(ReviewIfNameExist).WithMessage("MWO Number already exist");
        }
        async Task<bool> ReviewIfNameExist(UpdateMWOMinimalRequest mwo, CancellationToken cancellationToken)
        {
            return !(await _repository.ReviewIfNameExist(mwo.Id,mwo.Name));
        }
    }
}
