using CleanTeeth.Infrastructure.Notifications;
using CleanTeethApplication.Notifications;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeeth.Infrastructure
{
    public static class RegisterInfraStructureServices
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services) 
        { 
            services.AddScoped<INotifications,EmailServicecs>();
            return services;
        }
    }
}
