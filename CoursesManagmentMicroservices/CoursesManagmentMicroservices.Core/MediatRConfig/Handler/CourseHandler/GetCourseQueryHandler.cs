using CoursesManagmentMicroservices.Core.Caching;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using CoursesManagmentMicroservices.Core.MediatRConfig.Queries.CourseQueries;
using CoursesManagmentMicroservices.Core.ServiceContract;
using MediatR;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Handler.CourseHandler
{
    public class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, CourseResponse>
    {
        private readonly ICourseService _courseService;
        private readonly ICacheService _cacheService;

        public GetCourseQueryHandler(ICourseService courseService, ICacheService cacheService)
        {
            _courseService = courseService;
            _cacheService = cacheService;
        }

        public async Task<CourseResponse> Handle(GetCourseQuery request, CancellationToken cancellationToken)
        {
            return await _cacheService.GetAsync(
                $"course:{request.CourseID}", async () =>
                {
                    return await _courseService.GetByAsync(x => x.CourseID == request.CourseID);
                }
                );
        }
    }
}
