using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Documents.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Infrastructure.Configurations
{
    public class WorkRequestConfiguration : IEntityTypeConfiguration<WorkRequest>
    {
        public void Configure(EntityTypeBuilder<WorkRequest> builder)
        {
            builder.HasKey(i => i.ID);

            builder.Property(i => i.ID)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.DocumentType)
                .HasConversion<String>()
                .IsRequired();

            builder.Property(i => i.DocumentStatus)
                 .HasConversion<String>()
                 .IsRequired();

            builder.Property(i => i.StartDate)
                  .IsRequired();

            builder.Property(i => i.EndDate)
                  .IsRequired();

            builder.Property(i => i.CreatedOn)
                  .IsRequired(false)
                  .HasDefaultValueSql("getdate()");

            builder.Property(i => i.Purpose)
                  .HasMaxLength(100)
                  .IsRequired();

            builder.Property(i => i.Note)
                  .IsRequired(false)
                  .HasMaxLength(100);

            builder.Property(i => i.Details)
                 .IsRequired(false)
                 .HasMaxLength(100);

            builder.Property(i => i.IsEmergency)
                  .IsRequired();

            builder.Property(i => i.CompanyName)
                  .HasMaxLength(50)
                  .IsRequired(false);

            builder.Property(i => i.Phone)
                  .HasMaxLength(30)
                  .IsRequired(false);

            builder.Property(i => i.Street)
                  .HasMaxLength(50)
                  .IsRequired(false);

            builder.Property(i => i.UserID)
                .IsRequired();

            builder.HasOne(i => i.Incident)
                .WithOne(p => p.WorkRequest)
                .HasForeignKey<WorkRequest>(i => i.IncidentID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.MultimediaAnchor)
                .WithOne(p => p.WorkRequest)
                .HasForeignKey<WorkRequest>(i => i.MultimediaAnchorID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasOne(i => i.NotificationsAnchor)
               .WithOne(p => p.WorkRequest)
               .HasForeignKey<WorkRequest>(i => i.NotificationAnchorID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(i => i.StateChangeAnchor)
              .WithOne(p => p.WorkRequest)
              .HasForeignKey<WorkRequest>(i => i.StateChangeAnchorID)
              .IsRequired(false)
              .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
