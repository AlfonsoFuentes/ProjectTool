using Application.Interfaces;
using Client.Infrastructure.Managers.CostCenter;
using FluentValidation;
using Shared.Models.BudgetItemTypes;
using Shared.Models.MWO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.MWOs.Validators
{
    internal class ApproveMWOValidator:AbstractValidator<ApproveMWORequest>
    {
        private IMWORepository _repository;

        public ApproveMWOValidator(IMWORepository repository)
        {
            _repository = repository;
            
            RuleFor(x=>x.CostCenter.Id).NotEqual(CostCenterEnum.None.Id).WithMessage("Cost Center must be defined");

            RuleFor(x => x.MWONumber).MustAsync(ReviewIfNumberExist).WithMessage("MWO Number already exist");
        }
        async Task<bool> ReviewIfNumberExist(string cecnumber,CancellationToken cancellationToken)
        {
            return !(await _repository.ReviewIfNumberExist(cecnumber));
        }
    }
}
