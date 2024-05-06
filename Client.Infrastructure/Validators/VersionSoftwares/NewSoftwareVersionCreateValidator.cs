using Client.Infrastructure.Managers.VersionSoftwares;
using Shared.NewModels.SoftwareVersion;

namespace Client.Infrastructure.Validators.VersionSoftwares
{
    public class NewSoftwareVersionCreateValidator : AbstractValidator<NewSoftwareVersionCreateRequest>
    {
        private readonly INewVersionSoftwareValidatorService Service;

        public NewSoftwareVersionCreateValidator(INewVersionSoftwareValidatorService service)
        {
            Service = service;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Version Software Name must be defined!");


            RuleFor(x => x.Name).MustAsync(ReviewIfNameExist)
                .When(x => !string.IsNullOrEmpty(x.Name))
                .WithMessage(x => $"{x.Name} already exist");

        }

        async Task<bool> ReviewIfNameExist(string name, CancellationToken cancellationToken)
        {
            var result = await Service.ReviewIfNameExist(name.ToLower());
            return !result;
        }
    }
}
