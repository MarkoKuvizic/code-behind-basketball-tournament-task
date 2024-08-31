using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.service;

public interface IScheduleService
{
    public void GenerateGroupStageSchedule() { }

    private void GenerateKnockoutStageRound() { }

    public void ExecuteKnockoutStage() { }

    public void ExecuteRound() { }

    void PrintSchedule();
    public void ExecuteAllRounds() { }
}
