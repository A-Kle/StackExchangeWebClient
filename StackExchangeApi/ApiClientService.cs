using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using StackExchangeApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Json;
using System.Web;

namespace StackExchangeApi
{
    public class ApiClientService : IApiClient
    {
        private readonly string _key;
        private readonly string _apiUrl;
        private readonly string _tagsShortUrl;

        private readonly IConfigurationBuilder _configuration;
        private readonly HttpClient _httpClient;

        public ApiClientService()
        {
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            var configuration = _configuration.Build();

            _key = configuration["StackExchange:access_key"];
            _apiUrl = configuration["StackExchange:api_url"];
            _tagsShortUrl = configuration["StackExchange:tags_shorturl"];

            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            _httpClient = new HttpClient(handler);
        }

        private string CreateExtendedUri(string uri, params string[] queryParameters)
        {
            string extendedUri = uri;
            if (queryParameters != null)
            {
                extendedUri = extendedUri + "?";
                foreach (var parm in queryParameters)
                {
                    if (parm == null) continue;
                    extendedUri = extendedUri + "&" + parm;
                }
                extendedUri = extendedUri.Replace("?&", "?");
            }
            return extendedUri;
        }

        private async Task<string> Request(string uri)
        {
            var response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public List<TagResponseModel> GetTags(TagRequestModel requestParm)
        {
            List<TagResponseModel> tags = new List<TagResponseModel>();

            string baseUri = _apiUrl + _tagsShortUrl;

            string key = "key=" +_key;
            string page = requestParm.Page == null ? null : "page=" + requestParm.Page.ToString();
            string pagesize = requestParm.PageSize == null ? null : (requestParm.PageSize > 100 ? "pagesize=100" : "pagesize=" + requestParm.PageSize.ToString());
            string fromdate = requestParm.FromDate == null ? null : "fromdate=" + string.Format("yyyy-MM-dd", requestParm.FromDate);
            string todate = requestParm.ToDate == null ? null : "todate=" + string.Format("yyyy-MM-dd", requestParm.ToDate);
            string order = requestParm.Order == null ? null : "order=" + requestParm.Order.ToString();
            string min = requestParm.Min == null ? null : "min=" + requestParm.Min.ToString();
            string max = requestParm.Max == null ? null : "max=" + requestParm.Max.ToString();
            string sort = requestParm.Sort == null ? null : "sort=" + requestParm.Sort.ToString();
            string inname = string.IsNullOrEmpty(requestParm.Inname) ? null : "inname=" + requestParm.Inname;
            string site = "site=stackoverflow";

            if (requestParm.PageSize >= 100)
            {
                for(int i = 1; i <= requestParm.PageSize/100; i++) //max limit for request is 100 items
                {
                    string longurl = CreateExtendedUri(baseUri.ToString(), key, page, pagesize, fromdate, todate, order, min, max, sort, inname, site);
                    var response = Request(longurl).Result;

                    JObject ro = JObject.Parse(response);
                    JArray items = (JArray)ro["items"];

                    tags.AddRange(items.ToObject<List<TagResponseModel>>());

                    JToken lastItem = items.Last;
                    int lastItemCount = (int)lastItem["count"];
                    max = "max=" + (lastItemCount - 1).ToString(); //creating new value for next request
                }
            }
            else
            {
                string longurl = CreateExtendedUri(baseUri.ToString(), key, page, pagesize, fromdate, todate, order, min, max, sort, inname, site);
                var response = Request(longurl).Result;

                JObject rss = JObject.Parse(response);
                JArray items = (JArray)rss["items"];

                tags.AddRange(items.ToObject<List<TagResponseModel>>());
            }

            return tags;
        }
    }
}
