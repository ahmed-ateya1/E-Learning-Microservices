using MediatR;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Command
{
    public class DeleteCourseCommand : IRequest<bool>
    {
        public Guid CourseID { get; set; }

        public DeleteCourseCommand(Guid courseID)
        {
            CourseID = courseID;
        }
    }
}
