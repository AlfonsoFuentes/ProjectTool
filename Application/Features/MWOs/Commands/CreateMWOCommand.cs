using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.MWO;

namespace Application.Features.MWOs.Commands
{
    public record CreateMWOCommand(CreateMWORequest Data) : IRequest<IResult>;

    public class CreateMWOCommandHandler : IRequestHandler<CreateMWOCommand, IResult>
    {
        private readonly IMWORepository Repository;
   
        private IAppDbContext AppDbContext;
        public CreateMWOCommandHandler(IMWORepository repository,  IAppDbContext appDbContext)
        {
            Repository = repository;
      
            AppDbContext = appDbContext;
        }

        public async Task<IResult> Handle(CreateMWOCommand request, CancellationToken cancellationToken)
        {
            var row = MWO.Create(request.Data.Name, request.Data.Type!.Id);
            await Repository.AddMWO(row);
            var result =  await AppDbContext.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Result.Success($"{request.Data.Name} created succesfully");
            }

            return Result.Fail($"{request.Data.Name} was not created succesfully");
        }
    }

}
