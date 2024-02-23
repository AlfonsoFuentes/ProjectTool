

namespace Client.Infrastructure.IdentityModels.Validations
{
    public class RegisterRequestValidation : AbstractValidator<RegisterRequest>
    {
        private IAccountManager _accountManager;
        public RegisterRequestValidation(IAccountManager accountManager)
        {

            _accountManager = accountManager;
          
            RuleFor(x => x.Email).NotEmpty().WithMessage("Must Supply email");
            RuleFor(x => x.Email).MustAsync(ReviewEmailExist).WithMessage("Email already exist");
            RuleFor(x => x.Role).NotEmpty().WithMessage("Must Supply Role");
        }
        async Task<bool> ReviewEmailExist(string email, CancellationToken cancellationToken)
        {
            var result = await _accountManager.ReviewEmailExist(email);
            return !result.Succeeded;
        }
    }
}
