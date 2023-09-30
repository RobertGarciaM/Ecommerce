using Microsoft.Owin.Security.OAuth;
using Swashbuckle.Application;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ECOMMERCE
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));


            GlobalConfiguration.Configuration
               .EnableSwagger(c =>
               {
                   c.SingleApiVersion("v1", "Ecommerce");
                   c.IncludeXmlComments(GetXmlCommentsPath());
                   c.ApiKey("Authorization")
                     .Description("JWT.")
                     .Name("Bearer")
                     .In("header");
               })
               .EnableSwaggerUi(c =>
               {
                   c.DocumentTitle("Ecommerce");

               });

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                 name: "Swagger",
                 routeTemplate: "",
                 defaults: null,
                 constraints: null,
                 handler: new RedirectHandler((message => message.RequestUri.ToString()), "swagger")
             );
        }

        private static string GetXmlCommentsPath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory + @"\bin\ECOMMERCE.XML";
        }
    }
}
