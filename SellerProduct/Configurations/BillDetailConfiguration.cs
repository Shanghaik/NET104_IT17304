using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SellerProduct.Models;
namespace SellerProduct.Configurations
{
    public class BillDetailConfiguration : IEntityTypeConfiguration<BillDetails>
    {
        public void Configure(EntityTypeBuilder<BillDetails> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(p=>p.Quantity).IsRequired().
                HasColumnType("int");
            // Set khóa ngoại
            builder.HasOne(p=>p.Bill).WithMany(p => p.BillDetails).
                HasForeignKey(p=>p.IdHD).HasConstraintName("FK_HD");
            builder.HasOne(p => p.Product).WithMany(p => p.Details).
                HasForeignKey(p => p.IdSP).HasConstraintName("FK_SP");
        }
    }
}
