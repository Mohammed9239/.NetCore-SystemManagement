using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Alameen_CustomerAndDonglesMangment.Data;
using Alameen_CustomerAndDonglesMangment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Alameen_CustomerAndDonglesMangment.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public ServicesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Services
        public async Task<IActionResult> Index(int? StateId, int? IsPaid, string Word)
        {
            await End();
            User user = await _userManager.GetUserAsync(User);

            var applicationDbContext = _context.Tasks.Where(i => i.User.BranchId == user.BranchId)
                .Include(t => t.User).Include(t => t.Customer)
                .Include(t => t.Customer.State).Include(t => t.Customer.City);


            if (StateId.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(i => (int)i.State == StateId)
                    .Include(t => t.User).Include(t => t.Customer)
                    .Include(t => t.Customer.State).Include(t => t.Customer.City);
                ViewBag.StateId = StateId;
            }

            if (IsPaid.HasValue)
            {
                switch (IsPaid)
                {
                    case 1:
                        applicationDbContext = applicationDbContext.Where(i => i.IsPaid == true)
                            .Include(t => t.User).Include(t => t.Customer)
                            .Include(t => t.Customer.State).Include(t => t.Customer.City);
                        break;

                    case 2:
                        applicationDbContext = applicationDbContext.Where(i => i.IsPaid == false)
                            .Include(t => t.User).Include(t => t.Customer)
                            .Include(t => t.Customer.State).Include(t => t.Customer.City);
                        break;
                }

                ViewBag.IsPaid = IsPaid;

            }

            if (!String.IsNullOrEmpty(Word))
            {
                applicationDbContext = applicationDbContext.Where(i => i.NameOfCustomer.Contains(Word) || i.Customer.Name.Contains(Word))
                    .Include(t => t.User).Include(t => t.Customer)
                    .Include(t => t.Customer.State).Include(t => t.Customer.City);
                ViewBag.Word = Word;
            }

            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            await End();

            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.GetUserAsync(User);

            var tasks = await _context.Tasks
                .Include(t => t.User)
                .Include(t => t.Customer)
                .Include(t => t.MaintenanceType)
                .FirstOrDefaultAsync(m => m.Id == id && m.User.BranchId == user.BranchId);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }


        // GET: Services/Create
        public async Task<IActionResult> Create()
        {
            await End();

            ViewData["CompetentId"] = new SelectList(_userManager.GetUsersInRoleAsync("TechnicalSupport").Result, "Id", "FullName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["MaintenanceTypeId"] = new SelectList(_context.MaintenanceTypes, "Id", "Name");

            return View();
        }


        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tasks tasks)
        {
            await End();

            if (ModelState.IsValid)
            {
                _context.Add(tasks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompetentId"] = new SelectList(_userManager.GetUsersInRoleAsync("TechnicalSupport").Result, "Id", "FullName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", tasks.CustomerId);
            ViewData["MaintenanceTypeId"] = new SelectList(_context.MaintenanceTypes, "Id", "Name", tasks.MaintenanceTypeId);

            return View(tasks);
        }


        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            await End();

            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.GetUserAsync(User);

            var tasks = await _context.Tasks
                .Include(t => t.User)
                .Include(t => t.Customer)
                .Include(t => t.MaintenanceType)
                .FirstOrDefaultAsync(m => m.Id == id && m.User.BranchId == user.BranchId);

            if (tasks == null)
            {
                return NotFound();
            }
            ViewData["CompetentId"] = new SelectList(_userManager.GetUsersInRoleAsync("TechnicalSupport").Result, "Id", "FullName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", tasks.CustomerId);
            ViewData["MaintenanceTypeId"] = new SelectList(_context.MaintenanceTypes, "Id", "Name", tasks.MaintenanceTypeId);

            return View(tasks);
        }


        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tasks tasks)
        {
            await End();

            if (id != tasks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tasks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasksExists(tasks.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompetentId"] = new SelectList(_userManager.GetUsersInRoleAsync("TechnicalSupport").Result, "Id", "FullName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", tasks.CustomerId);
            ViewData["MaintenanceTypeId"] = new SelectList(_context.MaintenanceTypes, "Id", "Name", tasks.MaintenanceTypeId);

            return View(tasks);
        }


        // GET: Services/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            await End();

            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.GetUserAsync(User);

            var tasks = await _context.Tasks
                .Include(t => t.User)
                .Include(t => t.Customer)
                .Include(t => t.MaintenanceType)
                .FirstOrDefaultAsync(m => m.Id == id && m.User.BranchId == user.BranchId);

            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }


        // POST: Services/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await End();

            var tasks = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(tasks);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool TasksExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
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
