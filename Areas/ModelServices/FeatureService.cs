using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrumPokerPlanning.Areas.Identity.Pages;
using ScrumPokerPlanning.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.ModelServices
{
    public interface IFeatureService
    {
        List<UsersVoting> GetAll(ApplicationContext _appContext, int sessionid, int featureid);
    }
    public class FeatureService : IFeatureService
    {
        public List<UsersVoting> GetAll(ApplicationContext _appContext, int sessionid,int featureid)
        {
            var usersVoting = _appContext.PlanningSessionUser.
                                Where(x => x.PlanningSessionId == sessionid).
                                Select(g => new UsersVoting()
                                {
                                    Status = (_appContext.FeatureUser.Where(a => a.FeatureId == featureid && a.UserId == g.UserId).Sum(p => p.SelectedValue) > 0),
                                    UserName = _appContext.Users.Where(p => p.Id == g.UserId).FirstOrDefault().UserName
                                }).ToList();

            return usersVoting;

        }
   }
}
