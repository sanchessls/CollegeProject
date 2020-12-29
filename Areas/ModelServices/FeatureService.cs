using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrumPokerPlanning.Areas.Identity.Pages;
using ScrumPokerPlanning.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.ModelServices
{
    public interface IFeatureService
    {
        List<UsersVoting> GetUsersVoting(ApplicationContext _appContext, int sessionid, int featureid);
        List<Models.Feature> GetFeatures(ApplicationContext appContext, int sessionid);
    }
    public class FeatureService : IFeatureService
    {
        public List<Models.Feature> GetFeatures(ApplicationContext _appContext, int sessionid)
        {
            return _appContext.Feature.Where(x => x.SessionId == sessionid).Include(x => x.FeatureUser).
                Select(g => new Models.Feature()
                {
                    CreationDate = g.CreationDate,
                    Description = g.Description,
                    FeatureUser = g.FeatureUser,
                    Id = g.Id,
                    Identification = g.Identification,
                    PlanningSession = g.PlanningSession,
                    SessionId = g.SessionId,
                    Status = g.Status,
                    VotesTotal = (_appContext.PlanningSessionUser.
                                      Where(x => x.PlanningSessionId == g.PlanningSession.Id).Count()),

                    VotesComplete = (_appContext.PlanningSessionUser.
                                      Where(x => x.PlanningSessionId == g.PlanningSession.Id).Where(x => ((_appContext.FeatureUser.Where(a => a.FeatureId == g.Id).Where(x => x.SelectedValue > 0).Select(l => l.UserId)).Contains(x.UserId))).Count())

                                      //Select(k => new UsersVoting()
                                      //{
                                      //    Status = (_appContext.FeatureUser.Where(a => a.FeatureId == g.Id && a.UserId == k.UserId).Sum(p => p.SelectedValue) > 0),
                                      //}).Where(a => a.Status).Count())
        }).ToList();
        }

        public List<UsersVoting> GetUsersVoting(ApplicationContext _appContext, int sessionid,int featureid)
        {
            var usersVoting = _appContext.PlanningSessionUser.
                                Where(x => x.PlanningSessionId == sessionid).
                                Select(g => new UsersVoting()
                                {
                                    Status = (_appContext.FeatureUser.Where(a => a.FeatureId == featureid && a.UserId == g.UserId).Sum(p => p.SelectedValue) > 0),
                                    UserName = _appContext.Users.Where(p => p.Id == g.UserId).FirstOrDefault().UserName,
                                    Vote = (_appContext.FeatureUser.Where(a => a.FeatureId == featureid && a.UserId == g.UserId).FirstOrDefault().SelectedValue)
                                }).ToList();


            bool FullVotedSession = !usersVoting.Where(x => !x.Status).Any();

            usersVoting.ForEach(x => { x.FullVoted = FullVotedSession; });

            return usersVoting.OrderByDescending(x => x.Vote).ToList();

        }
   }
}
