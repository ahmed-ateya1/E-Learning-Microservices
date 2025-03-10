using AutoMapper;
using QuizManagementMicroservice.Core.Domain.Entities;
using QuizManagementMicroservice.Core.Dtos.QuizzDto;

namespace QuizManagementMicroservice.Core.MappingProfile
{
    public class QuizzMappingProfile : Profile
    {
        public QuizzMappingProfile()
        {
            CreateMap<QuizzAddRequest, Quizz>()
                .ForMember(dest => dest.InstructorID, opt => opt.MapFrom(src => src.InstructorID))
                .ForMember(dest => dest.QuizzID, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();
            CreateMap<QuizzUpdateRequest, Quizz>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();
            CreateMap<Quizz , QuizzResponse>()
                .ReverseMap();
        }
    }
}
