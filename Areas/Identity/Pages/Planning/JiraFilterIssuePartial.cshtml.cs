using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Internal;
using ScrumPokerPlanning.Areas.Identity.Pages;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.ModelServices;
using ScrumPokerPlanning.Services;
using System.Collections.Generic;
using System.Globalization;

namespace ScrumPokerPlanning.JiraFilterIssuePartial
{
    public class JiraFilterIssuePartialModel : BaseModelUser
    {
        private IJiraService _jiraService;
        private readonly ApplicationContext _appContext;
        public JiraFilterIssuePartialModel(IJiraService jService, ApplicationContext appContext, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _jiraService = jService;
            _appContext = appContext;
        }        

        public PartialViewResult OnGetJiraFilterIssue(string code,int filterID )
        {
            JiraIssue JiraFilterIssues = _jiraService.GetJiraIssuesFromFilter(userIdentity().JiraWebSite, userIdentity().JiraEmail, userIdentity().JiraKey, filterID);
            
            if (JiraFilterIssues == null)
            {
                JiraFilterIssues = new JiraIssue();
                JiraFilterIssues.issues = new List<Issue>();
            }

#pragma warning disable EF1001 // Internal EF Core API usage.
            if (JiraFilterIssues.issues.Any())
#pragma warning restore EF1001 // Internal EF Core API usage.
            {
                return Partial("_JiraFilterIssuePartial", JiraFilterIssues);

            }
            else
            {
                return Partial("_JiraFilterIssuePartialEmpty", JiraFilterIssues);

            }
        }
    }

}
