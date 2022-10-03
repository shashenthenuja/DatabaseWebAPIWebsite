using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLayer.Models
{
    public class Access
    {
        public RestClient restClient = new RestClient("http://localhost:53746/");
    }
}