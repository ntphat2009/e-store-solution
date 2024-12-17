using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Data.Configurations
{
    public class StoreListConfiguration:IEntityTypeConfiguration<StoreList>
    {
        public void Configure(EntityTypeBuilder<StoreList> builder)
        {
            builder.ToTable("Store");
            builder.HasKey(x => x.Id);
        }
    }
}
