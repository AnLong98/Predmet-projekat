using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using SmartEnergy.Documents.DomainModels;
using SmartEnergy.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartEnergy.Infrastructure
{
    public class DocumentsDbContext : DbContext
    {
        public DocumentsDbContext(DbContextOptions options) : base(options)
        {
        }

        public bool Exists()
        {
            return (this.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();
        }

        public DbSet<Call> Calls { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<MultimediaAnchor> MultimediaAnchors { get; set; }
        public DbSet<MultimediaAttachment> MultimediaAttachments { get; set; }
        public DbSet<NotificationAnchor> NotificationAnchors { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<SafetyDocument> SafetyDocuments { get; set; }
        public DbSet<StateChangeAnchor> StateChangeAnchors { get; set; }
        public DbSet<StateChangeHistory> StateChangeHistories { get; set; }
        public DbSet<WorkPlan> WorkPlans { get; set; }
        public DbSet<WorkRequest> WorkRequests { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DocumentsDbContext).Assembly);

        }
    }
}
