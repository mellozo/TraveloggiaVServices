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
using disaster.Models;

namespace disaster.Controllers
{
    public class JournalsController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();

        // GET: api/Journals
        //public IQueryable<Journal> GetJournals()
        //{
        //    return db.Journals;
        //}

        // GET: api/Journals/5
        [ResponseType(typeof(Journal))]
        public IQueryable<Journal> GetJournal(int id)
        {
            return db.Journals.Where(j => j.SiteID == id).AsQueryable();
           
        }


        // POST: api/Journals
        [ResponseType(typeof(Journal))]
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

        //// GET: api/Journals/5
        //[ResponseType(typeof(Journal))]
        //public IHttpActionResult GetJournal(int id)
        //{
        //    Journal journal = db.Journals.Find(id);
        //    if (journal == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(journal);
        //}

        // PUT: api/Journals/5
        [ResponseType(typeof(void))]
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


        // DELETE: api/Journals/5
        [ResponseType(typeof(Journal))]
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