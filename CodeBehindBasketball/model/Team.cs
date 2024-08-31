using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.model;
public class Team
{
    public string Name { get; set; }
    public string ISOCode { get; set; }
    public int FIBARanking { get; set; }
    public double FormStrength { get; set; }
    public int Points { get; set; } = 0;
    public int BasketDifference { get; set; } = 0;
    public int PointsScored { get; set; } = 0;
    public List<Game> Form = new List<Game>();

    public string ToString()
    {
        return $"{ISOCode} (Fiba Ranking: {FIBARanking}) \t P:{Points}/BD:{BasketDifference}/Scored:{PointsScored}";
    }
    public void ProcessResult(Game game)
    {
        int resultMultiplier = -1;
        if (game.Winner == this)
        {
            Points += 2;
            BasketDifference += Math.Abs(game.Team1Score - game.Team2Score);
            PointsScored += Math.Max(game.Team1Score, game.Team2Score);
        }
        else
        {
            Points += 1;
            BasketDifference -= Math.Abs(game.Team1Score - game.Team2Score);
            PointsScored += Math.Min(game.Team1Score, game.Team2Score);
        }
    }
}
