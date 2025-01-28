namespace BroadcastSocialMedia.ViewModels
{
    public class ProfileIndexViewModel
    {
        public string Name { get; set; }
        public string? ProfilePicture { get; set; }  // Image path stored in DB
        public IFormFile? ProfileImage { get; set; }  // For upload
    }
}
