using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Documents.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Infrastructure.Configurations
{
    public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
    {
        public void Configure(EntityTypeBuilder<Incident> builder)
        {
            builder.HasKey(i => i.ID);

            builder.Property(i => i.ID)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.WorkType)
                .HasConversion<String>()
                .IsRequired();

            builder.Property(i => i.Priority)
                .IsRequired(false);

            builder.Property(i => i.Confirmed)
                 .IsRequired();

            builder.Property(i => i.IncidentStatus)
                 .HasConversion<String>()
                 .IsRequired();

            builder.Property(i => i.ETA)
                 .IsRequired();

            builder.Property(i => i.ATA)
                  .IsRequired(false);

            builder.Property(i => i.IncidentDateTime)
                  .IsRequired(false);

            builder.Property(i => i.ETR)
                  .IsRequired(false);

            builder.Property(i => i.VoltageLevel)
                   .IsRequired(false);
             
            builder.Property(i => i.WorkBeginDate)
                  .IsRequired(false);

            builder.Property(i => i.Description)
                  .IsRequired(false);

            builder.Property(i => i.UserID)
                .IsRequired();

            builder.Property(i => i.CrewID)
                .IsRequired(false);

            builder.HasOne(i => i.MultimediaAnchor)
                .WithOne(p => p.Incident)
                .HasForeignKey<Incident>(i => i.MultimediaAnchorID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(i => i.NotificationAnchor)
               .WithOne(p => p.Incident)
               .HasForeignKey<Incident>(i => i.NotificationAnchorID)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
