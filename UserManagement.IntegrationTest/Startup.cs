using Application.Interfaces;
using Application.Services;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using SolidToken.SpecFlow.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.IntegrationTest.Services;

namespace UserManagement.IntegrationTest
{
    public class Startup
    {
        [ScenarioDependencies]
        public static IServiceCollection CreateService()
        {
            var services = new ServiceCollection();
            

           // services.Configure<MongoDbSettings>(services.GetSection("MongoDbSettings"));
            services.AddScoped<UserDbContext>();

            services.AddScoped<IUserService, UserService>();
            services.AddHttpClient();
            services.AddScoped<HttpClientService>();

            services.AddHttpContextAccessor();


            return services;
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddDbContext<ReservationDevContext>(options =>
        //        options.UseSqlServer("Server=redi-dev.database.windows.net,1433;Database=reservation-dev;User Id=red-dev-admin;Password=sWRW8bdS3CzFDOpyNwxA;TrustServerCertificate=True"));

        //    services.AddScoped<ReservationDevContext>();


        //    services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
        //    services.AddScoped<IReservation, ReservationRepo>();
        //    services.AddScoped<ICustomersRepository, CustomersRepository>();
        //    services.AddScoped<ICustomersService, CustomersService>();
        //    services.AddScoped<IPlaces, PlacesRepo>();
        //    services.AddScoped<IReservation, ReservationRepo>();

        //    services.AddScoped<IDailyNotesRepo, DailyNotesRepo>();
        //    services.AddScoped<IDailyNotesService, DailyNotesService>();

        //}

        ////public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        ////{
        ////    // Configure middleware pipeline if necessary
        ////}
    }
}
