using CoursesManagmentMicroservices.Core.Caching;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using CoursesManagmentMicroservices.Core.MediatRConfig.Command;
using CoursesManagmentMicroservices.Core.ServiceContract;
using MediatR;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Handler.CourseHandler
{
    public class AddCourseCommandHandler : IRequestHandler<AddCourseCommand, CourseResponse>
    {
        private readonly ICourseService _courseService;
        private readonly ICacheService _cacheService;

        public AddCourseCommandHandler(ICourseService courseService, ICacheService cacheService)
        {
            _courseService = courseService;
            _cacheService = cacheService;
        }

        public async Task<CourseResponse> Handle(AddCourseCommand request, CancellationToken cancellationToken)
        {
            var result = await _courseService.CreateAsync(request.CourseAddRequest);
            if (result != null)
            {
                await _cacheService.SetAsync($"course:{result.CourseID}", result);
            }
            return result;
        }
    }
}
