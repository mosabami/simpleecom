//using Microsoft.Azure.CosmosRepository.ChangeFeed;
//using Microsoft.Azure.CosmosRepository;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Simpleecom.Shared.Models;

//namespace Simpleecom.Shared.Processors
//{
  
//    public class ProductChangeFeedProcessor(ILogger<ProductChangeFeedProcessor> logger,
//    IRepository<Product> productRepository) : IItemChangeFeedProcessor<Product>
//    {
//        public async ValueTask HandleAsync(Product product, CancellationToken cancellationToken)
//        {
//            logger.LogInformation("Change detected for Product with ID: {ProductId}", product.Id);

//            //if (!rating.HasBeenUpdated)
//            //{
//            //    await bookByIdReferenceRepository
//            //        .CreateAsync(new BookByIdReference(rating.Id, rating.Category),
//            //        cancellationToken);
//            //}

//            logger.LogInformation("Processed change for product with ID: {ProductId}", product.Id);
//        }
//    }
//}
