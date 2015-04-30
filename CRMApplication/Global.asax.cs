using CRMApplication.Repositories;
using log4net;
using StructureMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CRMApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger("Test");

        protected void Application_Start()
        {
            InitializeProfiler();
            InitializeLogger();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitializeAppData();
        }

        private void InitializeProfiler()
        {
            
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            log.Debug("**************************");
            log.Error("Exception - \n" + ex);
            log.Debug("**************************");
        }

        private void InitializeLogger()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
        }

        private void InitializeAppData()
        {
            ObjectFactory.Initialize(x => x.For<IDataRepository>().Singleton().Use<DataRepository>());
            //var container = new Container(x => x.For<IDataRepository>().Singleton().Use<DataRepository>());
            //For<IDataRepository>().Singleton().Use<DataRepository>();
        }

        
    }
}
