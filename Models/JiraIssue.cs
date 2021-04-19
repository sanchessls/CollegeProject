using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.Models
{
    public class JiraIssueReturn
    {
        public string Identificator { get; set; }
        public string Subject { get; set; }
        public bool Success { get; set; }
        public string Exception { get; set; }
        public string MessageToUi { get; set; }
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

    public class JiraIssue
    {
        public string expand { get; set; }
        public int startAt { get; set; }
        public int maxResults { get; set; }
        public int total { get; set; }
        public List<Issue> issues { get; set; }
    }



}
