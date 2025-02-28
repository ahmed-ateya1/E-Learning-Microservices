using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using MediatR;
using System.Linq.Expressions;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Queries.CourseQueries
{
    public class GetAllCoursesQuery : IRequest<PaginatedResponse<CourseResponse>>
    {
        public GetAllCoursesQuery( PaginationDto pagination)
        {
            Pagination = pagination;
        }

        public PaginationDto? Pagination { get; set; }

    }
}
