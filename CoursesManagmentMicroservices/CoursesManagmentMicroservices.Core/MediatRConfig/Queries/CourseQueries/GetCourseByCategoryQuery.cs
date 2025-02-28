using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using MediatR;
using System.Linq.Expressions;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Queries.CourseQueries
{
    public class GetCourseByCategoryQuery : IRequest<PaginatedResponse<CourseResponse>>
    {
        public Guid CategoryID { get; }
        public PaginationDto? Pagination { get; set; } 
        public GetCourseByCategoryQuery(Guid categoryID, PaginationDto pagination)
        {
            CategoryID = categoryID;
            Pagination = pagination;
        }
    }
}
