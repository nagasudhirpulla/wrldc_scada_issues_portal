using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
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
    [Authorize]
    public class ReportingCasesController : Controller
    {
        public const string ControllerPath = "ReportingCases";
        private readonly ILogger<ReportingCasesController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        public ReportingCasesController(ILogger<ReportingCasesController> logger, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _context = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            // check if user is admin
            bool isAdmin = User.IsInRole(SecurityConstants.AdminRoleString);

            // get user id
            string userId = _userManager.GetUserId(User);

            // show only issues which the logged in user is concerned with
            var vm = await _context.ReportingCases
                            .Include(rc => rc.CaseItems)
                            .Include(rc => rc.ConcernedAgencies)
                            .ThenInclude(ca => ca.User)
                            .Where(rc => isAdmin || rc.ConcernedAgencies.Any(ca => ca.UserId == userId))
                            .OrderByDescending(rc => rc.CreatedAt).ToListAsync();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            ReportingCaseCreateVM vm = new ReportingCaseCreateVM
            {
                DownTime = DateTime.Now,
                CaseItems = await _context.CaseItemTemplates.Include(cit => cit.Options).OrderBy(ci => ci.SerialNum).ToListAsync()
            };
            vm.Responses = Enumerable.Range(1, vm.CaseItems.Count).Select(i => "").ToList();
            vm.ChoiceTexts = Enumerable.Range(1, vm.CaseItems.Count).Select(i => "").ToList();
            var userList = await _context.Users.ToListAsync();
            ViewData["userId"] = new SelectList(userList, nameof(IdentityUser.Id), nameof(IdentityUser.UserName));
            return View(vm);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ReportingCaseCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                // create a new case object
                ReportingCase newCase = new ReportingCase()
                {
                    DownTime = vm.DownTime
                };

                // add case items from vm
                List<ReportingCaseItem> caseItems = new List<ReportingCaseItem>();
                for (int caseTemplIter = 0; caseTemplIter < vm.CaseItems.Count; caseTemplIter++)
                {
                    CaseItemTemplate caseItemTemplate = vm.CaseItems[caseTemplIter];
                    string resp = vm.Responses[caseTemplIter];
                    if (caseItemTemplate.ResponseType == ResponseType.ChoicesWithText)
                    {
                        resp = vm.ChoiceTexts[caseTemplIter] ?? resp;
                    }
                    ReportingCaseItem caseItem = new ReportingCaseItem()
                    {
                        SerialNum = caseItemTemplate.SerialNum,
                        Question = caseItemTemplate.Question,
                        ResponseType = caseItemTemplate.ResponseType,
                        Response = resp
                    };
                    caseItems.Add(caseItem);
                }
                newCase.CaseItems = caseItems;

                _context.Add(newCase);
                int numInserted = await _context.SaveChangesAsync();
                if (numInserted >= 1)
                {
                    _logger.LogInformation("New Case created");
                    IdentityUser user = await _context.Users.FindAsync(vm.ConcernedAgencyId);
                    ReportingCaseConcerned concerned = new ReportingCaseConcerned()
                    {
                        ReportingCaseId = newCase.Id,
                        UserId = user.Id
                    };
                    _context.Add(concerned);
                    numInserted = await _context.SaveChangesAsync();
                    if (numInserted == 1)
                    {
                        _logger.LogInformation("New Case Concerned agency created");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        string msg = $"New Case Concerned agency not created as db returned num insertions as {numInserted}";
                        _logger.LogInformation(msg);
                        //todo create custom exception
                        throw new Exception(msg);
                    }
                }
                else
                {
                    string msg = $"New Case not created as db returned num insertions as {numInserted}";
                    _logger.LogInformation(msg);
                    //todo create custom exception
                    throw new Exception(msg);
                }
            }
            return View(vm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            // get the reporting case
            ReportingCase repCase = await _context.ReportingCases.Include(rc => rc.ConcernedAgencies)
                                            .ThenInclude(ca => ca.User)
                                            .Include(rc => rc.CaseItems)
                                            .SingleOrDefaultAsync(rc => rc.Id == id);
            ReportingCaseEditVM vm = new ReportingCaseEditVM
            {
                DownTime = repCase.DownTime,
                CaseItems = repCase.CaseItems,
                ResolutionTime = repCase.ResolutionTime,
                ResolutionRemarks = repCase.ResolutionRemarks,
                AdminRemarks = repCase.AdminRemarks,
                ConcernedAgencies = repCase.ConcernedAgencies
            };
            if (repCase.ConcernedAgencies.Count > 0)
            {
                vm.ConcernedAgencyId = repCase.ConcernedAgencies[0].UserId;
            }
            var userList = await _context.Users.ToListAsync();
            ViewData["userId"] = new SelectList(userList, nameof(IdentityUser.Id), nameof(IdentityUser.UserName));
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReportingCaseEditVM vm)
        {
            if (ModelState.IsValid)
            {
                // get the reporting case
                ReportingCase repCase = await _context.ReportingCases.Include(rc => rc.ConcernedAgencies)
                                                .SingleOrDefaultAsync(rc => rc.Id == id);
                if (repCase == null)
                {
                    return NotFound();
                }
                // update the reporting case
                repCase.AdminRemarks = vm.AdminRemarks;
                repCase.ResolutionRemarks = vm.ResolutionRemarks;
                repCase.ResolutionTime = vm.ResolutionTime;
                repCase.DownTime = vm.DownTime;
                repCase.CaseItems = vm.CaseItems;
                try
                {
                    _context.Update(repCase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // check if the we are trying to edit was already deleted due to concurrency
                    if (!_context.ReportingCases.Any(ci => ci.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // get the concerned agency Ids
                List<string> concernedUserIds = await _context.ReportingCaseConcerneds
                                                    .Where(rcc => rcc.ReportingCaseId == id)
                                                    .Select(rcc => rcc.UserId).ToListAsync();
                if (!concernedUserIds.Any(usr => usr == vm.ConcernedAgencyId))
                {
                    // _context.ReportingCaseConcerneds.Remove(conc);
                    // await _context.SaveChangesAsync();
                    ReportingCaseConcerned concerned = new ReportingCaseConcerned()
                    {
                        ReportingCaseId = repCase.Id,
                        UserId = vm.ConcernedAgencyId
                    };
                    _context.Add(concerned);
                    int numInserted = await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            var userList = await _context.Users.ToListAsync();
            ViewData["userId"] = new SelectList(userList, nameof(IdentityUser.Id), nameof(IdentityUser.UserName));
            return View(vm);
        }

        [Authorize(Roles = SecurityConstants.AdminRoleString)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var repCase = await _context.ReportingCases.FindAsync(id);
            if (repCase == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SecurityConstants.AdminRoleString)]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var repCase = await _context.ReportingCases.FindAsync(id);
            if (repCase == null)
            {
                return NotFound();
            }
            _context.ReportingCases.Remove(repCase);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
