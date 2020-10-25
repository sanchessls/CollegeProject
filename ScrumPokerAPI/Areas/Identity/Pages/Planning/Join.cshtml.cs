using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrumPokerAPI.Context;
using ScrumPokerAPI.Models;

namespace ScrumPokerAPI.Areas.Identity.Pages
{
    public class JoinModel : BaseModelUser
    {

        public JoinModel(UserManager<IdentityUser> userManager):base( userManager)
        {
        }

        [BindProperty]
        public string SessionCode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {           
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model!");
                return Page();
            }

            return RedirectToPage("./Session", new { id = SessionCode });
        }
    }
}
