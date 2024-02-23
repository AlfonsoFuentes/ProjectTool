using Shared.Models.BudgetItems;
using Shared.Models.MWOStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.MWO
{
    public class MWOResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public MWOStatusEnum Status { get; set; } = MWOStatusEnum.None;
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedOn { get; set; } = string.Empty;
        public double Capital { get; set; } = 0;
        public double Expenses { get; set; } = 0;
        public double Appropiation => Capital + Expenses;
        public string CECName { get; set; } = string.Empty;
        public string CostCenter { get; set; } = string.Empty;
        public bool IsRealProductive { get; set; } = false;
        public List<BudgetItemResponse> BudgetItems { get; set; } = new();
    }
}
