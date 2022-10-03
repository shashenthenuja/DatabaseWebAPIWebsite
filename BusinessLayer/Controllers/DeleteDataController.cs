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
    public class DeleteDataController : ApiController
    {
        Access access = new Access();
        public IHttpActionResult DeleteData(int id)
        {
            RestRequest request = new RestRequest("api/bankdata/{id}", Method.Delete);
            request.AddParameter("id", id);
            RestResponse restResponse = access.restClient.Execute(request);
            BankData result = JsonConvert.DeserializeObject<BankData>(restResponse.Content);
            if (result != null)
            {
                return Json(result);
            }
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
