using CoursesManagmentMicroservices.Core.Caching;
using CoursesManagmentMicroservices.Core.Dtos;
using CoursesManagmentMicroservices.Core.Dtos.CourseDto;
using CoursesManagmentMicroservices.Core.MediatRConfig.Queries.CourseQueries;
using CoursesManagmentMicroservices.Core.ServiceContract;
using MediatR;

namespace CoursesManagmentMicroservices.Core.MediatRConfig.Handler.CourseHandler
{
    public class GetCoursesByTitleQueryHandler : IRequestHandler<GetCoursesByTitleQuery, PaginatedResponse<CourseResponse>>
    {
        private readonly ICourseService _courseService;
        private readonly ICacheService _cacheService;

        public GetCoursesByTitleQueryHandler(ICourseService courseService, ICacheService cacheService)
        {
            _courseService = courseService;
            _cacheService = cacheService;
        }

        public async Task<PaginatedResponse<CourseResponse>> Handle(GetCoursesByTitleQuery request, CancellationToken cancellationToken)
        {
            request.Pagination ??= new PaginationDto();

            return await _cacheService.GetAsync($"GetAllCoursesByTitle:{request.Title.ToLower()}{request.Pagination.PageIndex}" +
                $"{request.Pagination.PageSize}" +
                $"{request.Pagination.SortDirection}" +
                $"{request.Pagination.SortBy}", async () =>
                {
                    return await _courseService.GetAllAsync(x => x.Title.ToLower()
                    .Contains(request.Title.ToLower()) && x.IsPublic == true, request.Pagination);
                }, cancellationToken);
        }
    }
}
