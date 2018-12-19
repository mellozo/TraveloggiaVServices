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
using REST_API.Models;
using System.Web.Http.Cors;

namespace REST_API.Controllers
{
    public class JournalsController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();

   

        // GET: api/Journals/5
        [ResponseType(typeof(Journal))]
        [EnableCors(origins: "http://www.traveloggia.pro ,https://www.traveloggia.pro , http://traveloggia.pro ,  https://traveloggia.pro , http://html5.traveloggia.net, http://localhost:53382", headers: "*", methods: "*")]
        public IQueryable<Journal> GetJournal(int id)
        {
            return db.Journals.Where(j => j.SiteID == id).AsQueryable();


            //Journal journal = db.Journals.Find(id);
            //if (journal == null)
            //{
            //    return NotFound();
            //}

            //return Ok(journal);
        }

        // PUT: api/Journals/5
        [ResponseType(typeof(void))]
        [EnableCors(origins: "http://www.traveloggia.pro ,https://www.traveloggia.pro, https://traveloggia.pro, http://traveloggia.pro ,  http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult PutJournal(int id, Journal journal)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (id != journal.JournalID)
            {
                return BadRequest();
            }

            db.Entry(journal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JournalExists(id))
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

        // POST: api/Journals
        [ResponseType(typeof(Journal))]
        [EnableCors(origins: "http://www.traveloggia.pro , https://www.traveloggia.pro, https://traveloggia.pro, http://traveloggia.pro ,  http://html5.traveloggia.net, http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult PostJournal(Journal journal)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            journal.DateAdded = DateTime.Now;
            journal.JournalDate = DateTime.Now;
            db.Journals.Add(journal);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = journal.JournalID }, journal);
        }

        // DELETE: api/Journals/5
        [ResponseType(typeof(Journal))]
        [EnableCors(origins: "http://www.traveloggia.pro, https://www.traveloggia.pro, https://traveloggia.pro , http://traveloggia.pro ,  http://html5.traveloggia.net, http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult DeleteJournal(int id)
        {
            Journal journal = db.Journals.Find(id);
            if (journal == null)
            {
                return NotFound();
            }

            db.Journals.Remove(journal);
            db.SaveChanges();

            return Ok(journal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JournalExists(int id)
        {
            return db.Journals.Count(e => e.JournalID == id) > 0;
        }
    }
}