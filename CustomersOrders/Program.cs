using AutoMapper;
using CustomersOrders.Classes;
using CustomersOrders.Handlers.Customers;
using CustomersOrders.Models;
using CustomersOrders.Repositories;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace CustomersOrders
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            mapperConfig.AssertConfigurationIsValid();
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            builder.Services.AddDbContext<AppDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer("Server=.;Database=CustomersOrders;Trusted_Connection=True;TrustServerCertificate=True;"));
            
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            
            builder.Services.AddScoped<CreateCustomerCommandHandler>();
            builder.Services.AddScoped<GetAllCustomersQueryHandler>();
            builder.Services.AddScoped<GetCustomerQueryHandler>();
            builder.Services.AddScoped<SearchCustomersQueryHandler>();
            builder.Services.AddScoped<UpdateCustomerCommandHandler>();
            
            builder.Services.AddScoped<MailMessage>();
            
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddControllers();
                    
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomersOrders")
            );

            app.MapControllers();

            app.Run();
        }
    }
}
