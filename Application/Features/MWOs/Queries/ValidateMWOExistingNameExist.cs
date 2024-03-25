using Application.Interfaces;
using MediatR;

namespace Application.Features.MWOs.Queries
{
    public record ValidateMWOExistingNameExist(Guid MWOId,string mwoname) : IRequest<bool>;
    public class ValidateMWOExistingNameExistHandler : IRequestHandler<ValidateMWOExistingNameExist, bool>
    {
        private readonly IMWORepository repository;

        public ValidateMWOExistingNameExistHandler(IMWORepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateMWOExistingNameExist request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfNameExist(request.MWOId, request.mwoname);
        }
    }
}
