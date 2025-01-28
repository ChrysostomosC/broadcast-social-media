using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BroadcastSocialMedia.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var viewModel = new ProfileIndexViewModel()
            {
                Name = user.Name ?? ""
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileIndexViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            user.Name = viewModel.Name;

            await _userManager.UpdateAsync(user);

            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(ProfileIndexViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (viewModel.ProfileImage != null && viewModel.ProfileImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.ProfileImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.ProfileImage.CopyToAsync(stream);
                }

                user.ProfilePicture = "/uploads/" + fileName;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("Index");
        }

    }
}
