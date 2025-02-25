namespace CoursesManagmentMicroservices.Core.Dtos.CategoryDto
{
    public class CategoryResponse
    {
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
        public Guid? BaseCategoryID { get; set; }
        public string? BaseCategoryName { get; set; }
        public long NumberOfCourses { get; set; }
    }
}
