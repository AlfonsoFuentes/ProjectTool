namespace Shared.NewModels.Suppliers.Reponses
{
    public class NewSupplierExportFileListResponse
    {
        public IQueryable<NewSupplierExportFileResponse> Suppliers { get; set; } = null!;

    }
}
