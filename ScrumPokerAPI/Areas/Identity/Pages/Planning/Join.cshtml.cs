using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrumPokerAPI.Context;
using ScrumPokerAPI.Models;
using Sketch7.Core;

namespace ScrumPokerAPI.Areas.Identity.Pages
{
    public class JoinModel : BaseModelDatabaseUser
    {

        public JoinModel(ApplicationContext context, UserManager<IdentityUser> userManager):base(context, userManager)
        {
        }

        [BindProperty]
        public int SessionCode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {           
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model!");
                return Page();
            }

            //todo: evaluate if user CAN JOIN first
            //todo: Check if session EXISTS after all

            PlanningSessionUser planningSessionUser = _appContext.PlanningSessionUser.Where(x => x.UserId == userIdentity().Id && x.PlanningSessionId == SessionCode).FirstOrDefault();

            if (planningSessionUser == null)
            {
                planningSessionUser = new PlanningSessionUser()
                {
                    PlanningSessionId = SessionCode,
                    UserId = userIdentity().Id,
                    UserIsCreator = false
                };

                _appContext.PlanningSessionUser.Add(planningSessionUser);

                await _appContext.SaveChangesAsync();
            }

            return RedirectToPage("./Session", new { id = SessionCode.ToString() });
        }
    }
}
