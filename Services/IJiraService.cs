
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScrumPokerPlanning.APIViewModel;
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
    public interface IJiraService
    {
        JiraIssueReturn GetJiraFeature(string Identificator, string website, string email, string key);
        List<JiraFilter> GetJiraFilter(string website, string email, string key,bool favourite);
        JiraIssue GetJiraIssuesFromFilter(string jiraWebSite, string jiraEmail, string jiraKey, int filterID);
    }

    public class JiraService : IJiraService
    {
        public JiraService()
        {
             
        }

        public List<JiraFilter> GetJiraFilter(string website, string email, string key, bool favourite)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    string web = "";

                    if (favourite)
                    {
                        web = "/rest/api/2/filter/favourite";
                    }
                    else
                    {
                        web = "/rest/api/2/filter";
                    }

                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), website + web))
                    {
                        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(email + ":" + key));
                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                        HttpResponseMessage response = httpClient.SendAsync(request).Result;


                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var stringJson = response.Content.ReadAsStringAsync().Result;

                            var ObjFilter = Newtonsoft.Json.JsonConvert.DeserializeObject<JiraFilter[]>(stringJson).ToList();
                           
                            return ObjFilter.OrderBy(x => x.Id).ToList();
                        }

                        return null;
                    }
                }



            }
            catch (Exception e)
            {
                return null;
            }
        }
    

        public JiraIssueReturn GetJiraFeature(string Identificator, string website, string email, string key)
        {
            JiraIssueReturn objReturn = new JiraIssueReturn();

            try
            {
                using (var httpClient = new HttpClient())
                {

                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), website + "/rest/api/2/search?jql=key=" + Identificator))
                    {
                        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(email + ":" + key));
                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                        HttpResponseMessage response = httpClient.SendAsync(request).Result;


                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var stringJson = response.Content.ReadAsStringAsync().Result;

                            JiraIssue ObjFeature = Newtonsoft.Json.JsonConvert.DeserializeObject<JiraIssue>(stringJson);

                            if (ObjFeature.total == 0)
                            {
                                objReturn.MessageToUi = "No issue was found with this Identificator.";
                                objReturn.Identificator = "";
                                objReturn.Subject = "";
                                objReturn.Success = false;
                                return objReturn;
                            }

                            objReturn.Identificator = ObjFeature.issues.First().key;
                            objReturn.Subject = ObjFeature.issues.First().fields.summary ;
                            objReturn.Success = true;

                            return objReturn;
                        }
                        else
                        {
                            //update this accordly to the results
                            objReturn.MessageToUi = "Failed To Retrieve from JIRA.";
                            objReturn.Identificator = "";
                            objReturn.Subject = "";
                            objReturn.Success = false;
                        }
                        return objReturn;


                    }
                }



            }
            catch (Exception e)
            {
                return (new JiraIssueReturn() { Success = false, Exception = e.Message, MessageToUi = "Failed To Retrieve from JIRA." });
            }
        }

        public JiraIssue GetJiraIssuesFromFilter(string website, string email, string key, int filterID)
        {            
            try
            {
                using (var httpClient = new HttpClient())
                {

                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), website + "/rest/api/2/search?jql=Filter=" + filterID))
                    {
                        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(email + ":" + key));
                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                        HttpResponseMessage response = httpClient.SendAsync(request).Result;


                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var stringJson = response.Content.ReadAsStringAsync().Result;

                            JiraIssue ObjFeature = Newtonsoft.Json.JsonConvert.DeserializeObject<JiraIssue>(stringJson);

                            if (ObjFeature != null)
                            {                         
                                return ObjFeature;
                            }

                            return null;
                        }
                        else
                        {
                            //update this accordly to the results
                       
                        }
                        return null;


                    }
                }



            }
            catch (Exception e)
            {
                return null;
            }
        }
    }

 

}
