using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComuniGamesWeb.Infrastructure.Interfaces
{
    public interface IDependencyResolver
    {
        object GetService(Type serviceType);
        IEnumerable<object> GetServices(Type serviceType);
    }
}