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
    public class CaseItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public CaseItemsController(ILogger<CaseItemsController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            CaseItemTemplateListVM vm = new CaseItemTemplateListVM
            {
                CaseItemTemplates = await _context.CaseItemTemplates.Include(ci => ci.Options).ToListAsync()
            };
            return View(vm);
        }

        public IActionResult Create()
        {
            CaseItemTemplate vm = new CaseItemTemplate();
            var resTypes = Enum.GetValues(typeof(ResponseType)).Cast<ResponseType>().Select(v => v.ToString());
            ViewData["ResponseTypeId"] = new SelectList(resTypes, ResponseType.ShortText.ToString());
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaseItemTemplate vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm);
                int numInserted = await _context.SaveChangesAsync();

                if (numInserted == 1)
                {
                    _logger.LogInformation("New Case Item Template created");

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    string msg = $"New Case Item Template not created as db returned num insertions as {numInserted}";
                    _logger.LogInformation(msg);
                    //todo create custom exception
                    throw new Exception(msg);
                }
            }
            // If we got this far, something failed, redisplay form
            return View(vm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CaseItemTemplate templ = await _context.CaseItemTemplates.Include(ci => ci.Options).SingleOrDefaultAsync(ci => ci.Id == id);
            if (templ == null)
            {
                return NotFound();
            }
            var resTypes = Enum.GetValues(typeof(ResponseType)).Cast<ResponseType>().Select(v => v.ToString());
            ViewData["ResponseTypeId"] = new SelectList(resTypes, templ.ResponseType.ToString());
            return View(templ);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CaseItemTemplate vm)
        {
            if (ModelState.IsValid)
            {
                CaseItemTemplate templ = await _context.CaseItemTemplates.FindAsync(id);
                if (templ == null)
                {
                    return NotFound();
                }

                // update object as per user changes
                templ.Question = vm.Question;
                templ.ResponseType = vm.ResponseType;
                templ.SerialNum = vm.SerialNum;
                templ.IsResponseRequired = vm.IsResponseRequired;
                templ.Options = vm.Options;
                try
                {
                    _context.Update(templ);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // check if the we are trying to edit was already deleted due to concurrency
                    if (!_context.CaseItemTemplates.Any(ci => ci.Id == id))
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
            var resTypes = Enum.GetValues(typeof(ResponseType)).Cast<ResponseType>().Select(v => v.ToString());
            ViewData["ResponseTypeId"] = new SelectList(resTypes, vm.ResponseType.ToString());
            // If we got this far, something failed, redisplay form
            return View(vm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CaseItemTemplate templ = await _context.CaseItemTemplates.SingleOrDefaultAsync(ci => ci.Id == id);
            if (templ == null)
            {
                return NotFound();
            }
            return View(templ);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CaseItemTemplate templ = await _context.CaseItemTemplates.SingleOrDefaultAsync(ci => ci.Id == id);
            if (templ == null)
            {
                return NotFound();
            }
            _context.CaseItemTemplates.Remove(templ);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }        
    }
}