using Application.Interfaces;
using MediatR;

namespace Application.Features.MWOs.Queries
{
    public record ValidateMWONumberExist(string mwonumber):IRequest<bool>;

    public class ValidateMWONumberExistHandler : IRequestHandler<ValidateMWONumberExist, bool>
    {
        private readonly IMWORepository repository;

        public ValidateMWONumberExistHandler(IMWORepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateMWONumberExist request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfNumberExist(request.mwonumber);
        }
    }


}
