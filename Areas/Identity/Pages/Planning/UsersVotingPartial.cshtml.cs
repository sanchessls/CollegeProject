using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrumPokerPlanning.Areas.Identity.Pages;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.ModelServices;
using System.Collections.Generic;
using System.Globalization;

namespace ScrumPokerPlanning.UsersVotingPartial
{
    public class UsersVotingModel : PageModel
    {
        private IFeatureService _featureService;
        private readonly ApplicationContext _appContext;
        public UsersVotingModel(IFeatureService FeatureService, ApplicationContext appContext)
        {
            _featureService = FeatureService;
            _appContext = appContext;
        }        
        public void OnGet()
        {
        }

        public PartialViewResult OnGetUsersVoting(int sessionid,int featureid)
        {
            List<UsersVoting> usersVoting = _featureService.GetAll(_appContext, sessionid, featureid);
            return Partial("_UsersVotingPartial", usersVoting);
        }


        public PartialViewResult OnGetLoading()
        {
            return Partial("_LoadingPartial");
        }


    }

}
