using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Infrastructure.Data.Configurations
{
    public class BannerConfiguration : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.ToTable("Banners");
            builder.HasKey(o => o.Id);
            builder.HasIndex(t => t.ImageURL);
            builder.HasIndex(t => t.IsActive);
            builder.HasOne(b => b.Category)
                 .WithMany(c => c.Banners)
                 .HasForeignKey(b => b.CategoryId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
