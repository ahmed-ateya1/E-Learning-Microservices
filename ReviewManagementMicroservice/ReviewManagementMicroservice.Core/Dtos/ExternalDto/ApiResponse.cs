using System.Net;

namespace ReviewManagementMicroservice.Core.Dtos.ExternalDto
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
    }
}
