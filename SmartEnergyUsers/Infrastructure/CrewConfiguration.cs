using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Users.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Users.Infrastructure
{
    public class CrewConfiguration : IEntityTypeConfiguration<Crew>
    {
        public void Configure(EntityTypeBuilder<Crew> builder)
        {
            builder.HasKey(i => i.ID);

            builder.Property(i => i.ID)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.CrewName)
                .IsRequired()
                .HasMaxLength(50);

        }
    }
}
