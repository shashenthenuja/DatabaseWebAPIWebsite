using BusinessLayer.Models;
using DataAccessLayer.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace BusinessLayer.Controllers
{
    public class GetAllDataController : ApiController
    {

        Access access = new Access();
        [ResponseType(typeof(BankData))]
        public IHttpActionResult GetAllData()
        {
            RestRequest request = new RestRequest("api/bankdata/", Method.Get);
            RestResponse response = access.restClient.Execute(request);
            List<BankData> list = JsonConvert.DeserializeObject<List<BankData>>(response.Content);
            if (response.Content != null)
            {
                return Ok(list);
            }
            return BadRequest();
        }
    }
}
