using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScadaIssuesPortal.Core.Entities;
using ScadaIssuesPortal.Data;
using ScadaIssuesPortal.Web.Models;

namespace ScadaIssuesPortal.Web.Controllers
{
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
                CaseItemTemplates = await _context.CaseItemTemplates.Include(ci=>ci.Options).ToListAsync()
            };
            return View(vm);
        }

        public IActionResult Create()
        {
            CaseItemTemplate vm = new CaseItemTemplate();
            var resTypes = Enum.GetValues(typeof(ResponseType)).Cast<ResponseType>().Select(v=>v.ToString());
            ViewData["ResponseTypeId"] = new SelectList(resTypes, ResponseType.ShortText.ToString());
            return View(vm);
        }

        [HttpPost]
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
    }
}