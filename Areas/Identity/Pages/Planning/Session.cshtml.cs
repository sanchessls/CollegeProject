using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.Repositories.Implementation;
using ScrumPokerPlanning.Repositories.Interface;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{
    public partial class Session : BaseModelDatabaseUser
    {        
        public Session(ApplicationContext context, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }
        [BindProperty]
        [Display(Name = "New feature description")]
        public string FeatureDescription { get; set; }

        [BindProperty]
        [Display(Name = "Identification")]
        [MaxLength(15)]
        public string FeatureIdentification { get; set; }


        [BindProperty]
        public int PlanningSessionId { get; set; }
        public string DescriptionSession { get; set; }
        public bool UserCreator { get; set; }
        public List<ScrumPokerPlanning.Models.Feature> FeaturesList { get; set; }

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

            ScrumPokerPlanning.Models.Feature feature = new ScrumPokerPlanning.Models.Feature
            {
                SessionId = PlanningSessionId,
                Status = EnumFeature.Open,
                CreationDate = DateTime.Now,
                Description = FeatureDescription,
                Identification = FeatureIdentification
            };

            _appContext.Feature.Add(feature);
            await _appContext.SaveChangesAsync();
            

            ScrumPokerPlanning.Models.FeatureUser featureUser = new ScrumPokerPlanning.Models.FeatureUser
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
