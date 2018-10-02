using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackExchangeWebClient.Models
{
    public class HomeViewModel
    {
        public List<TagModel> TagsList { get; set; }
        public float? TotalCount { get; set; }
    }
}
