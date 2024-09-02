using CodeBehindBasketball.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.service;

public interface IGroupRankingStrategy
{
    public List<Team> RankGroupTeams(List<Team> teams) {
        return new List<Team>();   
    }
}
