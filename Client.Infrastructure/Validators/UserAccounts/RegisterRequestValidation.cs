using Client.Infrastructure.Managers.UserAccount;
using Shared.Models.UserAccounts.Registers;

namespace Client.Infrastructure.Validators.UserAccounts
{
    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        private IUserAccountService _accountManager;
        public RegisterUserRequestValidator(IUserAccountService accountManager)
        {

            _accountManager = accountManager;

            RuleFor(x => x.Email).NotEmpty().WithMessage("Must Supply email");
            RuleFor(x => x.Email).NotNull().WithMessage("Must Supply email");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Must supply valid email");
            RuleFor(x => x.Email).MustAsync(ReviewEmailExist).When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Email already exist");
            RuleFor(x => x.Role.Name).NotEqual("None").WithMessage("Must Supply Role");
        }
        async Task<bool> ReviewEmailExist(string email, CancellationToken cancellationToken)
        {
            var result = await _accountManager.ValidateIfEmailExist(email);
            return !result.Succeeded;
        }
    }
}
