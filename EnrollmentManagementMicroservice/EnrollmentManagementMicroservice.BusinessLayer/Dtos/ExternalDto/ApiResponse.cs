﻿using System.Net;

namespace EnrollmentManagementMicroservice.BusinessLayer.Dtos.ExternalDto
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
    }
}
