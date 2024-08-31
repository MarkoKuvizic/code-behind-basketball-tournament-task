using CodeBehindBasketball.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.repo;

public interface ITeamPreloader
{
    public void Load(Dictionary<string, Team> teams);
}
