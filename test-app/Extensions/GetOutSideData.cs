using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using System.Security.Claims;
using test_app.Models;


namespace GetOutsideData
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GetOutsideData
    {
        private readonly RequestDelegate _next;

        public GetOutsideData(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            ClaimsPrincipal Usr = httpContext.User;
            if (Usr.Identity.IsAuthenticated)
            {
                var uri = "https://dev-test-functions.azurewebsites.net/api/GetHeaders?code=22M0gQaN1ezms2sqXt8LrN3gifdENbsiwKVapk6icLjX8toj6PP1Iw==";
                var client = new HttpClient();
                var str = WebApiClient.RequestLogicApp(client, uri, Usr).Result;
                Console.Write(str);
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GetOutsideDataExtensions
    {
        public static IApplicationBuilder GetOutsideDataTemplate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GetOutsideData>();
        }
    }
}
