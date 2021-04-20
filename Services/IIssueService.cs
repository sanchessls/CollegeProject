
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScrumPokerPlanning.APIViewModel;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.Services
{
    public interface IIssueService
    {
        Feature CreateFeatureAsync(int PlanningSessionId, string subject, string identificator, bool jiraCreated, string link, string userId);
        int GetSessionIdByCode(string sessionCode);
    }

    public class IssueService : IIssueService
    {
        protected readonly ApplicationContext _appContext;
        public IssueService(ApplicationContext appContext)
        {
            _appContext = appContext;
        }

        public Feature CreateFeatureAsync(int PlanningSessionId,string subject,string identificator,bool jiraCreated, string link, string userId)
        {
            //the ones  who call this should treat the result in case of exception
            Models.Feature feature = new Models.Feature
            {
                SessionId = PlanningSessionId,
                Status = EnumFeature.Open,
                CreationDate = DateTime.Now,
                Description = subject,
                Identification = identificator,
                JiraCreated = jiraCreated,
                Link = link
            };

            _appContext.Feature.Add(feature);

            _appContext.SaveChangesAsync().Wait();

            AddUsersInFeatureAsync(PlanningSessionId, userId).Wait();

            return feature;
        }

        public int GetSessionIdByCode(string sessionCode)
        {
            return _appContext.PlanningSession.Where(x => x.SessionCode.ToUpper() == sessionCode.ToUpper()).FirstOrDefault().Id;
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
    }

 

}
