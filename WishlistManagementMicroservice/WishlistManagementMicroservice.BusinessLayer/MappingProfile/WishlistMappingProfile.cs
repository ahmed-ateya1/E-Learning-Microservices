using AutoMapper;
using WishlistManagementMicroservice.BusinessLayer.Dtos;
using WishlistManagementMicroservice.DataAccessLayer.Entities;

namespace WishlistManagementMicroservice.BusinessLayer.MappingProfile
{
    public class WishlistMappingProfile : Profile
    {
        public WishlistMappingProfile()
        {
            CreateMap<WishlistAddRequest, Wishlist>();

            CreateMap<WishlistUpdateRequest, Wishlist>();

            CreateMap<Wishlist, WishlistResponse>();
        }
    }
}
