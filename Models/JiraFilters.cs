using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.Models
{
    public class AvatarUrls
    {
        [JsonProperty("48x48")]
        public string _48x48 { get; set; }

        [JsonProperty("24x24")]
        public string _24x24 { get; set; }

        [JsonProperty("16x16")]
        public string _16x16 { get; set; }

        [JsonProperty("32x32")]
        public string _32x32 { get; set; }
    }

    public class Owner
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("avatarUrls")]
        public AvatarUrls AvatarUrls { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }

    public class Group
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("self")]
        public string Self { get; set; }
    }

    public class Roles
    {
    }

    public class ProjectCategory
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class Properties
    {
    }

    public class Project
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("assigneeType")]
        public string AssigneeType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("roles")]
        public Roles Roles { get; set; }

        [JsonProperty("avatarUrls")]
        public AvatarUrls AvatarUrls { get; set; }

        [JsonProperty("projectCategory")]
        public ProjectCategory ProjectCategory { get; set; }

        [JsonProperty("projectTypeKey")]
        public string ProjectTypeKey { get; set; }

        [JsonProperty("simplified")]
        public bool Simplified { get; set; }

        [JsonProperty("style")]
        public string Style { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }
    }

    public class SharePermission
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("group")]
        public Group Group { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }
    }

    public class SharedUsers
    {
        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("items")]
        public List<object> Items { get; set; }

        [JsonProperty("max-results")]
        public int MaxResults { get; set; }

        [JsonProperty("start-index")]
        public int StartIndex { get; set; }

        [JsonProperty("end-index")]
        public int EndIndex { get; set; }
    }

    public class Subscriptions
    {
        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("items")]
        public List<object> Items { get; set; }

        [JsonProperty("max-results")]
        public int MaxResults { get; set; }

        [JsonProperty("start-index")]
        public int StartIndex { get; set; }

        [JsonProperty("end-index")]
        public int EndIndex { get; set; }
    }

    public class JiraFilter
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("jql")]
        public string Jql { get; set; }

        [JsonProperty("viewUrl")]
        public string ViewUrl { get; set; }

        [JsonProperty("searchUrl")]
        public string SearchUrl { get; set; }

        [JsonProperty("favourite")]
        public bool Favourite { get; set; }

        [JsonProperty("favouritedCount")]
        public int FavouritedCount { get; set; }

        [JsonProperty("sharePermissions")]
        public List<SharePermission> SharePermissions { get; set; }

        [JsonProperty("sharedUsers")]
        public SharedUsers SharedUsers { get; set; }

        [JsonProperty("subscriptions")]
        public Subscriptions Subscriptions { get; set; }
    }

}
