using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kursach_CorpHubPortal.Data;
using System.Security.Claims;

[Authorize]
public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim)) return NotFound();

        return await GetUserProfile(int.Parse(userIdClaim));
    }

    [HttpGet("/Profile/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        return await GetUserProfile(id);
    }

    private async Task<IActionResult> GetUserProfile(int id)
    {
        var user = await _context.Users
            .Include(u => u.Department)
            .Include(u => u.Position)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return NotFound();

        var currentUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        ViewBag.IsOwnProfile = (currentUserIdClaim != null && id == int.Parse(currentUserIdClaim));

        return View("Index", user);
    }
}