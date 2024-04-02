
using Microsoft.Azure.CosmosRepository.AspNetCore.Extensions;
using Microsoft.Azure.CosmosRepository.ChangeFeed;
using Microsoft.Azure.CosmosRepository.Options;
using Simpleecom.Shared.Models;
using Simpleecom.Shared.Options;
using Simpleecom.Shared.Processors;
using Simpleecom.Shared.Services;
using System.Configuration;

namespace Simpleecom.Carts.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var repositoryOptions = builder.Configuration.GetSection(nameof(Shared.Options.RepositoryOptions)).Get<Shared.Options.RepositoryOptions>();
            var containerOptionsValue = builder.Configuration.GetSection("RepositoryOptions:ContainerOptions").Get<ContainerOptions>();


            builder.Services.AddCosmosRepository(options =>
            {
                options.CosmosConnectionString = repositoryOptions.ConnectionString;
                options.DatabaseId = repositoryOptions.DatabaseId;
                options.ContainerPerItemType = repositoryOptions.ContainerPerItemType;
                options.IsAutoResourceCreationIfNotExistsEnabled = repositoryOptions.IsAutoResourceCreationIfNotExistsEnabled;

                options.ContainerBuilder.Configure<Product>(containerOptions =>
                {
                    containerOptions.WithContainer(containerOptionsValue.ContainerId);
                    containerOptions.WithPartitionKey(containerOptionsValue.PartitionKeyPath);
                    containerOptions.WithChangeFeedMonitoring();
                });

            });

            builder.Services.AddSingleton<IItemChangeFeedProcessor<Product>, ProductChangeFeedProcessor>();
            builder.Services.AddCosmosRepositoryChangeFeedHostedService();
            builder.Services.AddCosmosRepositoryItemChangeFeedProcessors(typeof(ProductChangeFeedProcessor).Assembly);



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
