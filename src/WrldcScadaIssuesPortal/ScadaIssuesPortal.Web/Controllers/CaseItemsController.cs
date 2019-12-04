using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScadaIssuesPortal.Core.Entities;

namespace ScadaIssuesPortal.Web.Controllers
{
    public class CaseItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            CaseItemTemplate vm = new CaseItemTemplate();
            return View(vm);
        }
    }
}