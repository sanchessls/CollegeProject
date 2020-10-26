using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrumPokerPlanning.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{
    public class BaseModelUser : PageModel
    {
        protected readonly UserManager<IdentityUser> _userManager;
        private IdentityUser _userIdentity = null;
        protected IdentityUser userIdentity() 
        {
            if (User != null)
            {
                _userIdentity = _userManager.GetUserAsync(User).Result;
            }

            return _userIdentity;
        }

        public BaseModelUser(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
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

        public virtual async Task<RedirectToPageResult> Validator()
        {
            return null;
        }

        public virtual async Task LoadAsync()
        {
            //Must be overrided
        }
    }



}
