using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using MediatR;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Command
{
    public class UpdateCourseCommand : IRequest<CourseResponse>
    {
        public CourseUpdateRequest _courseUpdateRequest { get; set; }

        public UpdateCourseCommand(CourseUpdateRequest courseUpdateRequest)
        {
            _courseUpdateRequest = courseUpdateRequest;
        }
    }
}
