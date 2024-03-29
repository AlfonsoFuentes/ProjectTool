

using Client.Infrastructure.Managers.UserAccount;
using Shared.Models.UserAccounts.Logins;

namespace Client.Infrastructure.IdentityModels.Validations
{
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        private IUserAccountService _accountManager;
        public LoginUserRequestValidator(IUserAccountService accountManager)
        {

            _accountManager = accountManager;

            RuleFor(x => x.Email).NotEmpty().WithMessage("Must Supply email");
            RuleFor(x => x.Email).NotNull().WithMessage("Must Supply email");
            RuleFor(x => x.Email).MustAsync(ReviewEmailExist).When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Email does not exist");
            RuleFor(x => x.Password).MustAsync(ReviewPasswordMatch).When(x => x.EmailConfirmed).WithMessage("Pasword doesn match");
        }
        async Task<bool> ReviewEmailExist(string email, CancellationToken cancellationToken)
        {
            var result = await _accountManager.ValidateIfEmailExist(email);
            return result.Succeeded;
        }
        async Task<bool> ReviewPasswordMatch(LoginUserRequest user,string pasword, CancellationToken cancellationToken)
        {
            var result = await _accountManager.ValidatePasswordMatch(user);
            return result.Succeeded;
        }
    }
}
