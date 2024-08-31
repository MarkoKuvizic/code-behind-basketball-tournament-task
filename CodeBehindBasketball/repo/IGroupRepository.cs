using CodeBehindBasketball.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.repo;

public interface IGroupRepository : IRepository<Team>
{
    public List<Team> GetGroup(string groupName);
    public List<string> GetGroupNames();
    public void SetGroup(string groupName, List<Team> teams);
}
