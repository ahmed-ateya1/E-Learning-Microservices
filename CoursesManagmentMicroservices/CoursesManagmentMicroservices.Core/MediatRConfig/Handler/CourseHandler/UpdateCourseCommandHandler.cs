using CoursesManagmentMicroservices.Core.Caching;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using CoursesManagmentMicroservices.Core.MediatRConfig.Command;
using CoursesManagmentMicroservices.Core.ServiceContract;
using MediatR;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Handler.CourseHandler
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, CourseResponse>
    {
        private readonly ICourseService _courseService;
        private readonly ICacheService _cacheService;
        public UpdateCourseCommandHandler(ICourseService courseService, ICacheService cacheService)
        {
            _courseService = courseService;
            _cacheService = cacheService;
        }
        public async Task<CourseResponse> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var result =  await _courseService.UpdateAsync(request._courseUpdateRequest);
            if(result != null)
            {
                await _cacheService.RemoveAsync($"course:{result.CourseID}");
                await _cacheService.SetAsync($"course:{result.CourseID}", result);
            }
            return result;
        }
    }
}
