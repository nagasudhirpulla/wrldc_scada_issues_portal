using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScadaIssuesPortal.Core.Entities;
using ScadaIssuesPortal.Data;
using ScadaIssuesPortal.Web.Extensions;

namespace ScadaIssuesPortal.Web.Controllers
{
    [Authorize]
    public class ConcernedAgenciesController : Controller
    {
        private readonly ILogger<ReportingCasesController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        public ConcernedAgenciesController(ILogger<ReportingCasesController> logger, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _context = dbContext;
        }

        public async Task<IActionResult> Delete(int? caseId, string userId)
        {
            if (caseId == null || userId == null)
            {
                return NotFound();
            }
            // get the concerned agency entity
            var concernedAgency = await _context.ReportingCaseConcerneds.SingleOrDefaultAsync(ca => ca.UserId == userId && ca.ReportingCaseId == caseId);
            if (concernedAgency == null)
            {
                return NotFound();
            }
            _context.ReportingCaseConcerneds.Remove(concernedAgency);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ReportingCasesController.Edit), ReportingCasesController.ControllerPath, new { id = caseId }).WithSuccess("Concerned Agency Deleted");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(int? caseId, string userId)
        //{
        //    if (caseId == null || userId == null)
        //    {
        //        return NotFound();
        //    }
        //    // create concerned agency
        //    var ca = new ReportingCaseConcerned()
        //    {
        //        UserId = userId,
        //        ReportingCaseId = caseId.Value
        //    };
        //    _context.Add(ca);
        //    int numIns = await _context.SaveChangesAsync();
        //    if (numIns == 1)
        //    {
        //        _logger.LogInformation("New Case Concerned agency created");
        //        return RedirectToAction(nameof(ReportingCasesController.Edit), ReportingCasesController.ControllerPath, new { id = caseId });
        //    }
        //    else
        //    {
        //        string msg = $"New Case Concerned agency not created as db returned num insertions as {numIns}";
        //        _logger.LogInformation(msg);
        //        //todo create custom exception
        //        throw new Exception(msg);
        //    }
        //}
    }
}