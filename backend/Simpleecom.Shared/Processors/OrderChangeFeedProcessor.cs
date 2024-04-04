//using Microsoft.Azure.CosmosRepository;
//using Microsoft.Azure.CosmosRepository.ChangeFeed;
//using Microsoft.Extensions.Logging;
//using Simpleecom.Shared.Models;

//namespace Simpleecom.Shared.Processors
//{

//   public class OrderChangeFeedProcessor(ILogger<OrderChangeFeedProcessor> logger,
//    IRepository<Product> productRepository) : IItemChangeFeedProcessor<Order>
//    {
//        public async ValueTask HandleAsync(Order order, CancellationToken cancellationToken)
//        {
//            logger.LogInformation("Change detected for Order with ID: {OrderId}", order.Id);

//         if(!order.IsUpdate)
//            {
//                foreach (var product in order.Products)
//                {
//                    var productToUpdate = await productRepository.GetAsync(product.Id,null, cancellationToken);
//                    productToUpdate.Inventory -= order.Quantity;
//                    await productRepository.UpdateAsync(productToUpdate,true, cancellationToken);
//                }
//            }

//            logger.LogInformation("Processed change for order with ID: {OrderId}", order.Id);
//        }
//    }
//}
