using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test_app.Models;

using System.Net.Http;

using System.Runtime.Serialization.Json;
using System.Security.Principal;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace test_app.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        public HttpClient client = new HttpClient();
        public DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Repository>));
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        } 

        public IActionResult TestRequest()
        {
            
            var uri = "https://prod-08.centralus.logic.azure.com:443/workflows/eb7971b7f2024877af7a662f6493b5aa/triggers/request/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Frequest%2Frun&sv=1.0&sig=03DBaKvnYJdz7NhbOyjmFfOqn-VbqBuhRTS4DqFvhgY";

            WebApiClient.RequestLogicApp(client, serializer, uri, User);
            var repositories = WebApiClient.ProcessRepositories(client, serializer).Result;
            ViewData["Message"] = "Some Json";
            ViewData["repositories"] = repositories;
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
