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
    public partial class JiraFilter : BaseModelDatabaseUser
    {
        IJiraService _JiraService;
        private int JiraFilterCode = 0;
        
        private readonly IHubContext<FeatureHub, IFeature> _FeatureHub;
        public JiraFilter(ApplicationContext context, UserManager<ApplicationUser> userManager, IJiraService jiraService, IHubContext<FeatureHub, IFeature> FeatureHub) : base(context, userManager)
        {
            _JiraService = jiraService;
            _FeatureHub = FeatureHub;
        }

        [BindProperty]
        [Display(Name = "Is Favourite Filters Only")]
        public string FavouriteChoosed { get; set; }
        public string SessionCode { get; set; }

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

            JiraFilterCode = idSessionFilter;

            SessionCode = aSessionFilterCode;
            string? favouriteFilters = Request.Query["favourite"];

            FavouriteChoosed = favouriteFilters;

            return null;
        }

        public override Task LoadAsync()
        {            

            return base.LoadAsync();
        }
        
    }
}
