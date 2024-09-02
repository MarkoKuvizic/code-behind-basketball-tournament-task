using CodeBehindBasketball.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.service;

internal class FibaRankingFormOddsStrategy : IOddsStrategy
{
    // Strategy for determining odds to win that incorporates both team form and fiba ranking
    private const double baseWinWeight = 10.0; 
    private const double rankingFormWeight = 0.5;
    private const double formStrengthWeight = 0.3;
    private const double scoreDifferenceWeight = 0.2;

    private const double formStrengthWeigth = 0.3;
    private const double fibaRankingWeigth = 1 - formStrengthWeigth;    // Weights should add up to 1
    private const double maxFibaRanking = 50;

    public void DetermineOdds(Game game)
    {
        double team1FormStrength = GetFormStrength(game.Team1);
        double team2FormStrength = GetFormStrength(game.Team2);

        double team1FibaScore = (double)(maxFibaRanking - game.Team1.FIBARanking) / maxFibaRanking;
        double team2FibaScore = (double)(maxFibaRanking - game.Team2.FIBARanking) / maxFibaRanking;


        double formStrengthComponent = 0.3 * (team1FormStrength / (team1FormStrength + team2FormStrength));
        double fibaRankingComponent = 0.7 * (team1FibaScore / (team1FibaScore + team2FibaScore));

        // Combine the components to get the win probability
        game.Team1Odds = formStrengthComponent + fibaRankingComponent;
    }
    private double GetFormStrength(Team team)
    {
        double formStrength = 0;

        foreach (Game game in team.Form)
        {
            Team opponent = game.GetOtherTeam(team);
            double opponentFormStrength = opponent.FormStrength != null ? opponent.FormStrength : GetFormStrength(opponent);    // If the opposing team has a predetermined form strength, use it, otherwise calculate recursively
            int opponentRanking = opponent.FIBARanking;
            bool didWin = game.DidTeamWin(team);
            int scoreDifference = game.GetScoreDifference(team);

            double gameStrength = baseWinWeight * (didWin ? 1 : -1); // Positive for a win, negative for a loss
            gameStrength += rankingFormWeight * (200 - opponentRanking); // Higher score for defeating a higher-ranked team
            gameStrength += formStrengthWeight * opponentFormStrength; // Factor in the opponent's form strength
            gameStrength += scoreDifferenceWeight * scoreDifference; // Higher score for a larger margin of victory

            formStrength += gameStrength;
        }
        team.FormStrength = formStrength;
        return formStrength;
    }
}
