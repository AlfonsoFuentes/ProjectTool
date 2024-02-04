using Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbSet<MWO> MWOs { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<Supplier> Suppliers { get; set; }
        DbSet<BudgetItem> BudgetItems { get; set; }
        DbSet<TaxesItem> TaxesItems { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
