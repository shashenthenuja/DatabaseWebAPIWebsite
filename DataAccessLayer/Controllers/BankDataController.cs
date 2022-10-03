using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DataAccessLayer.Models;

namespace DataAccessLayer.Controllers
{
    public class BankDataController : ApiController
    {
        private bankEntities1 db = new bankEntities1();

        // GET: api/BankData
        public List<BankData> GetBankDatas()
        {
            List<BankData> bd = new List<BankData>();
            if (db.BankDatas != null)
            {
                foreach (BankData item in db.BankDatas)
                {
                    bd.Add(item);
                }
            }
            return bd;
        }

        // GET: api/BankData/5
        [ResponseType(typeof(BankData))]
        public IHttpActionResult GetBankData(int id)
        {
            BankData bankData = db.BankDatas.Find(id);
            if (bankData == null)
            {
                return NotFound();
            }

            return Ok(bankData);
        }

        // GET: api/BankData/5
        [ResponseType(typeof(BankData))]
        public IHttpActionResult GetBankDataSearch(string name)
        {
            BankData bankData = db.BankDatas.Find(name);
            if (bankData == null)
            {
                return NotFound();
            }

            return Ok(bankData);
        }

        // PUT: api/BankData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBankData(int id, BankData bankData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bankData.Id)
            {
                return BadRequest();
            }

            db.Entry(bankData).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BankData
        [ResponseType(typeof(BankData))]
        public IHttpActionResult PostBankData(BankData bankData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BankDatas.Add(bankData);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BankDataExists(bankData.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = bankData.Id }, bankData);
        }

        // DELETE: api/BankData/5
        [ResponseType(typeof(BankData))]
        public IHttpActionResult DeleteBankData(int id)
        {
            BankData bankData = db.BankDatas.Find(id);
            if (bankData == null)
            {
                return NotFound();
            }

            db.BankDatas.Remove(bankData);
            db.SaveChanges();

            return Ok(bankData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BankDataExists(int id)
        {
            return db.BankDatas.Count(e => e.Id == id) > 0;
        }
    }
}