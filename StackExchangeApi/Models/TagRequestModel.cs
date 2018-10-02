using System;
using System.Collections.Generic;
using System.Text;

namespace StackExchangeApi.Models
{
    public enum Order
    {
         asc, desc
    }

    public enum Sort
    {
        popular, activity, name
    }

    public class TagRequestModel : RequestModel
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public Order? Order { get; set; } = null;
        public Sort? Sort { get; set; } = null;
        public int? Min { get; set; } = null;
        public int? Max { get; set; } = null;
        public string Inname { get; set; } = null;

    }
}
