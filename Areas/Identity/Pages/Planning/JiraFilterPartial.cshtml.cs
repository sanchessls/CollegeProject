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

namespace ScrumPokerPlanning.JiraFilterPartial
{
    public class JiraFilterModel : BaseModelUser
    {
        private IJiraService _jiraService;
        private readonly ApplicationContext _appContext;
        public JiraFilterModel(IJiraService jService, ApplicationContext appContext, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _jiraService = jService;
            _appContext = appContext;
        }        

        public PartialViewResult OnGetJiraFilter(string SessionCode,string FavouriteChoosed)
        {
            List<Models.JiraFilter> JiraFilters = _jiraService.GetJiraFilter(userIdentity().JiraWebSite, userIdentity().JiraEmail, userIdentity().JiraKey,(FavouriteChoosed == "true"));

#pragma warning disable EF1001 // Internal EF Core API usage.
            if (JiraFilters.Any())
#pragma warning restore EF1001 // Internal EF Core API usage.
            {
                return Partial("_JiraFilterPartial", JiraFilters);

            }
            else
            {
                return Partial("_JiraFilterPartialEmpty", JiraFilters);

            }
        }
    }

}
