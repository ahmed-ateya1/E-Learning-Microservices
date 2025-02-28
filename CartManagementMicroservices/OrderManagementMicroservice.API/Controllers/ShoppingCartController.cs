using CartManagementMicroservice.BusinessLayer.ServiceContract;
using CartManagementMicroservice.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CartManagementMicroservice.API.Controllers
{
    /// <summary>
    /// Controller responsible for managing shopping cart operations such as retrieving, adding, removing, and clearing cart items.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartController"/> class.
        /// </summary>
        /// <param name="shoppingCartService">Service for shopping cart operations.</param>
        /// <param name="httpContextAccessor">Accessor for HTTP context to retrieve user information.</param>
        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            IHttpContextAccessor httpContextAccessor)
        {
            _shoppingCartService = shoppingCartService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Retrieves the shopping cart for a specified user.
        /// </summary>
        /// <param name="userID">The unique identifier of the user whose cart is to be retrieved.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user's shopping cart data.</returns>
        /// <response code="200">Returns the shopping cart for the specified user.</response>
        /// <response code="401">Returned if the request is not authorized.</response>
        [HttpGet("getCart/{userID}")]
        public async Task<IActionResult> GetCart(Guid userID, CancellationToken cancellationToken)
        {
            var cart = await _shoppingCartService.GetCartAsync(userID.ToString(), cancellationToken);
            return Ok(cart);
        }

        /// <summary>
        /// Adds an item to the user's shopping cart.
        /// </summary>
        /// <param name="item">The cart item to be added, provided in the request body.</param>
        /// <param name="userID">The unique identifier of the user whose cart will be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        /// <response code="200">Returns a success message indicating the item was added to the cart.</response>
        /// <response code="401">Returned if the request is not authorized.</response>
        /// <response code="400">Returned if the provided item data is invalid.</response>
        [HttpPost("add/{userID}")]
        public async Task<IActionResult> AddToCart([FromBody] CartItems item, Guid userID, CancellationToken cancellationToken)
        {
            await _shoppingCartService.AddToCartAsync(userID.ToString(), item, cancellationToken);
            return Ok(new { Message = "Item added to cart" });
        }

        /// <summary>
        /// Removes a specific item from the user's shopping cart.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose cart will be modified.</param>
        /// <param name="courseID">The unique identifier of the course to be removed from the cart.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        /// <response code="200">Returns a success message indicating the item was removed from the cart.</response>
        /// <response code="401">Returned if the request is not authorized.</response>
        /// <response code="404">Returned if the specified item is not found in the cart.</response>
        [HttpDelete("remove/{courseID}")]
        public async Task<IActionResult> RemoveFromCart(string userId, Guid courseID, CancellationToken cancellationToken)
        {
            await _shoppingCartService.RemoveFromCartAsync(userId, courseID, cancellationToken);
            return Ok(new { Message = "Item removed from cart" });
        }

        /// <summary>
        /// Clears all items from the user's shopping cart.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose cart will be cleared.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        /// <response code="200">Returns a success message indicating the cart was cleared.</response>
        /// <response code="401">Returned if the request is not authorized.</response>
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart(string userId, CancellationToken cancellationToken)
        {
            await _shoppingCartService.ClearCartAsync(userId, cancellationToken);
            return Ok(new { Message = "Cart cleared" });
        }
    }
}