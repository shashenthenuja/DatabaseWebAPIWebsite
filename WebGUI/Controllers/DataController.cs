using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace WebGUI.Controllers
{
    public class DataController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Search(int id)
        {
            RestClient restClient = new RestClient("http://localhost:53746/");
            RestRequest restRequest = new RestRequest("api/bankdata/{id}", Method.Get);
            restRequest.AddUrlSegment("id", id);
            RestResponse restResponse = restClient.Execute(restRequest);
            return Ok(restResponse.Content);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] BankData bankData)
        {
            RestClient restClient = new RestClient("http://localhost:52604/");
            RestRequest restRequest = new RestRequest("api/adddata", Method.Post);
            restRequest.AddJsonBody(JsonConvert.SerializeObject(bankData));
            RestResponse restResponse = restClient.Execute(restRequest);
            BankData result = JsonConvert.DeserializeObject<BankData>(restResponse.Content);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Update(int id, [FromBody] BankData bankData)
        {
            RestClient restClient = new RestClient("http://localhost:52604/");
            RestRequest restRequest = new RestRequest("api/updatedata/?id=" + id, Method.Post);
            restRequest.AddJsonBody(JsonConvert.SerializeObject(bankData));
            restClient.Execute(restRequest);
            
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            RestClient restClient = new RestClient("http://localhost:52604/");
            RestRequest restRequest = new RestRequest("api/deletedata/?id=" + id, Method.Delete);
            RestResponse restResponse = restClient.Execute(restRequest);
            BankData result = JsonConvert.DeserializeObject<BankData>(restResponse.Content);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Generate()
        {
            RestClient restClient = new RestClient("http://localhost:52604/");
            RestRequest request = new RestRequest("api/getalldata");
            RestResponse resp = restClient.Get(request);
            List<BankData> list = JsonConvert.DeserializeObject<List<BankData>>(resp.Content);

            int index = list.Count + 1;

            RestRequest request2 = new RestRequest("api/generatedata/?index=" + index.ToString(), Method.Post);
            RestResponse restResponse = restClient.Execute(request2);
            var data = (JObject)JsonConvert.DeserializeObject(restResponse.Content, new JsonSerializerSettings() { DateParseHandling = DateParseHandling.None });
            var status = (string)data["Status"];
            if (status.Equals("Success"))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
