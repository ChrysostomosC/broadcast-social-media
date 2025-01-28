using Microsoft.AspNetCore.Identity;

namespace BroadcastSocialMedia.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public ICollection<Broadcast> Broadcasts { get; set; }
        public ICollection<ApplicationUser> ListeningTo { get; set; } = new List<ApplicationUser>();
        public string? ProfilePicture { get; set; } // Nullable to prevent errors
    }
}
