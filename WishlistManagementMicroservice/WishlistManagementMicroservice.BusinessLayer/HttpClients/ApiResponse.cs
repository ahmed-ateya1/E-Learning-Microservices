using System.Net;

namespace WishlistManagementMicroservice.BusinessLayer.HttpClients
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
    }
}
