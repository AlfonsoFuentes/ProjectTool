using Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    internal class MWOConfiguration : IEntityTypeConfiguration<MWO>
    {
        public void Configure(EntityTypeBuilder<MWO> builder)
        {
            builder.HasKey(ci => ci.Id);
            builder.HasMany(c => c.BudgetItems).WithOne(t => t.MWO).HasForeignKey(e => e.MWOId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.PurchaseOrders).WithOne(t => t.MWO).HasForeignKey(e => e.MWOId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        }

    }
}
