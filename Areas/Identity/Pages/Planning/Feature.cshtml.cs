using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.SignalRServerSide;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{
    public partial class Feature : BaseModelDatabaseUser
    {
        private readonly IHubContext<FeatureHub, IFeature> _FeatureHub;
        public Feature(ApplicationContext context, UserManager<IdentityUser> userManager, IHubContext<FeatureHub, IFeature> FeatureHub) : base(context, userManager)
        {
            _FeatureHub = FeatureHub;
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

        public override Task LoadAsync()
        {
            int idFeature = Convert.ToInt32(Request.Query["id"]);
            Models.Feature FeatureObject = _appContext.Feature.Where(x => x.Id == idFeature).Include(x => x.PlanningSession).FirstOrDefault();
            
            PlanningSessionId = FeatureObject.PlanningSession.Id;
            FeatureId = FeatureObject.Id;
            DescriptionSession = FeatureObject.PlanningSession.Description;
            DescriptionFeature = FeatureObject.Description;

            //If the Creator is the one Logged
            //We will offer more features            
            UserCreator = _appContext.PlanningSessionUser.Where(x => x.UserId == userIdentity().Id && x.PlanningSessionId == FeatureObject.PlanningSession.Id).FirstOrDefault().UserIsCreator;          


            return base.LoadAsync();
        }
        public async Task<IActionResult> OnPostCloseFeatureAsync(int featureid,int sessionid)
        {
            var feature = _appContext.Feature.Where(x => x.Id == featureid).FirstOrDefault();

            if (feature != null)
            {
                feature.Status = EnumFeature.Closed;
                _appContext.Feature.Update(feature);
                await _appContext.SaveChangesAsync();

                //If we make that change we send it to the session page , so that the features can be updated in the view                
                var idList = _appContext.PlanningSessionUser.Where(x => x.PlanningSessionId == sessionid).Select(x => x.UserId).ToList();
                idList.ForEach(x =>
                {
                    _FeatureHub.Clients.Group(x).StatusFeatureUpdated(sessionid, userIdentity().Id);
                });

            }

            return RedirectToPage("./Session", new { id = sessionid });
        }
        public async Task<IActionResult> OnPostCancelFeatureAsync(int featureid, int sessionid)
        {
            var feature = _appContext.Feature.Where(x => x.Id == featureid).FirstOrDefault();

            if (feature != null)
            {
                feature.Status = EnumFeature.Canceled;
                _appContext.Feature.Update(feature);
                await _appContext.SaveChangesAsync();

                //If we make that change we send it to the session page , so that the features can be updated in the view                
                var idList = _appContext.PlanningSessionUser.Where(x => x.PlanningSessionId == sessionid).Select(x => x.UserId).ToList();
                idList.ForEach(x =>
                {
                    _FeatureHub.Clients.Group(x).StatusFeatureUpdated(sessionid, userIdentity().Id);
                });
            }

            return RedirectToPage("./Session", new { id = sessionid });
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

            //If we make an estimate we send a note from everyone that is on the
            //session page and on the feature page that belongs to it
            var idList = _appContext.PlanningSessionUser.Where(x => x.PlanningSessionId == this.PlanningSessionId).Select(x => x.UserId).ToList();
            idList.ForEach(x =>
            {
                _FeatureHub.Clients.Group(x).FeatureUpdated(this.FeatureId, userIdentity().Id);
            });

            return RedirectToPage("./Session", new { id = PlanningSessionId });
        }

    }

    public class UsersVoting
    {
        public string UserName { get; set; }
        public bool Status { get; set; }
    }
}
