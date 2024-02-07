using Newtonsoft.Json;

namespace GitHubToCalDav.Models;

public class Issue
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("node_id")]
    public string NodeId { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("repository_url")]
    public string RepositoryUrl { get; set; }

    [JsonProperty("labels_url")]
    public string LabelsUrl { get; set; }

    [JsonProperty("comments_url")]
    public string CommentsUrl { get; set; }

    [JsonProperty("events_url")]
    public string EventsUrl { get; set; }

    [JsonProperty("html_url")]
    public string HtmlUrl { get; set; }

    [JsonProperty("number")]
    public int Number { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }

    [JsonProperty("milestone")]
    public Milestone Milestone { get; set; }

    [JsonProperty("locked")]
    public bool Locked { get; set; }

    [JsonProperty("active_lock_reason")]
    public string ActiveLockReason { get; set; }

    [JsonProperty("comments")]
    public int Comments { get; set; }

    [JsonProperty("closed_at")]
    public object ClosedAt { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonProperty("author_association")]
    public string AuthorAssociation { get; set; }
}
