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
    public partial class JiraFilterIssue : BaseModelDatabaseUser
    {
        IJiraService _JiraService;
        IIssueService _IssueService;

        private readonly IHubContext<FeatureHub, IFeature> _FeatureHub;
        public JiraFilterIssue(ApplicationContext context, UserManager<ApplicationUser> userManager, IJiraService jiraService, IIssueService issueService, IHubContext<FeatureHub, IFeature> FeatureHub) : base(context, userManager)
        {
            _JiraService = jiraService;
            _IssueService = issueService;
            _FeatureHub = FeatureHub;
        }

        [BindProperty]
        [Display(Name = "Is Favourite Filters Only")]
        public int FilterID { get; set; }

        [BindProperty]
        public string SessionCode { get; set; }
        public string DescriptionSession { get; set; }

        public string DescriptionFilter { get; set; }
        
        public string Favourite { get; set; }

        [BindProperty]
        public List<string> AreChecked { get; set; }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async override Task<RedirectToPageResult> Validator()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            string aSessionFilterCode = Request.Query["code"];
            
            int idSessionFilter = 0;

            if (aSessionFilterCode == null)
            {
                ModelState.AddModelError(string.Empty, "Null Session Code!");
                return RedirectToPage(".Join");
            }
            

            var query = _appContext.PlanningSession.Where(x => x.SessionCode == aSessionFilterCode);

            if (query.Any())
            {
                idSessionFilter = query.FirstOrDefault().Id;
            }

            if (idSessionFilter <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Session Code!");
                return RedirectToPage(".Join");
            }
            

            SessionCode = aSessionFilterCode;

            DescriptionSession = _IssueService.GetSessionDescriptionByCode(SessionCode);

            string filterId = Request.Query["FilterID"];
            string filtername = Request.Query["filtername"];
            DescriptionFilter = filtername;
            //for the back filter button
            Favourite = Request.Query["favourite"];

            try
            {
                FilterID = Convert.ToInt32(filterId);
            }
            catch (Exception)
            {

                ModelState.AddModelError(string.Empty, "Invalid Filter ID!");
                return RedirectToPage(".Join");
            }

            return null;
        }

        public override Task LoadAsync()
        {            

            return base.LoadAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model!");
                return Page();
            }

            if (AreChecked == null) 
            {
                ModelState.AddModelError("AreChecked", "Error.");
                return Page();
            }

            if (AreChecked.Count <= 0)
            {
                ModelState.AddModelError("AreChecked", "At least one issue must be selected!");
                return Page();
            }

            AssociateIssue(AreChecked, SessionCode);

            return RedirectToPage("./session", new { code = SessionCode.ToUpper() });
        }

        private void AssociateIssue(List<string> areChecked, string sessionCode)
        {
            int sessionID = _IssueService.GetSessionIdByCode(sessionCode);

            List<string> ExistingIssues = _appContext.Feature.Where(x => (x.PlanningSession.Id == sessionID) && AreChecked.Contains(x.Identification)).Select(x => x.Identification).ToList();

            List<string> newOnes = areChecked.Except(ExistingIssues).ToList();

            foreach (var item in newOnes)
            {
                JiraIssueReturn JiraReturn = _JiraService.GetJiraFeature(item, userIdentity().JiraWebSite, userIdentity().JiraEmail, userIdentity().JiraKey);

                string Identificator = JiraReturn.Identificator;
                string subject = JiraReturn.Subject;
                string link = userIdentity().JiraWebSite + "/browse/" + Identificator;

                _IssueService.CreateFeatureAsync(sessionID, subject, Identificator, true, link, userIdentity().Id);
            }

            if (newOnes.Count > 0)
            {
                var idList = _appContext.PlanningSessionUser.Where(x => x.PlanningSession.SessionCode.ToUpper() == SessionCode.ToUpper()).Select(x => x.UserId).ToList();
                idList.ForEach(x =>
                {
                    _FeatureHub.Clients.Group(x).StatusFeatureUpdated(999, userIdentity().Id);
                });

            }

        }
    }
}
