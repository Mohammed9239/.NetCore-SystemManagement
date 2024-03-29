﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Alameen_CustomerAndDonglesMangment.Data;
using Alameen_CustomerAndDonglesMangment.Models;

namespace Alameen_CustomerAndDonglesMangment.Controllers
{
    public class MaintenanceTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MaintenanceTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MaintenanceTypes.ToListAsync());
        }

        // GET: MaintenanceTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceType = await _context.MaintenanceTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (maintenanceType == null)
            {
                return NotFound();
            }

            return View(maintenanceType);
        }

        // GET: MaintenanceTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MaintenanceTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] MaintenanceType maintenanceType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maintenanceType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(maintenanceType);
        }

        // GET: MaintenanceTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceType = await _context.MaintenanceTypes.FindAsync(id);
            if (maintenanceType == null)
            {
                return NotFound();
            }
            return View(maintenanceType);
        }

        // POST: MaintenanceTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] MaintenanceType maintenanceType)
        {
            if (id != maintenanceType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maintenanceType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceTypeExists(maintenanceType.Id))
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
            return View(maintenanceType);
        }

        // GET: MaintenanceTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceType = await _context.MaintenanceTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (maintenanceType == null)
            {
                return NotFound();
            }

            return View(maintenanceType);
        }

        // POST: MaintenanceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenanceType = await _context.MaintenanceTypes.FindAsync(id);
            _context.MaintenanceTypes.Remove(maintenanceType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceTypeExists(int id)
        {
            return _context.MaintenanceTypes.Any(e => e.Id == id);
        }
    }
}
