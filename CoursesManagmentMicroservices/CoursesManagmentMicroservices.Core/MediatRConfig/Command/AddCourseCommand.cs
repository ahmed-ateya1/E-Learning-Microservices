using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using MediatR;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Command
{
    public class AddCourseCommand : IRequest<CourseResponse>
    {
        public CourseAddRequest CourseAddRequest { get; set; }

        public AddCourseCommand(CourseAddRequest courseAddRequest)
        {
            CourseAddRequest = courseAddRequest;
        }
    }
}
