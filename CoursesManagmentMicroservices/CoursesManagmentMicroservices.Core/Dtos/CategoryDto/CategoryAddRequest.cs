namespace CoursesManagmentMicroservices.Core.Dtos.CategoryDto
{
    public class CategoryAddRequest
    {
        public string CategoryName { get; set; }
        public Guid? BaseCategoryID { get; set; }
    }
}
