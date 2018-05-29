using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Messenger.Areas.Api.Models;
using Messenger.Models;
using Messenger.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Messenger.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Messenger.Areas.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/contacts")]
    [Authorize]
    public class ContactsController : Controller
    {
        protected readonly UserManager<ApplicationUser> UserManager;

        public ContactsController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<ContactViewModel>> Get()
        {
            var users = await UserManager.Users
                .Select(u => u.UserName)
                .ToListAsync();

            return users.Select(u => new ContactViewModel
            {
                Name = u
            });
        }
    }
}