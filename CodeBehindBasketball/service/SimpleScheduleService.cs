using CodeBehindBasketball.model;
using CodeBehindBasketball.repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.service;

public class SimpleScheduleService : IScheduleService
{
    private IGroupRepository GroupRepository;
    private IOddsStrategy oddsStrategy;
    private IGameSimulationStrategy gameSimulationStrategy;
    private IGroupRankingStrategy groupRankingStrategy;
    public Schedule Schedule;
    public SimpleScheduleService(IGroupRepository groupRepository, Schedule schedule)
    {
        GroupRepository = groupRepository;
        Schedule = schedule;
    }
    public SimpleScheduleService(IGroupRepository groupRepository, IOddsStrategy oddsStrategy, IGameSimulationStrategy gameSimulationStrategy, IGroupRankingStrategy groupRankingStrategy)
    {
        GroupRepository = groupRepository;
        Schedule = new Schedule();

        this.oddsStrategy = oddsStrategy;
        this.gameSimulationStrategy = gameSimulationStrategy;
        this.groupRankingStrategy = groupRankingStrategy;
    }

    public void GenerateGroupStageSchedule() {
        foreach (string groupName in GroupRepository.GetGroupNames())
        {
            Schedule.AddRounds(GetGroupGames(groupName));
        }
    }
    private string GetGroupStageRoundName(int roundNumber, string groupName)
    {
        return $"[Grupa {groupName}]\t {roundNumber}. kolo";
    }
    private List<(string, List<Game>)> GetGroupGames(string groupName)
    {
        List<Team> teams = GroupRepository.GetGroup(groupName);

        List<(string, List<Game>)> rounds = new List<(string, List<Game>)>();
        int numberOfRounds = teams.Count - 1;

        for (int round = 0; round < numberOfRounds; round++)
        {
            List<Game> currentRound = new List<Game>();
            for (int i = 0; i < teams.Count / 2; i++)
            {
                int team1Index = (round + i) % (teams.Count - 1);
                int team2Index = (teams.Count - 1 - i + round) % (teams.Count - 1);

                if (i == 0)
                {
                    team2Index = teams.Count - 1;
                }
                Game game = new Game(teams[team1Index], teams[team2Index]);
                game.IsCompetitive = true;
                currentRound.Add(game);
            }
            rounds.Add((GetGroupStageRoundName(round + 1, groupName),currentRound));
        }

        return rounds;
    }
    public void ExecuteRound() {
        (String roundName, List<Game> games) = Schedule.PopRound();

        games.ForEach(game => oddsStrategy.DetermineOdds(game));
        games.ForEach(game => gameSimulationStrategy.SimulateGame(game));

        Console.WriteLine($"Simulated Round: {roundName}");
    }
    private void GenerateKnockoutStageRound() { }

    public void ExecuteKnockoutStage() {
        
        foreach(string groupName in GroupRepository.GetGroupNames())
        {
            GroupRepository.SetGroup(groupName, groupRankingStrategy.RankGroupTeams(GroupRepository.GetGroup(groupName)));

            Console.WriteLine(groupName);
            foreach (Team team in GroupRepository.GetGroup(groupName))
            {
                Console.Write(team.Points);
                Console.WriteLine(team.ToString());
            }
        }

        List<Team> firstPlacedTeams = new List<Team>();
        List<Team> secondPlacedTeams = new List<Team>();
        List<Team> thirdPlacedTeams = new List<Team>();

        foreach(string groupName in GroupRepository.GetGroupNames())
        {
            List<Team> group = GroupRepository.GetGroup(groupName);

            firstPlacedTeams.Add(group[0]);
            secondPlacedTeams.Add(group[1]);
            thirdPlacedTeams.Add(group[2]);
        }

        firstPlacedTeams = groupRankingStrategy.RankGroupTeams(firstPlacedTeams);
        secondPlacedTeams = groupRankingStrategy.RankGroupTeams(secondPlacedTeams);
        thirdPlacedTeams = groupRankingStrategy.RankGroupTeams(thirdPlacedTeams);

        List<Team> quarterFinalists = new List<Team>();
        quarterFinalists.AddRange(firstPlacedTeams);
        quarterFinalists.AddRange(secondPlacedTeams);
        quarterFinalists.AddRange(thirdPlacedTeams.Take(2));
        PrintHats(quarterFinalists);
        DrawQuarterfinals(quarterFinalists);

        
    }
    private void DrawQuarterfinals(List<Team> teams)
    {
        Random random = new Random();

        // Split teams into their respective hats
        List<Team> hatD = [teams[0], teams[1]];
        List<Team> hatE = [teams[2], teams[3]];
        List<Team> hatF = [teams[4], teams[5]];
        List<Team> hatG = [teams[6], teams[7]];

        // Shuffle the teams within each hat
        ShuffleTeams(hatD, random);
        ShuffleTeams(hatE, random);
        ShuffleTeams(hatF, random);
        ShuffleTeams(hatG, random);

        // Draw the quarterfinals
        var quarterfinal1 = new List<Team> { hatD[0], hatG[0] };
        var quarterfinal2 = new List<Team> { hatD[1], hatG[1] };
        var quarterfinal3 = new List<Team> { hatE[0], hatF[0] };
        var quarterfinal4 = new List<Team> { hatE[1], hatF[1] };

        Console.WriteLine("Cetvrt Finale:");
        PrintMatchup(quarterfinal1);
        PrintMatchup(quarterfinal2);
        PrintMatchup(quarterfinal3);
        PrintMatchup(quarterfinal4);

        Schedule.AddRound("Cervrt finale", [new Game(quarterfinal1[0], quarterfinal1[1]), new Game(quarterfinal2[0], quarterfinal2[1]), new Game(quarterfinal3[0], quarterfinal3[1]), new Game(quarterfinal4[0], quarterfinal4[1])]);

        // Determine the semifinal matchups
        var semifinal1 = new List<List<Team>> { quarterfinal1, quarterfinal4 };
        var semifinal2 = new List<List<Team>> { quarterfinal2, quarterfinal3 };

        Console.WriteLine("\nPolufinale:");
        PrintSemifinalMatchup(semifinal1);
        PrintSemifinalMatchup(semifinal2);

        ExecuteRound();

        (_, List<Game> quarterFinals) = Schedule.GetCurrentRound();
        Schedule.AddRound("Polufinale", [new Game(quarterFinals[0].Winner, quarterFinals[3].Winner), new Game(quarterFinals[1].Winner, quarterFinals[2].Winner)]);

        ExecuteRound();


        (_, List<Game> semiFinals) = Schedule.GetCurrentRound();
        Schedule.AddRound("Finale", [new Game(semiFinals[0].Winner, semiFinals[1].Winner)]);

        Schedule.AddRound("Utakmica za trece mesto", [new Game(semiFinals[0].Loser, semiFinals[1].Loser)]);

        ExecuteRound();
        ExecuteRound();

        (_, List<Game> finals) = Schedule.GetCurrentRound();
        Schedule.PrintKnockoutStage();
    }
    private static void ShuffleTeams(List<Team> teams, Random random)
    {
        for (int i = teams.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            var temp = teams[i];
            teams[i] = teams[j];
            teams[j] = temp;
        }
    }
    private static void PrintMatchup(List<Team> matchup)
    {
        Console.WriteLine($"{matchup[0].ISOCode} vs {matchup[1].ISOCode}");
    }
    private static void PrintSemifinalMatchup(List<List<Team>> semifinal)
    {
        Console.WriteLine($"Pobednik {semifinal[0][0].ISOCode} - {semifinal[0][1].ISOCode} igra protiv pobednika {semifinal[1][0].ISOCode} - {semifinal[1][1].ISOCode}");
    }
    private void PrintHats(List<Team> quarterFinalists)
    {
        Console.WriteLine($"Sesir D: {quarterFinalists[0].ISOCode} i {quarterFinalists[1].ISOCode}");
        Console.WriteLine($"Sesir E: {quarterFinalists[2].ISOCode} i {quarterFinalists[3].ISOCode}");
        Console.WriteLine($"Sesir F: {quarterFinalists[4].ISOCode} i {quarterFinalists[5].ISOCode}");
        Console.WriteLine($"Sesir G: {quarterFinalists[6].ISOCode} i {quarterFinalists[7].ISOCode}");

    }
    public void PrintSchedule()
    {
        Schedule.PrintSchedule();
    }
    public void ExecuteAllRounds()
    {
        while (Schedule.HasRoundsToExecute())
        {
            ExecuteRound();
        }
    }
}
