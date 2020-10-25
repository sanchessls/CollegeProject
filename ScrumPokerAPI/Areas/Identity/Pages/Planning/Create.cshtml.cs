using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration.UserSecrets;
using ScrumPokerAPI.Context;
using ScrumPokerAPI.Models;

namespace ScrumPokerAPI.Areas.Identity.Pages
{
    public class CreateModel : BaseModelDatabaseUser
    {

        public CreateModel(ApplicationContext context, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }   

        [BindProperty]
        public PlanningSession PlanningSession { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            PlanningSession.CreationDate = DateTime.Now;
            PlanningSession.Status = 1;
            PlanningSession.UserId = userIdentity().Id;
            _appContext.PlanningSession.Add(PlanningSession);
            await _appContext.SaveChangesAsync();

            return RedirectToPage("./Session", new { id = PlanningSession.Id });
        }
    }
}
