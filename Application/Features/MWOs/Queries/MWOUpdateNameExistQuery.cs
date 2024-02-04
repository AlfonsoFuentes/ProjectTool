using Application.Interfaces;
using MediatR;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Queries
{
    public record MWOUpdateNameExistQuery(UpdateMWORequest Item) : IRequest<bool>;
    public class MWOUpdateNameExistQueryHandler : IRequestHandler<MWOUpdateNameExistQuery, bool>
    {
        private readonly IMWORepository repository;

        public MWOUpdateNameExistQueryHandler(IMWORepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(MWOUpdateNameExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.ReviewNameExist(request.Item.Id,request.Item.Name);
        }
    }

}
