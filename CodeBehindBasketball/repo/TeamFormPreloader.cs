using CodeBehindBasketball.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeBehindBasketball.repo;

public class TeamFormPreloader : ITeamPreloader
{
    private static readonly string PATH = "../../../../exibitions.json";
    

    public void Load(Dictionary<string, Team> teams)
    {
        Dictionary<string, List<FixtureDTO>> fixtures = new Dictionary<string, List<FixtureDTO>>();
        try
        {
            string jsonString = File.ReadAllText(PATH);
            fixtures = JsonSerializer.Deserialize<Dictionary<string, List<FixtureDTO>>>(jsonString);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        foreach (string teamCode in teams.Keys){
            Team team = teams[teamCode];

            foreach (FixtureDTO fixture in fixtures[teamCode])
            {
                try
                {
                    team.Form.Add(FixtureDTOToGame(teamCode, fixture, teams));
                }
                catch (Exception ex)
                {
                    //We ignore fixtures against teams that don't otherwise show up in the dataset, because we have know way to calculate the strength of that fixture (no fiba ranking or form for the team)
                    //In this case that was POR (Probably portugal)
                }
            }
        }
    }
    private Game FixtureDTOToGame(string team1, FixtureDTO fixtureDTO, Dictionary<string, Team> teams) {
        Game game = new Game();
        game.Team1 = teams[team1];
        game.Team2 = teams[fixtureDTO.Opponent];

        string[] scores = fixtureDTO.Result.Split('-');
        game.Team1Score = int.Parse(scores[0]);
        game.Team2Score = int.Parse(scores[1]);

        return game;
    }
}
