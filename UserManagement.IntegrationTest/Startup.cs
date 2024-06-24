using Application.Interfaces;
using Application.Repository;
using Application.Services;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
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

            // Register MongoDB settings from configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();


            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
            services.AddScoped<UserDbContext>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddHttpClient();
            services.AddScoped<HttpClientService>();

            //services.AddHttpContextAccessor();


            return services;
        }

        
    }
}
