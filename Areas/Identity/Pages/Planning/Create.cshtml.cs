using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration.UserSecrets;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{ 
    [Authorize]
    public class CreateModel : BaseModelDatabaseUser
    {

        public CreateModel(ApplicationContext context, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }   

        [BindProperty]
        [Display(Name = "Planning session description(*)")]
        [StringLength(200)]
        public string PlanningSessionDescription { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            string generatedKey = Helpers.CodeGenerator.Generate(userIdentity().Id);

            PlanningSession planningSession = new PlanningSession
            {
                CreationDate = DateTime.Now,
                Description = PlanningSessionDescription,
                Status = EnumPlanningSession.Open,
                SessionCode = generatedKey
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

            return RedirectToPage("./Session", new { code = planningSession.SessionCode });
        }
    }
}
