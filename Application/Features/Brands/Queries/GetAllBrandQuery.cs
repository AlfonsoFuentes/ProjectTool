﻿using Application.Interfaces;
using Domain.Entities.Data;
using MediatR;
using Shared.Commons.Results;
using Shared.Models.Brands;
using System.Linq.Expressions;

namespace Application.Features.Brands.Queries
{
    public record GetAllBrandQuery() : IRequest<IResult<List<BrandResponse>>>;

    public class GetAllBrandQueryHandler:IRequestHandler<GetAllBrandQuery, IResult<List<BrandResponse>>>
    {
        private IBrandRepository Repository { get; set; }
      
        public GetAllBrandQueryHandler(IBrandRepository repository)
        {
            Repository = repository;
        
        }

        public async Task<IResult<List<BrandResponse>>> Handle(GetAllBrandQuery request, CancellationToken cancellationToken)
        {
        
            var rows = await Repository.GetBrandList();
            Expression<Func<Brand, BrandResponse>> expression = e => new BrandResponse
            {
                Id = e.Id,
                Name = e.Name,
                CreatedBy = e.CreatedByUserName,
                CreatedOn =e.CreatedDate.ToShortDateString(),
               
            };
            var result=rows.Select(expression).OrderBy(x=>x.Name).ToList();
            return Result<List<BrandResponse>>.Success(result);
        }
    }

}
