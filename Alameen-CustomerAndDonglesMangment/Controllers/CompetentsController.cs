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
using Alameen_CustomerAndDonglesMangment.VM;
using Microsoft.AspNetCore.Authorization;

namespace Alameen_CustomerAndDonglesMangment.Controllers
{
    public class CompetentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;


        public CompetentsController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Competents
        public async Task<IActionResult> Index(string Word)
        {
            await End();

            //var applicationDbContext = await _context.Competents.ToListAsync();

            User user = await _userManager.GetUserAsync(User);

            var Users = _userManager.GetUsersInRoleAsync("TechnicalSupport").Result.Select(b => new User
            {
                Id = b.Id,
                FullName = b.FullName,
                Jop = b.Jop,
                PhoneNumber = b.PhoneNumber,
                BranchId = b.BranchId
            }).Where(i=>i.BranchId == user.BranchId);

            if (!String.IsNullOrEmpty(Word))
            {
                Users = Users.Where(i => i.FullName.Contains(Word) || i.Jop.Contains(Word));
                ViewBag.Word = Word;
            }
            return View(Users);
        }


        // GET: Competents/Details/5
        public async Task<IActionResult> Details(string id)
        {
            await End();

            if (id == null)
            {
                return NotFound();
            }

            //var competent = await _context.Competents
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (competent == null)
            //{
            //    return NotFound();
            //}

            var User = await _userManager.FindByIdAsync(id);

            return View(User);
        }



        // GET: Competents/Create
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Create()
        {
            await End();

            return View();
        }


        // POST: Competents/Create
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            await End();

            User currentuser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                //check is email and username with other
                var Users = await _userManager.Users.ToListAsync();

                if (Users.Any(i => i.UserName == user.UserName))
                {
                    ModelState.AddModelError("", "اسم المستخدم مسجل مسبقا");
                    return View();
                }

                user.BranchId = currentuser.BranchId;
                var result = await _userManager.CreateAsync(user, user.PasswordHash);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("Users", "خطاء غير متوقع");
                }

                var AddToRole_result = await _userManager.AddToRoleAsync(user, "TechnicalSupport");
                if (!AddToRole_result.Succeeded)
                {
                    ModelState.AddModelError("", "حصل خطاء غير متوقع اثناء اضافة الصلاحية للمستخدم");
                    return View();
                }

                //_context.Add(competent);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }



        // GET: Competents/Edit/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            await End();

            if (id == null)
            {
                return NotFound();
            }

            //var competent = await _context.Competents.FindAsync(id);

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // POST: Competents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Edit(string id, User model)
        {
            await End();

            var user = await _userManager.FindByIdAsync(id);

            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //check is email and username with other
                var Users = await _userManager.Users.ToListAsync();

                if (Users.Any(i => i.UserName == model.UserName && i.Id != user.Id))
                {
                    ModelState.AddModelError("", "اسم المستخدم مسجل مسبقا");
                    return View();
                }

                //update
                user.FullName = model.FullName;
                user.Jop = model.Jop;
                user.PhoneNumber = model.PhoneNumber;

                //update user
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                    ModelState.AddModelError("", "خطاء غير متوقع");
            }

            return View(user);
        }


        // GET: Competents/Delete/5
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Delete(string? id)
        {
            await End();

            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // POST: Competents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await End();

            var user = await _userManager.FindByIdAsync(id);
            IdentityResult result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            return View(user);
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
