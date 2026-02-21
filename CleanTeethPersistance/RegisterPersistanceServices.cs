using CleanTeeth.Application.Contracts.Repositories;
using CleanTeethApplication.Contracts.Persistance;
using CleanTeethApplication.Contracts.Repositories;
using CleanTeethApplication.Features.Auth.Services;
using CleanTeethPersistance.Auth;
using CleanTeethPersistance.Repositories;
using CleanTeethPersistance.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanTeethPersistance
{
    public static class RegisterPersistanceServices
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<CleanTeethDbContext>(options =>
                options.UseInMemoryDatabase("CleanTeethDb"));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<CleanTeethDbContext>()
            .AddDefaultTokenProviders();

            // Register auth adapters
            services.AddScoped<IUserManager, UserManagerAdapter>();
            services.AddScoped<IRoleManager, RoleManagerAdapter>();

            services.AddScoped<IDentalOfficeRepository, DentalOfficeRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDentistRepositories, DentistRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWorkEfCore>();

            return services;
        }
    }
}