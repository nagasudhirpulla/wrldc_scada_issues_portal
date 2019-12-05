﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScadaIssuesPortal.Core.Entities;
using ScadaIssuesPortal.Data;
using ScadaIssuesPortal.Web.Models;

namespace ScadaIssuesPortal.Web.Controllers
{
    public class ReportingCasesController : Controller
    {
        private readonly ILogger<ReportingCasesController> _logger;
        private readonly ApplicationDbContext _context;
        public ReportingCasesController(ILogger<ReportingCasesController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public IActionResult Index()
        {
            return View();
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
                        Response = vm.Responses[caseTemplIter]
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

    }
}