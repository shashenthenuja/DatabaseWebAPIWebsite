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
    public class GetBankDataController : ApiController
    {
        Access access = new Access();
        [ResponseType(typeof(BankData))]
        public IHttpActionResult GetBankData(int id)
        {
            RestRequest request = new RestRequest("api/bankdata/{id}", Method.Get);
            request.AddParameter("id", id);
            RestResponse response = access.restClient.Execute(request);
            if (response.Content != null)
            {
                return Ok(response.Content);
            }
            return BadRequest();
        }
    }
}
