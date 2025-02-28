using AutoMapper;
using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;

namespace CoursesManagmentMicroservices.Core.MappingProfile.CourseProfile
{
    public class CourseMappingProfile : Profile
    {
        public CourseMappingProfile()
        {
           CreateMap<CourseAddRequest,Course>()
                .ForMember(dest=>dest.CourseID, opt => opt.Ignore())
                .ForMember(dest=>dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
                .ReverseMap();

            CreateMap<CourseUpdateRequest, Course>()
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt =>opt.MapFrom(src=>DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Course, CourseResponse>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ReverseMap();

        }
    }
}
