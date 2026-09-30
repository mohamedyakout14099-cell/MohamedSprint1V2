using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MohamedSprint1V2.DAL.Database.Configurations
{
    public class OrderHeaderConfiguration : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.HasKey(oh => oh.Id);

            builder.Property(oh => oh.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(oh => oh.ApplicationUser)
                .WithMany()
                .HasForeignKey(oh => oh.ApplicationUserId);
        }
    }
}
