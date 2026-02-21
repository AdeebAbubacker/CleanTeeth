using CleanTeeth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CleanTeethPersistance
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }

    public class CleanTeethDbContext : IdentityDbContext<ApplicationUser>
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
