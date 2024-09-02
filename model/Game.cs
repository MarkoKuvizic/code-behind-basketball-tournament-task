using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.model;

public class Game
{
    public Team Team1;
    public Team Team2;
    public Team Winner;
    public Team Loser
    {
        get
        {
            return Team1 != Winner ? Team1 : Team2;
        }
    }
    private double _odds;
    public bool IsCompetitive = false;
    public double Team1Odds
    {
        get
        {
            return _odds;
        }
        set
        {
            _odds = value;
        }
    }
    public double Team2Odds
    {
        get
        {
            return 1 - _odds;
        }
    }
    public int Team1Score;
    public int Team2Score;

    public Game(Team team1, Team team2) {
        this.Team1 = team1;
        this.Team2 = team2;
    }
    public Game()
    {
    }
    public void DetermineWinner()
    {
        // Determine the winner based on the generated scores
        if (Team1Score > Team2Score)
        {
            Winner = Team1;
        }
        else if (Team2Score > Team1Score)
        {
            Winner = Team2;
        }

        Team1.ProcessResult(this);
        Team2.ProcessResult(this);

        Team1.Form.Add(this);
        Team2.Form.Add(this);
    }
    public Team GetOtherTeam(Team team)
    {
        return team == Team1 ? Team2 : Team1;
    }

    public bool DidTeamWin(Team team)
    {
        return (team == Team1 && Team1Score > Team2Score) || (team == Team2 && Team2Score > Team1Score);
    }

    public int GetScoreDifference(Team team)
    {
        return team == Team1 ? Team1Score - Team2Score : Team2Score - Team1Score;
    }
}
