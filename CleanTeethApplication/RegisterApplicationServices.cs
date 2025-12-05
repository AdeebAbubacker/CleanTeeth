using CleanTeethApplication.Features.DentalOffices.Commands.CreateDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Commands.DeleteDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Commands.UpdateDentalOffice;
using CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeDetail;
using CleanTeethApplication.Features.DentalOffices.Queries.GetDentalOfficeList;
using CleanTeethApplication.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanTeethApplication
{
    public static class RegisterApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) { 
            services.AddTransient<IMediator,SimpleMediator>();

            //scanning all classes in application layer and adding to di system instead of below manual method
            //for this we use package called Scrutor
            services.Scan(scan => scan.FromAssembliesOf(typeof(RegisterApplicationServices)).AddClasses(c=>c.AssignableTo(
                typeof(IRequestHandler<>))).AsImplementedInterfaces().WithScopedLifetime().AddClasses(c=> c.AssignableTo(
                    typeof(IRequestHandler<,>))).AsImplementedInterfaces().WithScopedLifetime());


            //services.AddScoped<IRequestHandler<CreateDentalOfficeCommand, Guid>, CreateDentalOfficeCommandHandler>();
            //services.AddScoped<IRequestHandler<GetDentalOfficeDetailQuery, DentalOfficeDetailDTO>, GetDentalOfficeQueryHandler>();
            //services.AddScoped<IRequestHandler<GetDentalOfficeListQuery, List<GetDentalOfficeListDTO>>, GetDentalOfficeListQueryHandler>();
            //services.AddScoped<IRequestHandler<UpdateDentalOfficeCommand>, UpdateDentalOfficeCommandHandler>();
            //services.AddScoped<IRequestHandler<DeleteDentalOfficeCommand>, DeleteDentalOfficeCommandHandler>();
            return services;
        }
    }
}
