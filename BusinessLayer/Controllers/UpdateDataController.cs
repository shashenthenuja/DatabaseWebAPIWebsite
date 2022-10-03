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

namespace BusinessLayer.Controllers
{
    public class UpdateDataController : ApiController
    {
        Access access = new Access();
        public IHttpActionResult UpdateData([FromUri]int id, [FromBody]BankData bd)
        {
            RestRequest request = new RestRequest("api/bankdata/?id=" + id, Method.Put);
            request.AddBody(JsonConvert.SerializeObject(bd));
            RestResponse response = access.restClient.Execute(request);
            return Ok(response);
        }
    }
}
