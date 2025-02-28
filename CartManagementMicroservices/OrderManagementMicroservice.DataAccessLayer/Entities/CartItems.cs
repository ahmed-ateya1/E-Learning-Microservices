namespace CartManagementMicroservice.DataAccessLayer.Entities
{
    public class CartItems
    {
        public Guid CourseID { get; set; }
        public string CourseName { get; set; }
        public decimal PriceBeforeDiscount { get; set; }
        public decimal PriceAfterDiscount { get; set; }
        public decimal Discount { get; set; }
        public string PictureUrl { get; set; }
        public string InstructorName { get; set; }
        public string CatgeoryName { get; set; }
    }
}
