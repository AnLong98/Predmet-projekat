using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Documents.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Infrastructure.Configurations
{
    public class MultimediaAttachmentConfiguration : IEntityTypeConfiguration<MultimediaAttachment>
    {
        public void Configure(EntityTypeBuilder<MultimediaAttachment> builder)
        {

            builder.HasKey(i => i.ID);

            builder.Property(i => i.ID)
                .ValueGeneratedOnAdd();

            // add unique
            builder.Property(i => i.Url)
                .IsRequired();

            builder.HasOne(i => i.MultimediaAnchor)
                 .WithMany(p => p.MultimediaAttachments)
                 .HasForeignKey(i => i.MultimediaAnchorID)
                 .IsRequired();
        }
    }
}
