using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Contract.Enums;
using SmartEnergy.Users.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Users.Infrastructure
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(i => i.ID);

            builder.Property(i => i.ID)
                .ValueGeneratedOnAdd();

            // add unique
            builder.Property(i => i.Username)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(i => i.Email)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(i => i.Password)
                 .IsRequired();

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(i => i.Lastname)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(u => u.UserStatus)
                .IsRequired()
                .HasConversion<String>()
                .HasDefaultValue(UserStatus.PENDING);

            builder.Property(i => i.BirthDay)
                .IsRequired(false);

            builder.Property(i => i.UserType)
                 .HasConversion<String>()
                 .IsRequired();

            builder.Property(i => i.ImageURL)
                .IsRequired(false);

            builder.HasOne(i => i.Crew)
                 .WithMany(p => p.CrewMembers)
                 .HasForeignKey(i => i.CrewID)
                 .IsRequired(false)
                 .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Property(i => i.LocationID)
                .IsRequired();

        }
    }
}
