using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class UserTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserTasksController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserTask.ToListAsync());
        }
        
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        public async Task<IActionResult> ShowSearchResults(string searchPhrase)
        {
            return View("Index", await _context.UserTask.Where(userTask => userTask.TaskName.Contains(searchPhrase)).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            
            var userTask = await _context.UserTask.FirstOrDefaultAsync(m => m.Id == id);

            if (userTask == null) return NotFound();
            
            return View(userTask);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TaskName,TaskDescription")] UserTask userTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userTask);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();            

            var userTask = await _context.UserTask.FindAsync(id);

            if (userTask == null) return NotFound();
            
            return View(userTask);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TaskName,TaskDescription")] UserTask userTask)
        {
            if (id != userTask.Id) return NotFound();            

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {                    
                    if (!TaskExists(userTask.Id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(userTask);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();            

            var userTask = await _context.UserTask.FirstOrDefaultAsync(m => m.Id == id);

            if (userTask == null) return NotFound();            

            return View(userTask);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.UserTask.FindAsync(id);

            if (task != null) _context.UserTask.Remove(task);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int providedId)
        {
            return _context.UserTask.Any(task => task.Id == providedId);
        }
    }
}
