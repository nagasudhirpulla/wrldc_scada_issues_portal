using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScadaIssuesPortal.Core;
using ScadaIssuesPortal.Core.Entities;
using ScadaIssuesPortal.Data;
using ScadaIssuesPortal.Web.Models;
using MoreLinq.Extensions;

namespace ScadaIssuesPortal.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CaseEditUIController : ControllerBase
    {
        private readonly ILogger<ReportingCasesController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CaseEditUIController(ILogger<ReportingCasesController> logger, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _context = dbContext;
        }

        [HttpGet("commentTags")]
        public ActionResult<List<string>> GetCommentTags()
        {
            List<string> commentTagTypes = Enum.GetNames(typeof(CommentTag)).ToList();
            return Ok(commentTagTypes);
        }

        [HttpGet("currentUser")]
        public async Task<ActionResult<object>> GetUserInfo()
        {
            IdentityUser currUser = await _userManager.GetUserAsync(User);
            IList<string> roles = await _userManager.GetRolesAsync(currUser);
            return Ok(new { Username = currUser.UserName, Roles = roles });
        }

        [HttpGet("users")]
        public async Task<ActionResult<object>> GetUsersInfo()
        {
            var userList = await _context.Users.ToListAsync();
            var userInfo = userList.Select(async u => new { u.Id, u.UserName, Roles = await _userManager.GetRolesAsync(u) }).Select(t => t.Result);
            return Ok(userInfo);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportingCase>> GetCaseItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // get the reporting case
            ReportingCase repCase = await _context.ReportingCases
                                            .Include(rc => rc.Comments)
                                            .Include(rc => rc.CaseItems)
                                            .Include(rc => rc.ConcernedAgencies)
                                            .SingleOrDefaultAsync(rc => rc.Id == id);
            if (repCase == default)
            {
                return NotFound();
            }
            // get the logged in user id
            string currUserId = _userManager.GetUserId(User);
            // check if user is admin
            bool isCurrUserAdmin = User.IsInRole(SecurityConstants.AdminRoleString);
            // check if user is not authorized to see the case
            if (!(isCurrUserAdmin || repCase.ConcernedAgencies.Any(ca => ca.UserId == currUserId) || repCase.CreatedById == currUserId))
            {
                return Unauthorized();
            }
            return repCase;
        }

        [HttpPost("{id}/addComment")]
        public async Task<ActionResult> AddCaseComment([FromRoute]int? id, [FromBody]CommentAddFormModel vm)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // get the reporting case
            ReportingCase repCase = await _context.ReportingCases.Include(rc => rc.ConcernedAgencies)
                                            .Include(rc => rc.CreatedBy)
                                            .Include(rc => rc.Comments).ThenInclude(co => co.CreatedBy)
                                            .Include(rc => rc.Comments).ThenInclude(co => co.LastModifiedBy)
                                            .Include(rc => rc.ConcernedAgencies).ThenInclude(ca => ca.User)
                                            .SingleOrDefaultAsync(rc => rc.Id == id);
            if (repCase == default)
            {
                return NotFound();
            }
            // get the logged in user id
            string currUserId = _userManager.GetUserId(User);
            // check if user is admin
            bool isCurrUserAdmin = User.IsInRole(SecurityConstants.AdminRoleString);
            // check if user is not authorized to see the case
            if (!(isCurrUserAdmin || repCase.ConcernedAgencies.Any(ca => ca.UserId == currUserId) || repCase.CreatedById == currUserId))
            {
                return Unauthorized();
            }

            // get the latest comment
            ReportingCaseComment latestComment = repCase.Comments.OrderByDescending(co => co.Created).FirstOrDefault();
            if (latestComment != default && latestComment.Tag.Equals(CommentTag.Closed))
            {
                // issue is closed in this block
                // check if admin user is trying to re open issue
                if (vm.Tag.Equals(CommentTag.Reopened))
                {
                    if (!isCurrUserAdmin)
                    {
                        return BadRequest("Only admin can reopen an issue");
                    }
                }
                else
                {
                    return BadRequest($"This issue with id {id} is already closed");
                }
            }
            else
            {
                // issue is open in this block
                if (vm.Tag.Equals(CommentTag.Reopened))
                {
                    return BadRequest("We can only reopen a closed issue");
                }
                if (vm.Tag.Equals(CommentTag.Closed))
                {
                    if (!isCurrUserAdmin)
                    {
                        return BadRequest("Only admin can close an issue");
                    }
                }
            }

            // add new comment
            _context.ReportingCaseComments.Add(new ReportingCaseComment { Comment = vm.Comment, Tag = (CommentTag)Enum.Parse(typeof(CommentTag), vm.Tag), ReportingCaseId = repCase.Id });
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("deleteComment/{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.ReportingCaseComments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            // get the logged in user id
            string currUserId = _userManager.GetUserId(User);
            // check if user is admin
            bool isCurrUserAdmin = User.IsInRole(SecurityConstants.AdminRoleString);

            // check if current user is neither admin nor creator
            if (!isCurrUserAdmin && comment.CreatedById != currUserId)
            {
                return Unauthorized("Only creator / admin can delete the comment");
            }

            _context.ReportingCaseComments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }


    }

}