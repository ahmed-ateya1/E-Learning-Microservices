using AutoMapper;
using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Dtos.CategoryDto;

namespace CoursesManagmentMicroservices.Core.MappingProfile.CategoryProfile
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryAddRequest,Category>()
                .ForMember(dest=>dest.CategoryName,opt=>opt.MapFrom(src=>src.CategoryName))
                .ForMember(dest=>dest.BaseCategoryID, opt => opt.MapFrom(src => src.BaseCategoryID))
                .ReverseMap();

            CreateMap<CategoryUpdateRequest,Category>()
                .ForMember(dest => dest.CategoryID, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ReverseMap();

            CreateMap<Category, CategoryResponse>()
                .ForMember(dest => dest.CategoryID, opt => opt.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.BaseCategoryID, opt => opt.MapFrom(src => src.BaseCategoryID))
                .ForMember(dest => dest.BaseCategoryName, opt => opt.MapFrom(src => src.BaseCategory.CategoryName))
                .ForMember(dest => dest.NumberOfCourses, opt => opt.MapFrom(src => src.Courses.Count))
                .ReverseMap();

        }
    }
}
