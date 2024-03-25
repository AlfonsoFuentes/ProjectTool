using Application.Interfaces;
using MediatR;

namespace Application.Features.MWOs.Queries
{
    public record ValidateMWONameExist(string mwoname) : IRequest<bool>;
    public class ValidateMWONameExistHandler : IRequestHandler<ValidateMWONameExist, bool>
    {
        private readonly IMWORepository repository;

        public ValidateMWONameExistHandler(IMWORepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateMWONameExist request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfNameExist(request.mwoname);
        }
    }

}
