using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using ScadaIssuesPortal.Core;
using ScadaIssuesPortal.Core.Entities;
using ScadaIssuesPortal.Data;
using ScadaIssuesPortal.Web.Models;

namespace ScadaIssuesPortal.Web.Controllers
{
    [Authorize(Roles = SecurityConstants.AdminRoleString)]
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        public OptionsAPIController(ILogger<OptionsAPIController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        //[HttpPost]
        //public async Task<IActionResult> Add([FromBody] CaseItemTemplateOptionAddVM formModel)
        //{
            
        //}
    }
}