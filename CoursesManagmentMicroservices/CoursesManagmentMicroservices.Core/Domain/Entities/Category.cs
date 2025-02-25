namespace CoursesManagmentMicroservices.Core.Domain.Entities
{
    public class Category
    {
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
        public Guid? BaseCategoryID { get; set; }
        public virtual Category BaseCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
