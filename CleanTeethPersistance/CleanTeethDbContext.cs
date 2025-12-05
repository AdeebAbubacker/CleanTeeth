using CleanTeeth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CleanTeethPersistance
{
    public class CleanTeethDbContext : DbContext
    {
        public CleanTeethDbContext(DbContextOptions options) : base(options)
        {
        }

        protected CleanTeethDbContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CleanTeethDbContext).Assembly);
        }

        public DbSet<DentalOffice> DentalOffices { get; set; }
        public DbSet<Patient> Patients { get; set; }

        //----------------Dentist to be done

        public DbSet<Appointment> Appoinments { get; set; }

    }
}
