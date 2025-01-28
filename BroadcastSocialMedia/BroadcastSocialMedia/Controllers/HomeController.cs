using System.Diagnostics;
using BroadcastSocialMedia.Data;
using BroadcastSocialMedia.Models;
using BroadcastSocialMedia.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BroadcastSocialMedia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var dbUser = await _dbContext.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();

            var broadcasts = await _dbContext.Users.Where(u => u.Id == user.Id)
                .SelectMany(u => u.ListeningTo)
                .SelectMany(u => u.Broadcasts)
                .Include(b => b.User)
                .OrderByDescending(b => b.Published)
                .ToListAsync();

            var viewModel = new HomeIndexViewModel()
            {
                Broadcasts = broadcasts
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Broadcast(HomeBroadcastViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var broadcast = new Broadcast
            {
                Message = viewModel.Message,
                User = user
            };

            if (viewModel.Image != null && viewModel.Image.Length > 0) // This part is extra
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(viewModel.Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.Image.CopyToAsync(stream);
                }

                broadcast.ImageUrl = "/uploads/" + fileName;
            }

            _dbContext.Broadcasts.Add(broadcast);

            await _dbContext.SaveChangesAsync();

            return Redirect("/");
        }
    }
}
