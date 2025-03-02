using AutoMapper;
using EnrollmentManagementMicroservice.BusinessLayer.Dtos;
using EnrollmentManagementMicroservice.DataAccessLayer.Entities;

namespace EnrollmentManagementMicroservice.BusinessLayer.MappingProfile
{
    public class EnrollmentMappingProfile : Profile
    {
        public EnrollmentMappingProfile()
        {
            CreateMap<Enrollment, EnrollmentAddRequest>().ReverseMap();
            CreateMap<Enrollment, EnrollmentUpdateRequest>().ReverseMap();
            CreateMap<Enrollment, EnrollmentResponse>().ReverseMap();
        }
    }
}
