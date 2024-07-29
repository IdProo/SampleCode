using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SampleCore;
using SampleCore.Data;

namespace SampleCore.Controllers
{
    public class UsersController : Controller
    {
        public Context _context = new Context();

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersTable = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersTable == null)
            {
                return NotFound();
            }

            return View(usersTable);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,City,DisplayName,EmployeeId,FirstName,Username,MobilePhone,LastName")] UsersTable usersTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usersTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usersTable);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersTable = await _context.Users.FindAsync(id);
            if (usersTable == null)
            {
                return NotFound();
            }
            return View(usersTable);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,City,DisplayName,EmployeeId,FirstName,Username,MobilePhone,LastName")] UsersTable usersTable)
        {
            if (id != usersTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersTableExists(usersTable.Id))
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
            return View(usersTable);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersTable = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersTable == null)
            {
                return NotFound();
            }

            return View(usersTable);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var usersTable = await _context.Users.FindAsync(id);
            _context.Users.Remove(usersTable);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersTableExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
