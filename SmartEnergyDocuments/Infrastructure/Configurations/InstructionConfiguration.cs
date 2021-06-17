using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEnergy.Documents.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Infrastructure.Configurations
{
    public class InstructionConfiguration : IEntityTypeConfiguration<Instruction>
    {
        public void Configure(EntityTypeBuilder<Instruction> builder)
        {
            builder.HasKey(i => i.ID);

            builder.Property(i => i.ID)
                .ValueGeneratedOnAdd();

            builder.Property(i => i.IsExecuted)
                  .IsRequired();

            builder.Property(i => i.Description)
                  .HasMaxLength(100)
                  .IsRequired();

            builder.HasOne(i => i.WorkPlan)
                .WithMany(p => p.Instructions)
                .HasForeignKey(i => i.WorkPlanID)
                .IsRequired();

            builder.Property(i => i.DeviceID)
               .IsRequired();

        }
    }
}
