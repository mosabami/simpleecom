using Simpleecom.Shared.Options;
using Simpleecom.Shared.Processors;
using Simpleecom.Shared.Repositories;

namespace Simpleecom.Products.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            
            builder.Services.AddEndpointsApiExplorer();

            var ev = System.Environment.GetEnvironmentVariables();

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.Configure<CosmosDbOptions>(builder.Configuration.GetSection("CosmosDbOptions"));

            builder.Services.AddSingleton<IProductChangeFeedProcessor, ProductChangeFeedProcessor>();

            builder.Services.AddScoped(typeof(CosmosDBRepository<>));

            builder.Services.AddSwaggerGen();

            var SimpleecomCorsPolicy = "_simpleecomCorsPolicy";


            builder.Services.AddCors(options =>
            {
                options.AddPolicy(SimpleecomCorsPolicy, policy =>{ policy.AllowAnyOrigin(); });
            });

            var app = builder.Build();

            await app.Services.GetRequiredService<IProductChangeFeedProcessor>().InitializeAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseCors(SimpleecomCorsPolicy);

            app.MapControllers();


            app.Run();
        }
    }
}
