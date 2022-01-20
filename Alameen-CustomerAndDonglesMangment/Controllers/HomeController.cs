using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Alameen_CustomerAndDonglesMangment.Models;
using Alameen_CustomerAndDonglesMangment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Alameen_CustomerAndDonglesMangment.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;


        public HomeController(ILogger<HomeController> logger , ApplicationDbContext context , UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
           await End();

            User user = await _userManager.GetUserAsync(User);
            int? BranchId = user.BranchId;

            ViewBag.CountCustomer = _context.Customers.Where(i=>i.BranchId==BranchId).ToList().Count;
            ViewBag.CountDongles = _context.Dongles.Where(i => i.BranchId == BranchId).ToList().Count;

            return View();
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


        // To End Dongles
        public async Task End()
        {
            User user = await _userManager.GetUserAsync(User);
            int? BranchId = user.BranchId;

            ViewData["dayend"] = _context.Dongles.Where(i=>i.BranchId==BranchId && i.ExpirationDate.Value.Date >= DateTime.Now.Date && i.ExpirationDate <= DateTime.Now.Date.AddDays(20)).ToListAsync().Result.Count;
        }
    }
}
