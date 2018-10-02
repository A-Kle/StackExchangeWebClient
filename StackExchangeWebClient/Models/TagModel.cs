using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchangeWebClient.Models
{
    public class TagModel
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public int Popularity { get; set; }
    }
}
