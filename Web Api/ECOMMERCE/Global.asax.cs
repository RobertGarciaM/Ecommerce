using ECOMMERCE.Helpers;
using ECOMMERCE.Interfaces;
using ECOMMERCE.Repositories;
using Infrastructure.Repositories;
using Models.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;

namespace ECOMMERCE
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var container = new UnityContainer();
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<ISaleRepository, SaleRepository>();
            var resolver = new UnityResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }
    }
}
