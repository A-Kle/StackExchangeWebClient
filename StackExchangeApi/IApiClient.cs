using StackExchangeApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StackExchangeApi
{
    public interface IApiClient
    {
        List<TagResponseModel> GetTags(TagRequestModel requestParm);
    }
}
