using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using System.Collections.Generic;

namespace PMS.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Patient> Patients { get; set; }

        public DbSet<PatientRecord> PatientRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Patient>()

            .HasMany(p => p.Records)

            .WithOne(pr => pr.Patient)

            .HasForeignKey(pr => pr.PatientId)

            .OnDelete(DeleteBehavior.Cascade);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=patient.db");
        }
    }
}
