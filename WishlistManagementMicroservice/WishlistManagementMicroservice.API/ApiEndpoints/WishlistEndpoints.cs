using WishlistManagementMicroservice.BusinessLayer.Dtos;
using WishlistManagementMicroservice.BusinessLayer.ServiceContract;

namespace WishlistManagementMicroservice.API.ApiEndpoints
{
    public static class WishlistEndpoints
    {
        
        public static IEndpointRouteBuilder MapWishlistEndpoints(this IEndpointRouteBuilder app)
        {
           
            app.MapPost("/api/wishlist/addWishlist", async (IWishlistService wishlistService , WishlistAddRequest request) =>
            {
                var response = await wishlistService.CreateAsync(request);
                return Results.Ok(response);
            });
            app.MapDelete("/api/wishlist/deleteWishlist/{wishlistID}", async (IWishlistService wishlistService, Guid wishlistID) =>
            {
                var response = await wishlistService.DeleteAsync(wishlistID);
                return Results.Ok(response);
            });
            app.MapGet("/api/wishlist/getWishlistById/{wishlistID}", async (IWishlistService wishlistService, Guid wishlistID) =>
            {
                var response = await wishlistService.GetWishlistByIdAsync(wishlistID);
                return Results.Ok(response);
            });
            app.MapGet("/api/wishlist/getWishlistByUserId/{userID}", async (IWishlistService wishlistService, Guid userID) =>
            {
                var response = await wishlistService.GetWishlistByUserIdAsync(userID);
                return Results.Ok(response);
            });
            app.MapGet("/api/wishlist/getWishlistByCourseId/{courseID}", async (IWishlistService wishlistService, Guid courseID) =>
            {
                var response = await wishlistService.GetWishlistByCourseIdAsync(courseID);
                return Results.Ok(response);
            });
            app.MapGet("/api/wishlist/getAllWishlists", async (IWishlistService wishlistService) =>
            {
                var response = await wishlistService.GetAllAsync();
                return Results.Ok(response);
            });

            return app;
        }
    }
}
