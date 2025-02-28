using CoursesManagmentMicroservices.Core.Domain.Entities;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using MediatR;
using System.Linq.Expressions;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Queries.CourseQueries
{
    public class GetCourseQuery : IRequest<CourseResponse>
    {
        public Guid CourseID { get;}

        public GetCourseQuery(Guid courseID)
        {
            CourseID = courseID;
        }
    }
}
