using CodeBehindBasketball.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeBehindBasketball.repo;

public class GroupJsonRepository : IGroupRepository
{
    private static string PATH;
    private Dictionary<string, List<Team>> items;
    private List<ITeamLoadingDecorator> preloaders;

    public GroupJsonRepository(string path)
    {
        PATH = path;

        preloaders = new List<ITeamLoadingDecorator>();
        preloaders.Add(new TeamFormLoadingDecorator());

        load();
    }
    private void RunPreloaders()
    {
        Dictionary<string, Team> teams = GetPackagedTeams();

        preloaders.ForEach(preloader => preloader.Load(teams));
    }
    private Dictionary<string, Team> GetPackagedTeams()
    {
        Dictionary<string, Team> teams = new Dictionary<string, Team>();

        items.Values.ToList().ForEach(group => group.ForEach(team => teams[team.ISOCode] = team));

        return teams;
    }
    public void load()
    {
        try
        {
            string jsonString = File.ReadAllText(PATH);
            items = JsonSerializer.Deserialize<Dictionary<string, List<Team>>>(jsonString);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        RunPreloaders();
    }

    public List<Team> GetGroup(string groupName)
    {
        return items[groupName];
    }
    public List<string> GetGroupNames()
    {
        return items.Keys.ToList();
    }
    public void SetGroup(string name, List<Team> teams)
    {
        items[name] = teams;
    }
}

