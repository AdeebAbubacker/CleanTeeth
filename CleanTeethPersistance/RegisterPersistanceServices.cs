using CleanTeeth.Application.Contracts.Repositories;
using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethPersistance.Repositories;
using CleanTeethPersistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTeethPersistance
{
    public static class RegisterPersistanceServices
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<CleanTeethDbContext>(options =>
                options.UseSqlServer("Server=ADEEB\\SQLEXPRESS;Database=CleanTeethDB;TrustServerCertificate=True;Trusted_Connection=True"));
            services.AddScoped<IDentalOfficeRepository, DentalOfficeRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDentistRepositories, DentistRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWorkEfCore>();
            return services;
        }
    }
}
