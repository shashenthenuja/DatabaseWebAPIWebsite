using BankLib;
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
    public class GenerateDataController : ApiController
    {
        Access access = new Access();
        DatabaseClass data = new DatabaseClass();
        public IHttpActionResult GenerateData([FromUri] int index)
        {
            List<DataStruct> list = data.getList();
            int count = index;
            foreach (DataStruct item in list)
            {
                BankData bd = new BankData();
                bd.Id = count;
                bd.FirstName = item.firstName;
                bd.LastName = item.lastName;
                bd.AccNum = (int)item.acctNo;
                bd.Pin = (int)item.pin;
                bd.Balance = item.balance;
                bd.Image = item.image;
                Console.WriteLine(">>>>>" + bd.AccNum);
                RestRequest request = new RestRequest("api/bankdata/", Method.Post);
                request.AddJsonBody(JsonConvert.SerializeObject(bd));
                RestResponse response = access.restClient.Execute(request);
                BankData returnData = JsonConvert.DeserializeObject<BankData>(response.Content);
                if (returnData != null)
                {
                    Console.WriteLine("Data Successfully Inserted");
                }
                else
                {
                    Console.WriteLine("Error details:" + response.Content);
                }
                count++;
            }
            if (count != 0)
            {
                return Json(new { Status = "Success" });
            }
            else
            {
                return Json(new { Status = "Failed" });
            }

        }
    }
}
