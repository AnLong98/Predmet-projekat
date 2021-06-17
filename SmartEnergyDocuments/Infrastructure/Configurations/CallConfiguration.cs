using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Documents.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Infrastructure.Configurations
{
    public class CallConfiguration : IEntityTypeConfiguration<Call>
    {
        public void Configure(EntityTypeBuilder<Call> builder)
        {

            builder.HasKey(i => i.ID);

            builder.Property(i => i.ID)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.CallReason)
                .HasConversion<String>()
                .IsRequired();

            builder.Property(i => i.Comment)
                .IsRequired(false);

            builder.Property(i => i.Hazard)
               .IsRequired();

            builder.Property(i => i.LocationID)
                .IsRequired(true);

            builder.Property(i => i.ConsumerID)
                .IsRequired(false);

            builder.HasOne(i => i.Incident)
              .WithMany(p => p.Calls)
              .HasForeignKey(i => i.IncidentID)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
