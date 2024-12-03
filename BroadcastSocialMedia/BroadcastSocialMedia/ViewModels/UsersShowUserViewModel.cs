using BroadcastSocialMedia.Models;

namespace BroadcastSocialMedia.ViewModels
{
    public class UsersShowUserViewModel
    {
        public ApplicationUser  User { get; set; }
        public List<Broadcast> Broadcasts { get; set; }
        // Epidi auto to prop einai lista pame kai bazoume sto view tou ShowUser foreach-loop
    }
}
