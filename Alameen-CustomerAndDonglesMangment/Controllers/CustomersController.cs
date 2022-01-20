using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Alameen_CustomerAndDonglesMangment.Data;
using Alameen_CustomerAndDonglesMangment.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Alameen_CustomerAndDonglesMangment.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CustomersController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Customers
        public async Task<IActionResult> Index(int? StateId, int? ActivityTypeId, int? CityId, string Word)
        {
            await End();

            User user = await _userManager.GetUserAsync(User);
            int? BranchId = user.BranchId;

            var applicationDbContext = _context.Customers.Where(i=>i.BranchId==BranchId)
                .Include(c => c.ActivityType)
                .Include(c => c.State)
                .Include(c => c.City)
                .Include(c => c.ActivityType);

            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", StateId);
            ViewData["ActivityTypeId"] = new SelectList(_context.ActivityTypes, "Id", "Name", ActivityTypeId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", CityId);
            ViewBag.Word = Word;

            if (StateId.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(i=>i.StateId==StateId)
                    .Include(c => c.ActivityType)
                    .Include(c => c.State)
                    .Include(c => c.City)
                    .Include(c => c.ActivityType);
            }

            if (ActivityTypeId.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(i => i.ActivityTypeId == ActivityTypeId)
                    .Include(c => c.ActivityType)
                    .Include(c => c.State)
                    .Include(c => c.City)
                    .Include(c => c.ActivityType);
            }

            if (CityId.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(i => i.CityId == CityId)
                    .Include(c => c.ActivityType)
                    .Include(c => c.State)
                    .Include(c => c.City)
                    .Include(c => c.ActivityType);
            }

            if (!string.IsNullOrEmpty(Word))
            {
                applicationDbContext = applicationDbContext.Where(i => i.Name.Contains(Word) || i.State.Name.Contains(Word)
                || i.City.Name.Contains(Word) || i.Address.Contains(Word))
                    .Include(c => c.ActivityType)
                    .Include(c => c.State)
                    .Include(c => c.City)
                    .Include(c => c.ActivityType);
            }
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            await End();

            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.GetUserAsync(User);
            int? BranchId = user.BranchId;

            var customers = await _context.Customers.Where(i=>i.BranchId == BranchId)
                .Include(c => c.ActivityType)
                .Include(c => c.Branch)
                .Include(c => c.City)
                .Include(c => c.ContractType)
                .Include(c => c.CustomerStatus)
                .Include(c => c.Sqlver)
                .Include(c => c.State)
                .Include(c => c.WindowsVer)
                .Include(c => c.CustBrans)
                .FirstOrDefaultAsync(m => m.Id == id);

            customers.Dongles = await _context.Dongles
                .Include(i=>i.DongleType)
                .Include(i => i.DongleColor)

                .Where(i => i.CustomerId == customers.Id).ToListAsync();

            customers.Representatives = await _context.Representatives
                .Include(c => c.City)
                .Include(c => c.State)
                .Where(i => i.CustomersId == customers.Id).ToListAsync();


            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        // GET: Customers/Create
        public async Task<IActionResult> Create()
        {
            await End();

            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name");
            ViewData["ActivityTypeId"] = new SelectList(_context.ActivityTypes, "Id", "Name");
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "Id", "Name");
            ViewData["CustomerStatusId"] = new SelectList(_context.CustomerStatuses, "Id", "Name");
            ViewData["SqlverId"] = new SelectList(_context.Sqlvers, "Id", "Name");
            ViewData["WindowsVerId"] = new SelectList(_context.WindowsVers, "Id", "Name");


            return View();
        }


        // POST: Customers/Create
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customers customers , IFormFile logo)
        {
            await End();

            User user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                customers.Logo = Img(logo);
                customers.BranchId = user.BranchId;
                _context.Add(customers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityTypeId"] = new SelectList(_context.ActivityTypes, "Id", "Name", customers.ActivityTypeId);
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "Id", "Name", customers.ContractTypeId);
            ViewData["CustomerStatusId"] = new SelectList(_context.CustomerStatuses, "Id", "Name", customers.CustomerStatusId);
            ViewData["SqlverId"] = new SelectList(_context.Sqlvers, "Id", "Name", customers.SqlverId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", customers.StateId);
            ViewData["WindowsVerId"] = new SelectList(_context.WindowsVers, "Id", "Name", customers.WindowsVerId);
            return View(customers);
        }


        // GET: Customers/Edit/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
           await End();

            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.GetUserAsync(User);

            var customers = await _context.Customers.FirstOrDefaultAsync(i=>i.BranchId==user.BranchId && i.Id==id);
            if (customers == null)
            {
                return NotFound();
            }
            ViewData["ActivityTypeId"] = new SelectList(_context.ActivityTypes, "Id", "Name", customers.ActivityTypeId);
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", customers.BranchId);
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "Id", "Name", customers.ContractTypeId);
            ViewData["CustomerStatusId"] = new SelectList(_context.CustomerStatuses, "Id", "Name", customers.CustomerStatusId);
            ViewData["SqlverId"] = new SelectList(_context.Sqlvers, "Id", "Name", customers.SqlverId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", customers.StateId);
            ViewData["WindowsVerId"] = new SelectList(_context.WindowsVers, "Id", "Name", customers.WindowsVerId);
            return View(customers);
        }


        // POST: Customers/Edit/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customers customers)
        {
            await End();
            User user = await _userManager.GetUserAsync(User);

            if (id != customers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customers.BranchId = user.BranchId;
                    _context.Update(customers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomersExists(customers.Id))
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
            ViewData["ActivityTypeId"] = new SelectList(_context.ActivityTypes, "Id", "Name", customers.ActivityTypeId);
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", customers.BranchId);
            ViewData["ContractTypeId"] = new SelectList(_context.ContractTypes, "Id", "Name", customers.ContractTypeId);
            ViewData["CustomerStatusId"] = new SelectList(_context.CustomerStatuses, "Id", "Id", customers.CustomerStatusId);
            ViewData["SqlverId"] = new SelectList(_context.Sqlvers, "Id", "Name", customers.SqlverId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name", customers.StateId);
            ViewData["WindowsVerId"] = new SelectList(_context.WindowsVers, "Id", "Name", customers.WindowsVerId);
            return View(customers);
        }


        // GET: Customers/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            await End();

            if (id == null)
            {
                return NotFound();
            }

            User user = await _userManager.GetUserAsync(User);

            var customers = await _context.Customers
                .Include(c => c.ActivityType)
                .Include(c => c.Branch)
                .Include(c => c.City)
                .Include(c => c.ContractType)
                .Include(c => c.CustomerStatus)
                .Include(c => c.Sqlver)
                .Include(c => c.State)
                .Include(c => c.WindowsVer)
                .FirstOrDefaultAsync(m => m.Id == id && m.BranchId == user.BranchId);

            if (customers == null)
            {
                return NotFound();
            }

            return View(customers);
        }

        // POST: Customers/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await End();

            var customers = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> AddDongle(int id)
        {
            await End();

            var customers = await _context.Customers.FindAsync(id);
            ViewData["Dongles"] = new SelectList(_context.Dongles, "Id", "SerialNum");
            return View(customers);
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("AddDongle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDonglePost(int id, int DongleId)
        {
            await End();

            var customers = await _context.Customers.FindAsync(id);
            ViewData["Dongles"] = new SelectList(_context.Dongles, "Id", "SerialNum");

            if (customers==null)
            {
                return NotFound();
            }

            var Dongle = await _context.Dongles.FindAsync(DongleId);

            if (Dongle == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Dongle.CustomerId = customers.Id;

                _context.Dongles.Update(Dongle);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details),new { @id=id });

            }

            return View(customers);
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> AddBranch(int id)
        {
            await End();

            var customer = await _context.Customers.FindAsync(id);
            if (customer==null)
            {
                return NotFound();
            }

            ViewData["customer"] = customer.Name;
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name");

            return View();
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("AddBranch")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBranchPost(int id, CustBrans custBrans)
        {
            await End();

            var custBran = new CustBrans
            {
                CustomerId = id,
                Name = custBrans.Name,
                StateId = custBrans.StateId,
                CityId = custBrans.CityId,
                Address = custBrans.Address,
                Phone = custBrans.Phone
            };

            _context.CustBrans.Add(custBran);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details),new { @id=id });
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> EditBranch(int id)
        {
            await End();

            User user = await _userManager.GetUserAsync(User);

            var CustBran = await _context.CustBrans.Include(i=>i.Customer).FirstOrDefaultAsync(i => i.Id == id && i.Customer.BranchId == user.BranchId);
            if (CustBran == null)
            {
                return NotFound();
            }

            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name");

            return View(CustBran);
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("EditBranch")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBranchPost(int id, CustBrans custBrans)
        {
            End();

            _context.CustBrans.Update(custBrans);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { @id = custBrans.CustomerId });
        }



        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> AddVicar(int id)
        {
            await End();

            User user = await _userManager.GetUserAsync(User);

            var customer = await _context.Customers.FirstOrDefaultAsync(i=>i.Id == id && i.BranchId == user.BranchId);
            if (customer == null)
            {
                return NotFound();
            }

            ViewData["customer"] = customer.Name;
            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name");

            return View();
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("AddVicar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVicarPost(int id, Representatives representatives)
        {
            await End();

            var Vicar = new Representatives
            {
                CustomersId = id,
                Name = representatives.Name,
                StateId = representatives.StateId,
                CityId = representatives.CityId,
                Address = representatives.Address,
                Phone = representatives.Phone
            };

            _context.Representatives.Add(Vicar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { @id = id });
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> EditVicar(int id)
        {
            await End();

            User user = await _userManager.GetUserAsync(User);

            var Vicar = await _context.Representatives.Include(i => i.Customers).FirstOrDefaultAsync(i => i.Id == id && user.BranchId == user.BranchId);
            if (Vicar == null)
            {
                return NotFound();
            }

            ViewData["StateId"] = new SelectList(_context.States, "Id", "Name");

            return View(Vicar);
        }


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost, ActionName("EditVicar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVicarPost(int id, Representatives representatives)
        {
            End();

            _context.Representatives.Update(representatives);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { @id = representatives.CustomersId });
        }



        private bool CustomersExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }




        // To get Sub City
        [HttpGet]
        public async Task<IActionResult> getsub(int? id)
        {
            var ctor = await _context.Cities.Where(i => i.StateId == id).ToListAsync();
            return Json(ctor);
        }


        // Add Img
        public string Img(IFormFile fav)
        {
            string filename = "";
            if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
            {
                if (Request.Form.Files.Count > 0 && Request.Form.Files[0].Length > 0)
                {
                    var file = fav;
                    string[] str = file.FileName.Split('.');
                    string ext = str[str.Length - 1];
                    filename = string.Format("{0}.{1}",  Guid.NewGuid().ToString(), ext);
                    string path = string.Format("{0}/wwwroot/Uploads/{1}",
                        Directory.GetCurrentDirectory(), filename);
                    using (var streem = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(streem);
                    }
                }
            }
            return filename;
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
