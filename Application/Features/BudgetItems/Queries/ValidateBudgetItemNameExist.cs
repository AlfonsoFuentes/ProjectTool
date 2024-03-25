using Application.Features.Brands.Queries;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.BudgetItems.Queries
{
    public record ValidateBudgetItemNameExist(Guid MWOId, string Name):IRequest<bool>;
    public class ValidateBudgetItemNameExistHandler : IRequestHandler<ValidateBudgetItemNameExist, bool>
    {
        private readonly IBudgetItemRepository repository;

        public ValidateBudgetItemNameExistHandler(IBudgetItemRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ValidateBudgetItemNameExist request, CancellationToken cancellationToken)
        {
            return await repository.ReviewIfNameExist(request.MWOId, request.Name);
        }
    }
}
