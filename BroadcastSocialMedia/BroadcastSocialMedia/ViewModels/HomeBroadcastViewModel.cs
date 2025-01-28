namespace BroadcastSocialMedia.ViewModels
{
    public class HomeBroadcastViewModel
    {
        public string Message { get; set; }
        public IFormFile? Image { get; set; }  // For uploading images
        public string? ImageUrl { get; set; }  // Store the image path
    }
}
