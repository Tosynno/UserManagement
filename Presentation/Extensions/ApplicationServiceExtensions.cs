using Application.Interfaces;
using Application.Models;
using Application.Repository;
using Application.Services;
using Application.Validations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Presentation.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<HtmlEncoder>(
                HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                                               UnicodeRanges.CjkUnifiedIdeographs }));
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
            services.Configure<MongoDbSettings>(config.GetSection("MongoDbSettings"));
            services.AddScoped<UserDbContext>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IValidator<UserRequest>, UserRequestValidator>();
            services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserRequestValidator>();
            return services; 
        }
    }
}
