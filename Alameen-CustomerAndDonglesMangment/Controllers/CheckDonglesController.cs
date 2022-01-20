using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alameen_CustomerAndDonglesMangment.Data;
using Alameen_CustomerAndDonglesMangment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alameen_CustomerAndDonglesMangment.Controllers
{
    public class CheckDonglesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CheckDonglesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(double Day=20)
        {
            await End();

            User user = await _userManager.GetUserAsync(User);

            var applicationDbContext = _context.Dongles.Where(i => i.BranchId == user.BranchId && i.ExpirationDate >= DateTime.Now.Date && i.ExpirationDate <= DateTime.Now.Date.AddDays(Day))
                .Include(d => d.AmeenAddOnTypes).Include(d => d.AmeenVer)
                .Include(d => d.Branch).Include(d => d.CustBran)
                .Include(d => d.DongleColor).Include(d => d.DongleType)
                .Include(d => d.Representatives).Include(d => d.Customer)
                .Include(i => i.Customer.State).Include(i => i.Customer.City);

            ViewBag.Day = Day;

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> ExpiredDongles()
        {
            await End();

            User user = await _userManager.GetUserAsync(User);

            var applicationDbContext = _context.Dongles.Where(i => i.BranchId == user.BranchId && i.ExpirationDate <= DateTime.Now.Date )
                .Include(d => d.AmeenAddOnTypes).Include(d => d.AmeenVer)
                .Include(d => d.Branch).Include(d => d.CustBran)
                .Include(d => d.DongleColor).Include(d => d.DongleType)
                .Include(d => d.Representatives).Include(d => d.Customer).Include(i => i.Customer.State).Include(i => i.Customer.City);

            return View(await applicationDbContext.ToListAsync());
        }


        // To End Dongles
        public async Task End()
        {
            User user = await _userManager.GetUserAsync(User);
            int? BranchId = user.BranchId;

            ViewData["dayend"] = _context.Dongles.Where(i => i.BranchId == BranchId && i.ExpirationDate.Value.Date >= DateTime.Now.Date && i.ExpirationDate <= DateTime.Now.Date.AddDays(20)).ToListAsync().Result.Count;
        }
    }
}