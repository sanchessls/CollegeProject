using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ScrumPokerAPI.Context;
using ScrumPokerAPI.Models;
using ScrumPokerAPI.Repositories.Implementation;
using ScrumPokerAPI.Repositories.Interface;

namespace ScrumPokerAPI.Areas.Identity.Pages
{
    public partial class Session : BaseModelDatabaseUser
    {        
        public Session(ApplicationContext context, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }
        [BindProperty]
        public string FeatureDescription { get; set; }        
        [BindProperty]
        public int PlanningSessionId { get; set; }
        public string DescriptionSession { get; set; }
        public bool UserCreator { get; set; }
        public List<ScrumPokerAPI.Models.Feature> FeaturesList { get; set; }

        public override Task LoadAsync()
        {
            int idSession = Convert.ToInt32(Request.Query["id"]);
            PlanningSession SessionObject = _appContext.PlanningSession.Where(x => x.Id == idSession).Include(x => x.PlanningSessionUser).FirstOrDefault();
            PlanningSessionId = SessionObject.Id;
            DescriptionSession = SessionObject.Description;
            FeaturesList = _appContext.Feature.Where(x => x.SessionId == SessionObject.Id).Include(x => x.FeatureUser).ToList();

            //If the Creator is the one Logged
            //We will offer the option of create features
            UserCreator = SessionObject.PlanningSessionUser.Where(x => x.UserId == userIdentity().Id).FirstOrDefault().UserIsCreator;

            return base.LoadAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model!");
                return Page();
            }

            ScrumPokerAPI.Models.Feature feature = new ScrumPokerAPI.Models.Feature
            {
                SessionId = PlanningSessionId,
                Status = 1,
                CreationDate = DateTime.Now,
                Description = FeatureDescription
            };

            _appContext.Feature.Add(feature);
            await _appContext.SaveChangesAsync();
            

            ScrumPokerAPI.Models.FeatureUser featureUser = new ScrumPokerAPI.Models.FeatureUser
            {
                FeatureId = feature.Id,
                UserId = userIdentity().Id,                
            };

            _appContext.FeatureUser.Add(featureUser);
            await _appContext.SaveChangesAsync();




            return RedirectToPage("./session", new { id = PlanningSessionId });
        }

    }
}
