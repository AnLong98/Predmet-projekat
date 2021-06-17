using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Documents.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Infrastructure.Configurations
{
    public class MultimediaAnchorConfiguration : IEntityTypeConfiguration<MultimediaAnchor>
    {
        public void Configure(EntityTypeBuilder<MultimediaAnchor> builder)
        {
          
            builder.HasKey(i => i.ID);

            builder.Property(i => i.ID)
                .ValueGeneratedOnAdd();
        }
    }
}
