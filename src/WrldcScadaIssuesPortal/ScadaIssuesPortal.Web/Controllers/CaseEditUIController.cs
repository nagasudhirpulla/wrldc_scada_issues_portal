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
using ScadaIssuesPortal.App;
using ScadaIssuesPortal.Web.Models;
using MoreLinq.Extensions;
using System.IO;

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
        private readonly IEmailSender _emailSender;
        private readonly FolderPaths _folderPaths;

        public CaseEditUIController(ILogger<ReportingCasesController> logger, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IEmailSender emailSender, FolderPaths folderPaths)
        {
            _userManager = userManager;
            _logger = logger;
            _context = dbContext;
            _emailSender = emailSender;
            _folderPaths = folderPaths;
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

        [HttpPost("{id}")]
        public async Task<ActionResult<ReportingCase>> EditCaseItem(int? id, [FromBody]ReportingCase vm)
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
                                            .SingleOrDefaultAsync(rc => rc.Id == id);
            if (repCase == null)
            {
                return NotFound();
            }

            // get the logged in user id
            string currUserId = _userManager.GetUserId(User);
            // check if user is admin
            bool isCurrUserAdmin = User.IsInRole(SecurityConstants.AdminRoleString);
            // check if requesting user is creator / concerned agency for authorizing issue editing
            if (!(isCurrUserAdmin || repCase.CreatedById == currUserId))
            {
                return Unauthorized();
            }

            // update the reporting case
            repCase.ResolutionTime = vm.ResolutionTime;
            repCase.DownTime = vm.DownTime;
            repCase.CaseItems = vm.CaseItems;
            try
            {
                _context.Update(repCase);
                await _context.SaveChangesAsync();


                // get the concerned agency Ids
                List<ReportingCaseConcerned> existingconcernedAgencies = await _context.ReportingCaseConcerneds
                                                    .Where(rcc => rcc.ReportingCaseId == id)
                                                    .ToListAsync();

                foreach (ReportingCaseConcerned conAgency in existingconcernedAgencies)
                {
                    if (!vm.ConcernedAgencies.Any(ca => ca.UserId == conAgency.UserId))
                    {
                        //check if existing concerned agency is deleted
                        _context.ReportingCaseConcerneds.Remove(conAgency);
                        await _context.SaveChangesAsync();
                        IdentityUser user = await _context.Users.FindAsync(conAgency.UserId);
                        await _emailSender.SendEmailAsync(user.Email,
                                "WRLDC Issue alert",
                                $"Sir/Madam,<br>You are being removed from an issue with id <b>{repCase.Id}</b> in WRLDC issues portal." +
                                "<br><br>For kind information please<br><br>Regards<br>IT WRLDC");
                    }
                }

                foreach (ReportingCaseConcerned concernedAgency in vm.ConcernedAgencies)
                {
                    string concernedAgencyId = concernedAgency.UserId;
                    if (!existingconcernedAgencies.Any(ca => ca.UserId == concernedAgencyId))
                    {
                        // check if we need to add a new concerned agency
                        // _context.ReportingCaseConcerneds.Remove(conc);
                        // await _context.SaveChangesAsync();
                        ReportingCaseConcerned concerned = new ReportingCaseConcerned()
                        {
                            ReportingCaseId = repCase.Id,
                            UserId = concernedAgencyId
                        };
                        _context.Add(concerned);
                        int numInserted = await _context.SaveChangesAsync();
                        IdentityUser user = await _context.Users.FindAsync(concernedAgencyId);
                        string caseDetail = String.Join("<br>", repCase.CaseItems.Select(ci => $"{ci.Question} - {ci.Response}").ToArray());
                        await _emailSender.SendEmailAsync(user.Email,
                                "WRLDC Issue alert",
                                $"Sir/Madam,<br>You are being associated with an issue with id <b>{repCase.Id}</b> in WRLDC issues portal." +
                                "<br><b>Issue Details</b>" +
                                $"<br>{caseDetail}" +
                                "<br><br>For kind information please<br><br>Regards<br>IT WRLDC");
                    }
                }
                return Ok(repCase);
            }
            catch (DbUpdateConcurrencyException)
            {
                // check if the we are trying to edit was already deleted due to concurrency
                if (!_context.ReportingCases.Any(rc => rc.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
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
            ReportingCaseComment newComm = new ReportingCaseComment() { Comment = vm.Comment, Tag = vm.Tag, ReportingCaseId = repCase.Id };
            _context.ReportingCaseComments.Add(newComm);
            await _context.SaveChangesAsync();
            return Ok(newComm);
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

        [HttpDelete("attachment/{id}")]
        public async Task<IActionResult> DeleteAttachement([FromRoute] int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // get the reporting case
            ReportingCase repCase = await _context.ReportingCases.SingleOrDefaultAsync(rc => rc.Id == id);
            if (repCase == default)
            {
                return NotFound();
            }
            // get the logged in user id
            string currUserId = _userManager.GetUserId(User);
            // check if user is admin
            bool isCurrUserAdmin = User.IsInRole(SecurityConstants.AdminRoleString);
            // check if requesting user is creator / concerned agency for authorizing issue editing
            if (!(isCurrUserAdmin || repCase.CreatedById == currUserId))
            {
                return Unauthorized();
            }
            repCase.AttachmentName = null;
            repCase.AttachmentPath = null;
            //todo delete file from storage
            try
            {
                _context.Update(repCase);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                // check if the we are trying to edit was already deleted due to concurrency
                if (!_context.ReportingCases.Any(rc => rc.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpPost("attachment")]
        public async Task<IActionResult> AddAttachement([FromForm]CaseAttachmentFormModel caseAttachmentInfo)
        {
            // get the reporting case
            ReportingCase repCase = await _context.ReportingCases.SingleOrDefaultAsync(rc => rc.Id == caseAttachmentInfo.Id);
            if (repCase == default)
            {
                return NotFound();
            }
            // get the logged in user id
            string currUserId = _userManager.GetUserId(User);
            // check if user is admin
            bool isCurrUserAdmin = User.IsInRole(SecurityConstants.AdminRoleString);
            // check if requesting user is creator / concerned agency for authorizing issue editing
            if (!(isCurrUserAdmin || repCase.CreatedById == currUserId))
            {
                return Unauthorized();
            }
            // Getting Attachment
            IFormFile attachment = caseAttachmentInfo.CaseAttachment;
            // Saving attachment on Server
            if (attachment.Length > 0)
            {
                // create a new filename with timestamp
                string attachmentPath = $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}_{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";
                string filePath = Path.Combine(_folderPaths.AttachmentsFolder, attachmentPath);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    attachment.CopyTo(fileStream);
                    repCase.AttachmentName = attachment.FileName;
                    repCase.AttachmentPath = attachmentPath;
                }
            }
            try
            {
                _context.Update(repCase);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                // check if the we are trying to edit was already deleted due to concurrency
                if (!_context.ReportingCases.Any(rc => rc.Id == caseAttachmentInfo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}