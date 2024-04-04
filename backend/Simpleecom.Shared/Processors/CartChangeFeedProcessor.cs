//using Microsoft.Azure.CosmosRepository;
//using Microsoft.Azure.CosmosRepository.ChangeFeed;
//using Microsoft.Extensions.Logging;
//using Simpleecom.Shared.Constants;
//using Simpleecom.Shared.Models;


//namespace Simpleecom.Shared.Processors
//{
//    public class CartChangeFeedProcessor(ILogger<CartChangeFeedProcessor> logger,
//    IRepository<Order> orderRepository) : IItemChangeFeedProcessor<Cart>
//    {
//        public async ValueTask HandleAsync(Cart cart, CancellationToken cancellationToken)
//        {
//            logger.LogInformation("Change detected for Cart with ID: {CartId}", cart.Id);

//            if(cart.Status == Status.Completed)
//            {
//                logger.LogInformation("Cart with ID: {CartId} has been completed", cart.Id);
//                await orderRepository.CreateAsync(new Order(cart.Total,Status.New,cart.UserId,cart.Products,false), cancellationToken);
//            }

//            logger.LogInformation("Processed change for Cart with ID: {CartId}", cart.Id);
//        }
//    }
//}
