namespace WishlistManagementMicroservice.BusinessLayer.Dtos.ExternalDto
{
    public class UserDto
    {
        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PictureUrl { get; set; }
    }
}
