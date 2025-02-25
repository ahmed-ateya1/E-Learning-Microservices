namespace CoursesManagmentMicroservices.Core.Dtos.CategoryDto
{
    public class CategoryUpdateRequest
    {
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
    }
}
