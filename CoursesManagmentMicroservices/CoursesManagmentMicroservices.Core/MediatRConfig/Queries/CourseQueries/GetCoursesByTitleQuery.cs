using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using MediatR;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Queries.CourseQueries
{
    public class GetCoursesByTitleQuery : IRequest<PaginatedResponse<CourseResponse>>
    {
        public GetCoursesByTitleQuery(string title, PaginationDto? pagination)
        {
            Title = title;
            Pagination = pagination;
        }

        public string Title { get; set; }
        public PaginationDto? Pagination { get; set; }

    }
}
