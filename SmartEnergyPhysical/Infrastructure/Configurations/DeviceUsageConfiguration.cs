using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Physical.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Physical.Infrastructure.Configurations
{
    public class DeviceUsageConfiguration : IEntityTypeConfiguration<DeviceUsage>
    {
        public void Configure(EntityTypeBuilder<DeviceUsage> builder)
        {
            //Set table name mapping
            // builder.ToTable("ListItems");

            //Set FK
            builder.HasKey(i => i.ID);

            builder.Property(i => i.ID)
                .ValueGeneratedOnAdd();

            builder.HasOne(i => i.Device)
                .WithMany(p => p.DeviceUsage)
                .HasForeignKey(i => i.DeviceID)
                .IsRequired();

        }
    }
}
