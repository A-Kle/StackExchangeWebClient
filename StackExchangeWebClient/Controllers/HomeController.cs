using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchangeApi;
using StackExchangeApi.Models;
using StackExchangeWebClient.Models;

namespace StackExchangeWebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiClient _apiClient;

        public HomeController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [Route("/Home")]
        public IActionResult Index()
        {
            var tagsList = new List<TagModel>();
            float? total = null;
            try
            {
                var tags = _apiClient.GetTags(new TagRequestModel { PageSize = 1000, Sort = Sort.popular, Order = Order.desc });
                int id = 1;

                foreach (var item in tags)
                {
                    tagsList.Add(new TagModel { Id = id, TagName = item.Name, Popularity = item.Count });
                    id++;
                }

                total = tagsList.Sum(item => item.Popularity);
            }
            catch
            {
                ViewData["Error"] = "Could not read data source.";
            }

            var model = new HomeViewModel
            {
                TagsList = tagsList,
                TotalCount = total
            };

            return View(model);
        }

        [Route("/Contact")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
