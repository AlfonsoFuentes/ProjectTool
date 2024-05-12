namespace Application.Mappers.MWOS
{
    public static class MWOMappers
    {
        public static void FromMWOCreateRequest(this NewMWOCreateRequest request, MWO mwo)
        {
            mwo.Name = request.Name;
            mwo.IsAssetProductive = request.IsAssetProductive;

            mwo.PercentageTaxForAlterations = request.PercentageTaxForAlterations;
            mwo.Type = request.Type.Id;
            mwo.Focus = request.Focus.Id;

        }
        public static void FromMWOUpdateRequest(this NewMWOUpdateRequest request, MWO mwo)
        {
            mwo.Name = request.Name;

            mwo.PercentageTaxForAlterations = request.PercentageTaxForAlterations;
            mwo.Type = request.Type.Id;
            mwo.Focus = request.Focus.Id;

        }
        public static NewMWOCreatedResponse ToMWOCreatedResponse(this MWO mwo)
        {
            return new NewMWOCreatedResponse()
            {
                MWOId = mwo.Id,
                Focus = FocusEnum.GetType(mwo.Focus),
                IsAssetProductive = mwo.IsAssetProductive,
                Name = mwo.Name,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageCapitalizedSalary,
                PercentageTaxForAlterations = mwo.PercentageTaxForAlterations,
                Type = MWOTypeEnum.GetType(mwo.Type),
                CapitalUSD = mwo.CapitalUSD,
                ExpensesUSD = mwo.ExpensesUSD,
                HasExpenses = mwo.HasExpenses,



            };
        }
        public static NewMWOUpdateRequest ToMWOUpdateRequest(this MWO mwo)
        {
            return new NewMWOUpdateRequest()
            {
                MWOId = mwo.Id,
                Focus = FocusEnum.GetType(mwo.Focus),
                IsAssetProductive = mwo.IsAssetProductive,
                Name = mwo.Name,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageCapitalizedSalary,
                PercentageTaxForAlterations = mwo.PercentageTaxForAlterations,
                Type = MWOTypeEnum.GetType(mwo.Type),




            };
        }
        public static NewMWOApproveRequest ToMWOApproveRequest(this MWO mwo)
        {
            return new NewMWOApproveRequest()
            {

                MWONumber = mwo.MWONumber,
                MWOId = mwo.Id,
                Focus = FocusEnum.GetType(mwo.Focus),
                CostCenter = CostCenterEnum.GetType(mwo.CostCenter),
                IsAssetProductive = mwo.IsAssetProductive,
                Name = mwo.Name,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageCapitalizedSalary,
                PercentageTaxForAlterations = mwo.PercentageTaxForAlterations,
                Type = MWOTypeEnum.GetType(mwo.Type),
                BudgetItems = mwo.BudgetItems.Select(x => x.ToBudgetItemMWOCreatedResponse()).ToList(),



            };
        }
        public static NewMWOCreatedWithItemsResponse ToMWOWithItemsCreatedResponse(this MWO mwo)
        {
            return new NewMWOCreatedWithItemsResponse()
            {
                MWOId = mwo.Id,
                Focus = FocusEnum.GetType(mwo.Focus),
                IsAssetProductive = mwo.IsAssetProductive,
                Name = mwo.Name,
                PercentageAssetNoProductive = mwo.PercentageAssetNoProductive,
                PercentageContingency = mwo.PercentageContingency,
                PercentageEngineering = mwo.PercentageCapitalizedSalary,
                PercentageTaxForAlterations = mwo.PercentageTaxForAlterations,
                Type = MWOTypeEnum.GetType(mwo.Type),
                BudgetItems = mwo.BudgetItems == null || mwo.BudgetItems.Count == 0 ? new() :
                              mwo.BudgetItems.OrderBy(x => x.Nomeclatore).Select(x => x.ToBudgetItemMWOCreatedResponse()).ToList(),



            };
        }
        public static NewMWOApprovedReponse ToMWOApprovedReponse(this MWO mwo)
        {
            return new()
            {
                MWOId = mwo.Id,
                ApprovedDate = mwo.ApprovedDate,
                CostCenter = CostCenterEnum.GetType(mwo.CostCenter),
                Focus = FocusEnum.GetType(mwo.Focus),
                IsAssetProductive = mwo.IsAssetProductive,
                Name = mwo.Name,
                MWONumber = mwo.MWONumber,
                PercentageTaxForAlterations = mwo.PercentageTaxForAlterations,
                Type = MWOTypeEnum.GetType(mwo.Type),
                BudgetItems = (mwo.BudgetItems == null || mwo.BudgetItems.Count == 0) ? new() :
                mwo.BudgetItems.Select(x => x.ToBudgetItemMWOApprovedResponse()).ToList(),



            };
        }
       
        public static NewEBPReportResponse ToMWOEBPReportResponse(this MWO mwo)
        {
            return new()
            {
                ApprovedDate = mwo.ApprovedDate,
                CostCenter = CostCenterEnum.GetType(mwo.CostCenter),
                Focus = FocusEnum.GetType(mwo.Focus),
                IsAssetProductive = mwo.IsAssetProductive,
                Name = mwo.Name,
                MWOId = mwo.Id,
                MWONumber = $"CEC0000{mwo.MWONumber}",
                Type = MWOTypeEnum.GetType(mwo.Type),
                PurchaseOrders = mwo.PurchaseOrders == null || mwo.PurchaseOrders.Count == 0 ? new() :
                mwo.PurchaseOrders.Where(x=>x.IsAlteration==false).Select(x => x.ToPurchaseOrderResponse()).ToList(),

            };
        }

    }
}
