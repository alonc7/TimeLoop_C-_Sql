using Microsoft.Ajax.Utilities;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TimeLoop_Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Enable CORS globally for all controllers and actions
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Define a named route for user registration response
            config.Routes.MapHttpRoute(
                name: "UserRegistrationRoute",
                routeTemplate: "api/users/{id}",
                defaults: new { controller = "Users", id = RouteParameter.Optional }
            );

            // Define a named route for user registration response
            config.Routes.MapHttpRoute(
                name: "TaskAddingRoute",
                routeTemplate: "api/Tasks/addTask/{userId}",
                defaults: new { controller = "Tasks", id = RouteParameter.Optional }
            );
        }
    }
}
