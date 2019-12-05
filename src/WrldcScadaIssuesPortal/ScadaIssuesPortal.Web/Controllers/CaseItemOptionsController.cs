using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScadaIssuesPortal.Core;
using ScadaIssuesPortal.Core.Entities;
using ScadaIssuesPortal.Data;
using ScadaIssuesPortal.Web.Models;

namespace ScadaIssuesPortal.Web.Controllers
{
    [Authorize(Roles = SecurityConstants.AdminRoleString)]
    public class CaseItemOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public CaseItemOptionsController(ILogger<CaseItemOptionsController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CaseItemOption opt = await _context.CaseItemOptions.SingleOrDefaultAsync(ci => ci.Id == id);
            if (opt == null)
            {
                return NotFound();
            }
            return View(opt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CaseItemOption vm)
        {
            if (ModelState.IsValid)
            {
                CaseItemOption opt = await _context.CaseItemOptions.FindAsync(id);
                if (opt == null)
                {
                    return NotFound();
                }

                // update object as per user changes
                opt.SerialNum = vm.SerialNum;
                opt.OptionText = vm.OptionText;
                try
                {
                    _context.Update(opt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // check if the we are trying to edit was already deleted due to concurrency
                    if (!_context.CaseItemOptions.Any(cio => cio.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Edit), "CaseItems", new { id = opt.CaseItemTemplateId });
            }
            // If we got this far, something failed, redisplay form
            return View(vm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CaseItemOption opt = await _context.CaseItemOptions.SingleOrDefaultAsync(cio => cio.Id == id);
            if (opt == null)
            {
                return NotFound();
            }
            return View(opt);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CaseItemOption opt = await _context.CaseItemOptions.SingleOrDefaultAsync(cio => cio.Id == id);
            if (opt == null)
            {
                return NotFound();
            }
            _context.CaseItemOptions.Remove(opt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), "CaseItems", new { id = opt.CaseItemTemplateId });
        }
    }
}