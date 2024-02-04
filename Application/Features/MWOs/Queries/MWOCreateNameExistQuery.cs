using Application.Interfaces;
using MediatR;

namespace Application.Features.MWOs.Queries
{
    public record MWOCreateNameExistQuery(string Name):IRequest<bool>;

    public class MWONameExistQueryHandler:IRequestHandler<MWOCreateNameExistQuery,bool>
    {
        private readonly IMWORepository repository;

        public MWONameExistQueryHandler(IMWORepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(MWOCreateNameExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewNameExist(request.Name);
        }
    }


}
