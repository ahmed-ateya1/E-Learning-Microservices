using AutoMapper;
using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Dtos.LectureDto;

namespace CoursesManagmentMicroservices.Core.MappingProfile.LectureProfile
{
    public class LectureMappingProfile : Profile
    {
        public LectureMappingProfile()
        {
            CreateMap<Lecture, LectureResponse>()
                .ForMember(dest => dest.SectionTitle, opt => opt.MapFrom(src => src.Section.Title))
                .ForMember(dest => dest.SectionID, opt => opt.MapFrom(src => src.Section.SectionID));
            CreateMap<LectureAddRequest, Lecture>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest=>dest.LectureID, opt => opt.MapFrom(src => Guid.NewGuid()));
            CreateMap<LectureUpdateRequest, Lecture>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
   
}
