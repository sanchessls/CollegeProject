using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{
    public class BaseModelUser : PageModel
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        public string UserGroup = "";
        private ApplicationUser _userIdentity = null;
        protected ApplicationUser userIdentity() 
        {
            if (User != null)
            {
                _userIdentity = _userManager.GetUserAsync(User).Result;                
            }

            if (_userIdentity != null)
            {
                UserGroup = _userIdentity.Id;
            }

            return _userIdentity;
        }

        public BaseModelUser(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            userIdentity();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (userIdentity() == null)
            {
                return RedirectToPage(".Login");
            }

            RedirectToPageResult validator = await Validator();
            if (validator != null)
            {
                return validator;
            }

            await LoadAsync();
            return Page();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<RedirectToPageResult> Validator()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return null;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task LoadAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //Must be overrided
        }
    }



}
