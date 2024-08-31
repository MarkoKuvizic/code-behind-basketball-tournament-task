using CodeBehindBasketball.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.service;

public class WeighedScoreGameSimulationStrategy : IGameSimulationStrategy
{
    public void SimulateGame(Game game)
    {

        Random random = new Random();

        // Generate base scores
        int team1BaseScore = random.Next(80, 110); // Random base score for Team 1
        int team2BaseScore = random.Next(80, 110); // Random base score for Team 2

        // Adjust scores based on win probability
        game.Team1Score = (int)(team1BaseScore + game.Team1Odds * 10 - random.NextDouble() * 10);
        game.Team2Score = (int)(team2BaseScore + game.Team2Odds * 10 - random.NextDouble() * 10);

        game.DetermineWinner();
    }
}

