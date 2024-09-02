using CodeBehindBasketball.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.service;
//Ranking strategy that ranks teams within a group according to points first, head to head records second and basket difference third
public class BasicGroupRankingStrategy : IGroupRankingStrategy
{
    public List<Team> RankGroupTeams(List<Team> teams)
    {
        return teams
            .OrderByDescending(t => t.Points)
            .ThenByDescending(t => GetHeadToHeadPoints(t, teams))
            .ThenByDescending(t => t.BasketDifference)
            .ToList();
    }

    private static int GetHeadToHeadPoints(Team team, List<Team> teams)
    {
        int headToHeadPoints = 0;
        var equalPointsTeams = teams.Where(t => t.Points == team.Points && t.ISOCode != team.ISOCode).ToList();

        foreach (var opponent in equalPointsTeams)
        {
            var game = team.Form.FirstOrDefault(g => (g.Team1 == team && g.Team2 == opponent) || (g.Team2 == team && g.Team1 == opponent));
            if (game != null)
            {
                if (game.Winner == team)
                {
                    headToHeadPoints += 2; // Win
                }
                else
                {
                    headToHeadPoints += 1; // Loss
                }
            }
        }

        return headToHeadPoints;
    }
}
