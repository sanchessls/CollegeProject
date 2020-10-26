using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{
    public partial class Feature : BaseModelDatabaseUser
    {        
        public Feature(ApplicationContext context, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }
        [BindProperty]
        public float SelectedValue { get; set; }
        [BindProperty]
        public int PlanningSessionId { get; set; }
        [BindProperty]
        public int FeatureId { get; set; }
        public string DescriptionSession { get; set; }
        public string DescriptionFeature { get; set; }
        public bool UserCreator { get; set; }
        public List<UsersVoting> UsersVoting { get; set; }



        public override Task LoadAsync()
        {
            int idFeature = Convert.ToInt32(Request.Query["id"]);
            ScrumPokerPlanning.Models.Feature FeatureObject = _appContext.Feature.Where(x => x.Id == idFeature).Include(x => x.PlanningSession).FirstOrDefault();
            
            PlanningSessionId = FeatureObject.PlanningSession.Id;
            FeatureId = FeatureObject.Id;
            DescriptionSession = FeatureObject.PlanningSession.Description;
            DescriptionFeature = FeatureObject.Description;

            //If the Creator is the one Logged
            //We will offer more features            
            UserCreator = _appContext.PlanningSessionUser.Where(x => x.UserId == userIdentity().Id && x.PlanningSessionId == FeatureObject.PlanningSession.Id).FirstOrDefault().UserIsCreator;

            

            UsersVoting = _appContext.PlanningSessionUser.
                                      Where(x => x.PlanningSessionId == PlanningSessionId).
                                      Select(g => new UsersVoting() 
                                             { 
                                               Status = (_appContext.FeatureUser.Where(a => a.FeatureId == FeatureId && a.UserId == g.UserId).Sum(p => p.SelectedValue) > 0), 
                                               UserName = _appContext.Users.Where(p => p.Id == g.UserId).FirstOrDefault().UserName
                                             }).ToList();

            return base.LoadAsync();
        }

 
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model!");
                return Page();
            }

            var ObjectFeatureUser = _appContext.FeatureUser.Where(x => x.UserId == userIdentity().Id && x.FeatureId == FeatureId).FirstOrDefault();

            if (ObjectFeatureUser != null)
            {
                ObjectFeatureUser.SelectedValue = this.SelectedValue;
                _appContext.FeatureUser.Update(ObjectFeatureUser);
            }
            else
            {
                FeatureUser featureUser = new FeatureUser
                {
                    FeatureId = this.FeatureId,
                    UserId = userIdentity().Id,
                    SelectedValue = this.SelectedValue
                };
                _appContext.FeatureUser.Add(featureUser);
            }


            await _appContext.SaveChangesAsync();

            return RedirectToPage("./Session", new { id = PlanningSessionId });
        }

    }

    public class UsersVoting
    {
        public string UserName { get; set; }
        public bool Status { get; set; }
    }
}
