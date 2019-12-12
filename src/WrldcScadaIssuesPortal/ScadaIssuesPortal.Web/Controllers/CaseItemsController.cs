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
using ScadaIssuesPortal.Web.Extensions;

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

        public async Task<IActionResult> Create()
        {
            CaseItemTemplate vm = new CaseItemTemplate();
            // get the largest serial number
            int serNum = 1;
            if (_context.CaseItemTemplates.Count() > 0)
            {
                serNum = await _context.CaseItemTemplates.MaxAsync(ci => ci.SerialNum) + 1;
            }
            vm.SerialNum = serNum;
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

                    return RedirectToAction(nameof(Index)).WithSuccess("New Case Item Template created");
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
            CaseItemTemplate templ = await _context.CaseItemTemplates.SingleOrDefaultAsync(ci => ci.Id == id);
            templ.Options = await _context.CaseItemOptions.Where(o => o.CaseItemTemplateId == id).OrderBy(o => o.SerialNum).ToListAsync();
            if (templ == null)
            {
                return NotFound();
            }

            CaseItemTemplateEditVM vm = new CaseItemTemplateEditVM()
            {
                ItemTemplate = templ
            };

            var resTypes = Enum.GetValues(typeof(ResponseType)).Cast<ResponseType>().Select(v => v.ToString());
            ViewData["ResponseTypeId"] = new SelectList(resTypes, templ.ResponseType.ToString());
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CaseItemTemplateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                CaseItemTemplate templ = await _context.CaseItemTemplates.FindAsync(id);
                if (templ == null)
                {
                    return NotFound();
                }

                // update object as per user changes
                templ.Question = vm.ItemTemplate.Question;
                templ.ResponseType = vm.ItemTemplate.ResponseType;
                templ.SerialNum = vm.ItemTemplate.SerialNum;
                templ.IsResponseRequired = vm.ItemTemplate.IsResponseRequired;
                //templ.Options = vm.ItemTemplate.Options;
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

                // create new option if not null or blank
                string newOpt = (vm.NewOptionText ?? "").Trim();
                if (!string.IsNullOrEmpty(newOpt))
                {
                    // find the biggest serial number for this CaseItemTemplate - https://stackoverflow.com/questions/7542021/how-to-get-max-value-of-a-column-using-entity-framework
                    int maxSerNum = 1;
                    if (_context.CaseItemOptions.Count() > 0)
                    {
                        maxSerNum = _context.CaseItemOptions.Select(o => o.SerialNum).Max() + 1;
                    }
                    CaseItemOption opt = new CaseItemOption()
                    {
                        CaseItemTemplateId = id,
                        OptionText = newOpt,
                        SerialNum = maxSerNum
                    };

                    _context.Add(opt);
                    int numInserted = await _context.SaveChangesAsync();
                    if (numInserted == 1)
                    {
                        _logger.LogInformation("New Case Item Template Option created");
                        //return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        string msg = $"New Case Item Template Option not created as db returned num insertions as {numInserted}";
                        _logger.LogInformation(msg);
                        //todo create custom exception
                        throw new Exception(msg);
                    }
                }
                return RedirectToAction(nameof(Index)).WithSuccess("Case Item Editing done");
            }
            var resTypes = Enum.GetValues(typeof(ResponseType)).Cast<ResponseType>().Select(v => v.ToString());
            ViewData["ResponseTypeId"] = new SelectList(resTypes, vm.ItemTemplate.ResponseType.ToString());
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
            return RedirectToAction(nameof(Index)).WithSuccess("Case Item Deletion done");
        }
    }
}