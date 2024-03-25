using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Brands;

namespace Application.Features.Brands.Command
{
    public record CreateBrandCommand(CreateBrandRequestDto Data ):IRequest<IResult>;

    public class CreateBrandCommandHandler:IRequestHandler<CreateBrandCommand,IResult>
    {
        private IBrandRepository Repository { get; set; }
        private IAppDbContext AppDbContext { get; set; }

        public CreateBrandCommandHandler(IAppDbContext appDbContext, IBrandRepository repository)
        {
            AppDbContext = appDbContext;
            Repository = repository;
        }

        public async Task<IResult> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
           
            var row = Brand.Create(request.Data.Name);
            await Repository.AddBrand(row);
            var result=await AppDbContext.SaveChangesAsync(cancellationToken);
            if(result>0)
            {
                return Result.Success($"{request.Data.Name} created succesfully!");
            }
            return Result.Fail($"{request.Data.Name} was not created succesfully!");
        }
    }

}
