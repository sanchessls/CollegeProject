 using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.SignalRServerSide;
using Sketch7.Core;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{
    public class JoinModel : BaseModelDatabaseUser
    {

        private readonly IHubContext<FeatureHub, IFeature> _FeatureHub;
        public JoinModel(ApplicationContext context, UserManager<ApplicationUser> userManager, IHubContext<FeatureHub, IFeature> featureHub) :base(context, userManager)
        {
            _FeatureHub = featureHub;
        }

        [BindProperty]
        [Display(Name = "Session code*")]
        public string SessionCode { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {           
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model!");
                return Page();
            }

            //todo: evaluate if user CAN JOIN first
            //todo: Check if session EXISTS after all

            int sessionCode = 0;
            if (SessionCode == null)
            {
                SessionCode = "";
            }
            else
            {
                SessionCode = SessionCode.Trim();
            }
            var query = _appContext.PlanningSession.Where(x => x.SessionCode == SessionCode);

            if (query.Any())
            {
                sessionCode = query.FirstOrDefault().Id;
            }

            if (sessionCode <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Session!");
                return Page();
            }

            PlanningSessionUser planningSessionUser = _appContext.PlanningSessionUser.Where(x => x.UserId == userIdentity().Id && x.PlanningSessionId == sessionCode).FirstOrDefault();

            if (planningSessionUser == null)
            {
                planningSessionUser = new PlanningSessionUser()
                {
                    PlanningSessionId = sessionCode,
                    UserId = userIdentity().Id,
                    UserIsCreator = false
                };

                _appContext.PlanningSessionUser.Add(planningSessionUser);

                await _appContext.SaveChangesAsync();
            }

            await AddUsersInFeatureAsync(sessionCode, userIdentity().Id);       

            return RedirectToPage("./Session", new { code = SessionCode });
        }

        private async Task AddUsersInFeatureAsync(int sessionCode, string idUser)
        {
            //We have a identic funtion in session, that must be unique with this one
            var featuresInSession = _appContext.Feature.Where(x => x.SessionId == sessionCode).ToList();

            bool UserAdded = false;

            foreach (var item in featuresInSession)
            {
                var existRelationship = _appContext.FeatureUser.Where(x => x.FeatureId == item.Id && x.UserId == idUser).FirstOrDefault();

                if (existRelationship == null)
                {
                    if (item.Status == EnumFeature.Open)
                    {
                        var featureUser = new FeatureUser()
                        {
                            UserId = idUser,
                            FeatureId = item.Id
                        };

                        _appContext.FeatureUser.Add(featureUser);

                        await _appContext.SaveChangesAsync();
                        
                        //So we can send a signalR
                        UserAdded = true;
                    }

                }

            }

            if (UserAdded)
            {

                //If we make that change we send it to the session page , so that the features can be updated in the view                
                var idList = _appContext.PlanningSessionUser.Where(x => x.PlanningSessionId == sessionCode).Select(x => x.UserId).ToList();
                idList.ForEach(x =>
                {
                    _FeatureHub.Clients.Group(x).StatusFeatureUpdated(sessionCode, userIdentity().Id);
                });

            };

        }
    }
}
