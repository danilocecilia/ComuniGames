using ComuniGamesWeb.Infrastructure.Interfaces;
using ComuniGamesWeb.Models;
using ComuniGamesWeb.Models.DataContext;
using ComuniGamesWeb.Models.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ComuniGamesWeb.Infrastructure
{
    public class NinjectDependencyResolver : DefaultControllerFactory
    {
        private IKernel kernel;

        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)kernel.Get(controllerType);
        }

        private void AddBindings()
        {
            kernel.Bind<IRepository<UserProfile>>().To<Repository<UserProfile>>();
            kernel.Bind<IRepository<Usuario>>().To<Repository<Usuario>>();
            kernel.Bind<DbContext>().To<ComuniGamesEntities>();
            kernel.Bind<IRepository<Endereco>>().To<Repository<Endereco>>();
            kernel.Bind<IRepository<Cidade>>().To<Repository<Cidade>>();
            kernel.Bind<IRepository<Estado>>().To<Repository<Estado>>();
        }
    }
}