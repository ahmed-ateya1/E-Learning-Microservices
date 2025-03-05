using AutoMapper;
using ReviewManagementMicroservice.Core.Domian.Entities;
using ReviewManagementMicroservice.Core.Dtos;

namespace ReviewManagementMicroservice.Core.MappingProfile
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<ReviewAddRequest , Review>()
                .ForMember(dest => dest.ReviewID, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Review, ReviewResponse>()
                .ReverseMap();

            CreateMap<ReviewUpdateRequest, Review>()
                .ReverseMap();
        }
    }
}
