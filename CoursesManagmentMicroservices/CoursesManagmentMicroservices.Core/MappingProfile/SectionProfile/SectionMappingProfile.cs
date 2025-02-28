using AutoMapper;
using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Dtos.SectionDto;

namespace CoursesManagmentMicroservices.Core.MappingProfile.SectionProfile
{
    public class SectionMappingProfile : Profile
    {
        public SectionMappingProfile()
        {
            CreateMap<SectionAddRequest, Section>()
                .ForMember(dest => dest.CourseID, opt => opt.MapFrom(src => src.CourseID))
                .ForMember(dest => dest.SectionID, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<SectionUpdateRequest, Section>();
            CreateMap<Section, SectionResponse>()
                .ForMember(dest => dest.CourseID, opt => opt.MapFrom(src => src.Course.CourseID))
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title));
        }
    }
}
