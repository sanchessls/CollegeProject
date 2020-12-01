using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.HashFunction.xxHash;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.Repositories.Implementation;
using ScrumPokerPlanning.Repositories.Interface;
using ScrumPokerPlanning.Services;
using ScrumPokerPlanning.SignalRServerSide;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{
    public partial class Session : BaseModelDatabaseUser
    {
        IJiraService _JiraService;
        private readonly IHubContext<FeatureHub, IFeature> _FeatureHub;
        public Session(ApplicationContext context, UserManager<ApplicationUser> userManager, IJiraService jiraService, IHubContext<FeatureHub, IFeature> FeatureHub) : base(context, userManager)
        {
            _JiraService = jiraService;
            _FeatureHub = FeatureHub;
        }

        [BindProperty]
        [Display(Name = "Issue description*")]
        public string FeatureDescription { get; set; }

        [BindProperty]
        [Display(Name = "Jira identifier")]
        public string JiraIdentification { get; set; }
        

        [BindProperty]
        [Display(Name = "Issue identifier*")]
        [MaxLength(15)]
        public string FeatureIdentification { get; set; }

        [BindProperty]
        public int PlanningSessionId { get; set; }
        [BindProperty]
        public string DescriptionSession { get; set; }
        [BindProperty]
        public bool UserCreator { get; set; }
        [BindProperty]
        public string SessionCode { get; set; }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async override Task<RedirectToPageResult> Validator()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            string aSessionCode = Request.Query["code"];

            int idSession = 0;

            if (aSessionCode == null)
            {
                ModelState.AddModelError(string.Empty, "Null Session!");
                return RedirectToPage(".Join");
            }


            var query = _appContext.PlanningSession.Where(x => x.SessionCode == aSessionCode);

            if (query.Any())
            {
                idSession = query.FirstOrDefault().Id;
            }

            if (idSession <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Session!");
                return RedirectToPage(".Join");
            }

            SessionCode = aSessionCode;

            return null;
        }

        public override Task LoadAsync()
        {
            string SessionCode = Request.Query["code"];

            int idSession = 0;

            var query = _appContext.PlanningSession.Where(x => x.SessionCode == SessionCode);
          
            if (query.Any())
            {
                idSession = query.FirstOrDefault().Id;
            }

            if (idSession <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Session!");
            }

            PlanningSession SessionObject = _appContext.PlanningSession.Where(x => x.Id == idSession).Include(x => x.PlanningSessionUser).FirstOrDefault();
            PlanningSessionId = SessionObject.Id;
            DescriptionSession = SessionObject.Description;
            
            //FeaturesList = 

            //If the Creator is the one Logged
            //We will offer the option of create features
            UserCreator = SessionObject.PlanningSessionUser.Where(x => x.UserId == userIdentity().Id).FirstOrDefault().UserIsCreator;

            return base.LoadAsync();
        }

        public async Task<IActionResult> OnPostJiraImportAsync()
        {
            if ((JiraIdentification == null) || (JiraIdentification.Trim() == ""))
            {
                ModelState.AddModelError("JiraIdentification", "Invalid Jira identifier!");
                return Page();
            }

            ObjJiraFeature JiraReturn = _JiraService.GetJiraFeature(JiraIdentification, userIdentity().JiraWebSite, userIdentity().JiraEmail, userIdentity().JiraKey);

            if (!JiraReturn.Success)
            {
                ModelState.AddModelError("JiraIdentification", JiraReturn.MessageToUi);
                Console.WriteLine(JiraReturn.Exception);
                return Page();
            }


            string Identificator = JiraReturn.Identificator;
            string subject = JiraReturn.Subject;
            string link = userIdentity().JiraWebSite + "/browse/" + Identificator;

            Models.Feature feature = await CreateFeatureAsync(Identificator, subject, link);

            return RedirectToPage("./feature", new { id = feature.Id });



        }

        private async Task<Models.Feature> CreateFeatureAsync(string identificator, string subject,string link)
        {
            //the ones  who call this should treat the result in case of exception
            Models.Feature feature = new Models.Feature
            {
                SessionId = PlanningSessionId,
                Status = EnumFeature.Open,
                CreationDate = DateTime.Now,
                Description = subject,
                Identification = identificator,
                Link = link
            };

            _appContext.Feature.Add(feature);
            await _appContext.SaveChangesAsync();

            //Models.FeatureUser featureUser = new Models.FeatureUser
            //{
            //    FeatureId = feature.Id,
            //    UserId = userIdentity().Id,
            //};

            //_appContext.FeatureUser.Add(featureUser);
            //await _appContext.SaveChangesAsync();

            await AddUsersInFeatureAsync(PlanningSessionId, userIdentity().Id);

            var idList = _appContext.PlanningSessionUser.Where(x => x.PlanningSessionId == PlanningSessionId).Select(x => x.UserId).ToList();
            idList.ForEach(x =>
            {
                _FeatureHub.Clients.Group(x).StatusFeatureUpdated(this.PlanningSessionId, userIdentity().Id);
            });


            return feature;
        }


        private async Task AddUsersInFeatureAsync(int sessionCode, string idUser)
        {
            //We have a identic funtion in session, that must be unique with this one
            var featuresInSession = _appContext.Feature.Where(x => x.SessionId == sessionCode).ToList();

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
                    }
                }

            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model!");
                return Page();
            }


            if ((FeatureIdentification == null) || (FeatureIdentification.Trim() == ""))
            {
                ModelState.AddModelError("FeatureIdentification", "Invalid identifier!");
                return Page();
            }



            if ((FeatureDescription == null) || (FeatureDescription.Trim() == ""))
            {
                ModelState.AddModelError("FeatureDescription", "Invalid Description!");
                return Page();
            }
            
            CreateFeatureAsync(FeatureIdentification, FeatureDescription,"").Wait();          

            return RedirectToPage("./session", new { code = SessionCode.ToUpper() });
        }

    }
}
