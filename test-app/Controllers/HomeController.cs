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
using Microsoft.AspNetCore.Cors;

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
            var repositories = WebApiClient.ProcessRepositories(client, serializer).Result;
            ViewData["Message"] = "Some Json";
            ViewData["repositories"] = repositories;
            return View();
        }

        [EnableCors("AllowSpecificOrigin")]
        public IActionResult GetOutsideData() {

            //var uri = "https://prod-08.centralus.logic.azure.com:443/workflows/eb7971b7f2024877af7a662f6493b5aa/triggers/request/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Frequest%2Frun&sv=1.0&sig=03DBaKvnYJdz7NhbOyjmFfOqn-VbqBuhRTS4DqFvhgY";
            //var uri = "https://dev-test-functions.azurewebsites.net/api/GetHeaders?code=22M0gQaN1ezms2sqXt8LrN3gifdENbsiwKVapk6icLjX8toj6PP1Iw==";
            //WebApiClient.RequestLogicApp(client, uri, User);

            return RedirectToAction(nameof(HomeController.Index), "TestRequest");
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
