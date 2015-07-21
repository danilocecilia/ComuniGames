using ComuniGamesWeb.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComuniGamesWeb.Models.Interfaces
{
    public interface IComuniGamesEntity
    {
        ComuniGamesEntities ComuniGamesEntity(); 
    }
}
