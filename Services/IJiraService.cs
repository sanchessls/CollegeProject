
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScrumPokerPlanning.APIViewModel;
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
        ObjJiraFeature GetJiraFeature(string Identificator, string website, string email, string key);
    }

    public class ObjJiraFeature
    {
        public string Identificator { get; set; }
        public string Subject { get; set; }
        public bool Success { get; set; }
        public string Exception { get; set; }
        public string MessageToUi { get; set; }
    }

    public class JiraService : IJiraService
    {
        public JiraService()
        {
             
        }
        public ObjJiraFeature GetJiraFeature(string Identificator, string website, string email, string key)
        {
            ObjJiraFeature objReturn = new ObjJiraFeature();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    //show this exemple on the UI 
                    //https://YourWebSite.atlassian.net
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://cloudsphere.atlassian.net/rest/api/2/search?jql=key=IV-20555"))

                    //using (var request = new HttpRequestMessage(new HttpMethod("GET"), website + "/rest/api/2/search?jql=key=" + Identificator))
                    {
                        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("andre.sanches@cloudsphere.com:cd4xRZrocjwJK7bpDlAR9E4A"));
                        //var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(email +":"+key));
                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                        HttpResponseMessage response = httpClient.SendAsync(request).Result;


                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var stringJson = response.Content.ReadAsStringAsync().Result;

                            Root ObjFeature = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(stringJson);

                            if (ObjFeature.total == 0)
                            {
                                objReturn.MessageToUi = "No issue was found with this Identificator.";
                                objReturn.Identificator = "";
                                objReturn.Subject = "";
                                objReturn.Success = false;
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
                return (new ObjJiraFeature() { Success = false, Exception = e.Message, MessageToUi = "Failed To Retrieve from JIRA." });
            }
        }
    }

    public class Fields
    {
        public string summary { get; set; }

    }

    public class Issue
    {
        public string expand { get; set; }
        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public Fields fields { get; set; }

    }

    public class Root
    {
        public string expand { get; set; }
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public List<Issue> issues { get; set; }
    }


}
