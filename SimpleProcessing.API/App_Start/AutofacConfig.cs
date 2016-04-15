using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using AutoMapper;
using SimpleProcessing.Core.AuthService;
using SimpleProcessing.Core.StorageService;
using SimpleProcessing.Core.ProcessingService;
using System.Web.Mvc;

namespace SimpleProcessing.API
{
	public static class AutofacConfig
	{
		public static void ConfigureDependencyResolver()
		{
			var builder = new ContainerBuilder();
			AutofacConfig.RegisterPlatformFeatures(builder);

			builder
				.RegisterType<AuthManager>()
				.As<IAuthManager>()
				.InstancePerRequest();

			builder
				.RegisterType<CardStorage>()
				.As<ICardStorage>();

			builder
				.RegisterType<OrderStorage>()
				.As<IOrderStorage>();

			builder
				.RegisterType<CardManager>()
				.As<ICardManager>();

			builder
				.RegisterType<ProcessingManager>()
				.As<IProcessingManager>();

			var container = builder.Build();
			// MVC Dependency resolver
			var mvcResolver = new AutofacDependencyResolver(container);
			DependencyResolver.SetResolver(mvcResolver);
			// WebAPI dependency resolver
			var webApiResolver = new AutofacWebApiDependencyResolver(container);
			GlobalConfiguration.Configuration.DependencyResolver = webApiResolver;
		}

		static void RegisterPlatformFeatures(ContainerBuilder builder)
		{
			var config = GlobalConfiguration.Configuration;
			// MVC
			//builder.RegisterControllers(Assembly.GetExecutingAssembly());
			//builder.RegisterFilterProvider();
			builder.RegisterModule<AutofacWebTypesModule>();    // Register web abstractions like HttpContextBase.
			// Web API
			builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
			builder.RegisterWebApiFilterProvider(config);
		}
	}
}