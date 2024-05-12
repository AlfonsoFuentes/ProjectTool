

namespace Application.Interfaces
{
    public interface IExcelService
    {
        Task<IResult<FileResult>> ExportAsync<TData>(IQueryable<TData> data, string sheetName = "Sheet1");
    }
}
