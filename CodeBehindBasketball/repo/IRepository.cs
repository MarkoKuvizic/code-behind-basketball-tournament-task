using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeBehindBasketball.repo;

public interface IRepository<T>
{
    private static string PATH;
    public void load();
}
