using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration.UserSecrets;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{
    public class CreateModel : BaseModelDatabaseUser
    {

        public CreateModel(ApplicationContext context, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }   

        [BindProperty]
        public string PlanningSessionDescription { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            PlanningSession planningSession = new PlanningSession
            {
                CreationDate = DateTime.Now,
                Description = PlanningSessionDescription,
                Status = EnumPlanningSession.Open
            };
            _appContext.PlanningSession.Add(planningSession);

            await _appContext.SaveChangesAsync();

            PlanningSessionUser planningSessionUser = new PlanningSessionUser()
            {
                PlanningSessionId = planningSession.Id,
                UserId = userIdentity().Id,
                UserIsCreator = true
            };

            _appContext.PlanningSessionUser.Add(planningSessionUser);

            await _appContext.SaveChangesAsync();

            return RedirectToPage("./Session", new { id = planningSession.Id });
        }
    }
}
