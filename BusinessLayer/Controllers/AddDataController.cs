using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestSharp;
using Newtonsoft.Json;
using BusinessLayer.Models;

namespace BusinessLayer.Controllers
{
    public class AddDataController : ApiController
    {
        Access access = new Access();
        public IHttpActionResult AddData([FromBody]BankData bankData)
        {
            RestRequest restRequest = new RestRequest("api/bankdata", Method.Post);
            restRequest.AddJsonBody(JsonConvert.SerializeObject(bankData));
            RestResponse restResponse = access.restClient.Execute(restRequest);
            BankData result = JsonConvert.DeserializeObject<BankData>(restResponse.Content);
            if (result != null)
            {
                return Json(result);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
