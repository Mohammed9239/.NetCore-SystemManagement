using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Alameen_CustomerAndDonglesMangment.Data;
using Alameen_CustomerAndDonglesMangment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Alameen_CustomerAndDonglesMangment.Controllers
{
    public class DonglesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DonglesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Dongles
        public async Task<IActionResult> Index(string Word)
        {
            await End();

            User user = await _userManager.GetUserAsync(User);

            var applicationDbContext = _context.Dongles.Where(i=>i.BranchId == user.BranchId).Include(d => d.AmeenAddOnTypes).Include(d => d.AmeenVer)
                .Include(d => d.Branch).Include(d => d.CustBran)
                .Include(d => d.DongleColor).Include(d => d.DongleType)
                .Include(d => d.Representatives).Include(d => d.Customer).Include(i=>i.Customer.State).Include(i=>i.Customer.City);

            if (!String.IsNullOrEmpty(Word))
            {
                applicationDbContext = applicationDbContext.Where(i=>i.SerialNum.Contains(Word) || i.Customer.Name.Contains(Word) || i.Customer.State.Name.Contains(Word) || i.Customer.City.Name.Contains(Word) || i.Customer.Address.Contains(Word))
                    .Include(d => d.AmeenAddOnTypes).Include(d => d.AmeenVer)
                    .Include(d => d.Branch).Include(d => d.CustBran)
                    .Include(d => d.DongleColor).Include(d => d.DongleType)
                    .Include(d => d.Representatives).Include(d => d.Customer).Include(i => i.Customer.State).Include(i => i.Customer.City);
                ViewBag.Word = Word;
            }
            
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Dongles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            await End();

            User user = await _userManager.GetUserAsync(User);

            if (id == null)
            {
                return NotFound();
            }

            var dongles = await _context.Dongles
                .Include(d => d.AmeenAddOnTypes)
                .Include(d => d.AmeenVer)
                .Include(d => d.Branch)
                .Include(d => d.CustBran)
                .Include(d => d.Customer)
                .Include(d => d.Customer.State)
                .Include(d => d.Customer.City)
                .Include(d => d.DongleColor)
                .Include(d => d.DongleType)
                .Include(d => d.Representatives)
                .FirstOrDefaultAsync(m => m.Id == id && m.BranchId == user.BranchId);
            if (dongles == null)
            {
                return NotFound();
            }

            return View(dongles);
        }


        // GET: Dongles/Create
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Create()
        {
            await End();

            ViewBag.found = false;
            ViewData["AmeenAddOnTypesId"] = new SelectList(_context.AmeenAddOnTypes, "Id", "Name");
            ViewData["AmeenVerId"] = new SelectList(_context.AmeenVers, "Id", "Name");
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name");
            ViewData["CustBranId"] = new SelectList(_context.CustBrans, "Id", "Name");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["DongleColorId"] = new SelectList(_context.DongleColors, "Id", "Name");
            ViewData["DongleTypeId"] = new SelectList(_context.DongleTypes, "Id", "Name");
            ViewData["RepresentativesId"] = new SelectList(_context.Representatives, "Id", "Name");
            return View();
        }


        // POST: Dongles/Create
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Dongles dongles)
        {
            await End();
            ViewBag.found = false;
            var check = _context.Dongles.Where(i => i.SerialNum == dongles.SerialNum).ToList().Count;
            User user = await _userManager.GetUserAsync(User);

            if (check > 0)
            {
                ViewBag.found = true;
            }

            else
            {
                if (ModelState.IsValid)
                {
                    dongles.BranchId = user.BranchId;
                    _context.Add(dongles);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }

            ViewData["AmeenAddOnTypesId"] = new SelectList(_context.AmeenAddOnTypes, "Id", "Name", dongles.AmeenAddOnTypesId);
            ViewData["AmeenVerId"] = new SelectList(_context.AmeenVers, "Id", "Name", dongles.AmeenVerId);
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", dongles.BranchId);
            ViewData["CustBranId"] = new SelectList(_context.CustBrans, "Id", "Name", dongles.CustBranId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", dongles.CustomerId);
            ViewData["DongleColorId"] = new SelectList(_context.DongleColors, "Id", "Name", dongles.DongleColorId);
            ViewData["DongleTypeId"] = new SelectList(_context.DongleTypes, "Id", "Name", dongles.DongleTypeId);
            ViewData["RepresentativesId"] = new SelectList(_context.Representatives, "Id", "Name", dongles.RepresentativesId);
            return View(dongles);
        }


        // GET: Dongles/Edit/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            await End();
            User user = await _userManager.GetUserAsync(User);

            if (id == null)
            {
                return NotFound();
            }

            var dongles = await _context.Dongles.FirstOrDefaultAsync(i=>i.Id == id && i.BranchId == user.BranchId);
            if (dongles == null)
            {
                return NotFound();
            }
            ViewData["AmeenAddOnTypesId"] = new SelectList(_context.AmeenAddOnTypes, "Id", "Name", dongles.AmeenAddOnTypesId);
            ViewData["AmeenVerId"] = new SelectList(_context.AmeenVers, "Id", "Name", dongles.AmeenVerId);
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", dongles.BranchId);
            ViewData["CustBranId"] = new SelectList(_context.CustBrans, "Id", "Name", dongles.CustBranId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", dongles.CustomerId);
            ViewData["DongleColorId"] = new SelectList(_context.DongleColors, "Id", "Name", dongles.DongleColorId);
            ViewData["DongleTypeId"] = new SelectList(_context.DongleTypes, "Id", "Name", dongles.DongleTypeId);
            ViewData["RepresentativesId"] = new SelectList(_context.Representatives, "Id", "Name", dongles.RepresentativesId);
            return View(dongles);
        }


        // POST: Dongles/Edit/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Dongles dongles)
        {
            await End();
            User user = await _userManager.GetUserAsync(User);

            if (id != dongles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dongles.BranchId = user.BranchId;
                    _context.Update(dongles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonglesExists(dongles.Id))
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


            ViewData["AmeenAddOnTypesId"] = new SelectList(_context.AmeenAddOnTypes, "Id", "Name", dongles.AmeenAddOnTypesId);
            ViewData["AmeenVerId"] = new SelectList(_context.AmeenVers, "Id", "Name", dongles.AmeenVerId);
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", dongles.BranchId);
            ViewData["CustBranId"] = new SelectList(_context.CustBrans, "Id", "Name", dongles.CustBranId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", dongles.CustomerId);
            ViewData["DongleColorId"] = new SelectList(_context.DongleColors, "Id", "Name", dongles.DongleColorId);
            ViewData["DongleTypeId"] = new SelectList(_context.DongleTypes, "Id", "Name", dongles.DongleTypeId);
            ViewData["RepresentativesId"] = new SelectList(_context.Representatives, "Id", "Name", dongles.RepresentativesId);
            return View(dongles);
        }


        // GET: Dongles/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            await End();
            User user = await _userManager.GetUserAsync(User);

            if (id == null)
            {
                return NotFound();
            }

            var dongles = await _context.Dongles
                .Include(d => d.AmeenAddOnTypes)
                .Include(d => d.AmeenVer)
                .Include(d => d.Branch)
                .Include(d => d.CustBran)
                .Include(d => d.Customer)
                .Include(d => d.DongleColor)
                .Include(d => d.DongleType)
                .Include(d => d.Representatives)
                .FirstOrDefaultAsync(m => m.Id == id && m.BranchId == user.BranchId);
            if (dongles == null)
            {
                return NotFound();
            }

            return View(dongles);
        }


        // POST: Dongles/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dongles = await _context.Dongles.FindAsync(id);
            _context.Dongles.Remove(dongles);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool DonglesExists(int id)
        {
            return _context.Dongles.Any(e => e.Id == id);
        }


        // To get Sub Vicar
        [HttpGet]
        public async Task<IActionResult> getVicar(int? id)
        {
            var Vicar = await _context.Representatives.Where(i => i.CustomersId == id).ToListAsync();
            
            return Json(Vicar);
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
