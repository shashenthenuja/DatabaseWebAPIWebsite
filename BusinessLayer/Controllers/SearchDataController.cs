using BusinessLayer.Models;
using DataAccessLayer.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BusinessLayer.Controllers
{
    public class SearchDataController : ApiController
    {
        BankData result = new BankData();
        Access access = new Access();
        public IHttpActionResult SearchData(string name)
        {

            RestRequest request = new RestRequest("api/bankdata/", Method.Get);
            RestResponse response = access.restClient.Execute(request);
            List<BankData> list = JsonConvert.DeserializeObject<List<BankData>>(response.Content);
            foreach (BankData item in list)
            {
                if (item.LastName.ToLower().Equals(name.ToLower()))
                {
                    result = item;
                    break;
                }
            }
            if (result != null)
            {
                return Json(result);
            }
            return BadRequest();
        }
    }
}
