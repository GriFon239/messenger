using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Messenger.Areas.Api.Models;
using Messenger.Data;
using Messenger.Entities;
using Microsoft.AspNetCore.Identity;
using Messenger.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Areas.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/messages")]
    [Authorize]
    public class MessagesController : Controller
    {
        protected readonly ApplicationDbContext Context;
        protected readonly UserManager<ApplicationUser> UserManager;

        
        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            Context = context;
            UserManager = userManager;
        }

        
        [HttpGet]
        [Route("{name}")]
        public async Task<IEnumerable<MessageViewModel>> Get(string name)
        {
            var user = await UserManager.FindByNameAsync(name);
            var currentId = (User.Identity as ClaimsIdentity)
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            var currentUser = await UserManager.FindByIdAsync(currentId);

            if (user == null)
                return new List<MessageViewModel>();

            var messages = await Context.Messages.Where(
                m =>
                    (m.FromId == currentId || m.FromId == user.Id) &&
                    (m.ToId == currentId || m.ToId == user.Id))
                .OrderBy(m => m.Date)
                .ToListAsync();

            return messages.Select(m => new MessageViewModel
            {
                Date = m.Date,
                Text = m.Text,
                User = m.FromId == currentId ? currentUser.UserName : user.UserName
            });
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody]MessageViewModel message)
        {
            var fromId = (User.Identity as ClaimsIdentity)
                .FindFirst(c => c.Type == ClaimTypes.NameIdentifier)
                .Value;
            var toUser = await UserManager.FindByNameAsync(message.User);

            if (toUser == null)
                return BadRequest();

            Context.Messages.Add(new Message
            {
                Date = message.Date,
                Text = message.Text,
                FromId = fromId,
                ToId = toUser.Id
            });

            await Context.SaveChangesAsync();

            return Ok();
        }
    }
}