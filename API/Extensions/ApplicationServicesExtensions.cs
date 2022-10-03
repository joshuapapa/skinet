using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services){

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.Configure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = ActionContext => {
                    var errors = ActionContext.ModelState
                                    .Where(e=>e.Value.Errors.Count > 0)
                                    .SelectMany(x => x.Value.Errors)
                                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse{
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;
        }
    }
}