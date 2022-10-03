using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace WebGUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home";

            RestClient restClient = new RestClient("http://localhost:52604/");
            RestRequest request = new RestRequest("api/getalldata");
            RestResponse resp = restClient.Get(request);

            List<BankData> data = JsonConvert.DeserializeObject<List<BankData>>(resp.Content);

            return View(data);
        }
    }
}
