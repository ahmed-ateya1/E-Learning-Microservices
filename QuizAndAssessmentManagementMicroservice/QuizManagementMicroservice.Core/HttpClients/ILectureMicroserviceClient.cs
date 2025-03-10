using QuizManagementMicroservice.Core.Dtos.ExternalDto;

namespace QuizManagementMicroservice.Core.HttpClients
{
    public interface ILectureMicroserviceClient
    {
        Task<LectureDto?> GetLectureAsync(Guid lectureId);
    }
}
