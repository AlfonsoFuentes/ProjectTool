using Shared.Commons.Results;
using Shared.Models.FileResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IExcelService
    {
        Task<IResult<FileResult>> ExportAsync<TData>(IQueryable<TData> data, string sheetName = "Sheet1");
    }
}
