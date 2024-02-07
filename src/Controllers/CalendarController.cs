using System.IO.Compression;
using System.Net.Http.Headers;
using GitHubToCalDav.Models;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GitHubToCalDav.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarController : ControllerBase
{
    [HttpGet("{repoOwner}/{repo}")]
    public string GetCalendar([FromRoute] string repoOwner, [FromRoute] string repo, string token, bool includeIssues = false)
    {
        var milestones = FetchMilestonesFromRepo(repoOwner, repo, token);
        var calendar = new Calendar();

        List<Issue> issues = new();

        if (includeIssues)
        {
            issues = GetIssues(token).Where(x => x.Milestone != null).ToList();
        }

        foreach (var currentMilestone in milestones)
        {
            if (currentMilestone.DueOn == null) continue;

            calendar.Events.Add(new()
            {
                DtStamp = new CalDateTime((DateTime)currentMilestone.DueOn),
                DtStart = new CalDateTime((DateTime)currentMilestone.DueOn),
                DtEnd = new CalDateTime((DateTime)currentMilestone.DueOn),
                IsAllDay = true,
                Summary = currentMilestone.Title,
                Description = currentMilestone.Description
            });

            if(issues.Count == 0) continue;
            foreach (var currentIssue in issues.Where(x => x.Milestone.Id == currentMilestone.Id))
            {
                calendar.Events.Add(new()
                {
                    DtStamp = new CalDateTime((DateTime)currentMilestone.DueOn),
                    DtStart = new CalDateTime((DateTime)currentMilestone.DueOn),
                    DtEnd = new CalDateTime((DateTime)currentMilestone.DueOn),
                    IsAllDay = true,
                    Summary = currentIssue.Title,
                    Description = currentIssue.HtmlUrl
                });
            }
        }

        var serializer = new CalendarSerializer();
        return serializer.SerializeToString(calendar);
    }

    private List<Issue> GetIssues(string token)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubToCalDAV");
        var response = httpClient.GetAsync($"https://api.github.com/issues?state=all").Result;
        response.EnsureSuccessStatusCode();
        var responseContentAsString = response.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<List<Issue>>(responseContentAsString);
    }

    private List<Milestone> FetchMilestonesFromRepo(string repoOwner, string repo, string token)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubToCalDAV");
        var response = httpClient.GetAsync($"https://api.github.com/repos/{repoOwner}/{repo}/milestones?state=all").Result;
        response.EnsureSuccessStatusCode();
        var responseContentAsString = response.Content.ReadAsStringAsync().Result;
        return JsonConvert.DeserializeObject<List<Milestone>>(responseContentAsString);
    }
}
