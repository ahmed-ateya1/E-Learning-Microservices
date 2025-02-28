using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using MediatR;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Queries.CourseQueries
{
    public class GetCoursesByUserQuery : IRequest<PaginatedResponse<CourseResponse>>
    {
        public GetCoursesByUserQuery(Guid userID, PaginationDto pagination)
        {
            UserID = userID;
            Pagination = pagination;
        }

        public Guid UserID { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
