using CodeBehindBasketball.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.repo;

public class Schedule
{
    public List<(String, List<Game>)> rounds;
    private int currentRound = 0;
    public Schedule()
    {
        rounds = new List<(String, List<Game>)>();
    }
    public void AddRound(String name, List<Game> games) {
        this.rounds.Add((name, games));
    }
    public void AddRounds(List<(string name, List<Game>)> rounds)
    {
        this.rounds.AddRange(rounds);
    }
    public (String, List<Game>) PopRound()
    {
        return rounds[currentRound++];
    }
    public (String, List<Game>) GetCurrentRound()
    {
        return rounds[currentRound - 1];
    }
    public void PrintSchedule()
    {
       foreach ((String name, List<Game> games) in rounds)
       {
            PrintRound(name, games);     
       }
    }
    public void PrintKnockoutStage()
    {
        foreach ((String name, List<Game> games) in rounds.TakeLast(4)){
            PrintRound(name, games);
        }
    }
    private void PrintRound(String name, List<Game> games)
    {
        Console.WriteLine("___________________________________________________");
        Console.WriteLine(name);

        games.ForEach(game =>
        {
            Console.WriteLine($"{game.Team1.ToString()} protiv {game.Team2.ToString()}");
            Console.WriteLine($"{game.Team1Score} {game.Team2Score}");
            Console.WriteLine($"Verovatnoca da ce {game.Team1.ISOCode} pobediti je bila: {game.Team1Odds}");
        });
        Console.WriteLine("___________________________________________________");
    }
    public bool HasRoundsToExecute()
    {
        return currentRound < rounds.Count;
    }
}
