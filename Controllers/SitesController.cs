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
    public class SitesController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();


        // PUT: api/Sites/5
        [ResponseType(typeof(void))]
        [EnableCors(origins: "http://www.traveloggia.pro , http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult PutSite(int id, Site site)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != site.SiteID)
            {
                return BadRequest();
            }

            db.Entry(site).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(id))
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

        // POST: api/Sites
        [ResponseType(typeof(Site))]
        [EnableCors(origins: "http://www.traveloggia.pro, http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult PostSite(Site site)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            site.DateAdded = DateTime.Now;
            try
            {
                db.Sites.Add(site);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                var ex = e;
            }
         
            return CreatedAtRoute("DefaultApi", new { id = site.SiteID }, site);
        }



        // GET: api/SelectSite/5
        [Route("api/SelectSite/{id:int}")]
        [ResponseType(typeof(Site))]
        [AcceptVerbs("GET")]
        [HttpGet]
        [EnableCors(origins: "http://www.traveloggia.pro ,  http://localhost:53382", headers: " *", methods: "*")]
        public IHttpActionResult SelectMap(int id)
        {
            var site = db.Sites.Where(s => s.SiteID == id).FirstOrDefault();
            return Ok(site);
        }




        // DELETE: api/Sites/5
        [ResponseType(typeof(Site))]
        [EnableCors(origins: "http://www.traveloggia.pro , http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult DeleteSite(int id)
        {
           Site  site = db.Sites.Where(s=> s.SiteID == id ).Include("Journals").Include("Photos").FirstOrDefault();
                if (site == null)
                {
                    return NotFound();
                }
                db.Sites.Remove(site);       
                return Ok(site);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SiteExists(int id)
        {
            return db.Sites.Count(e => e.SiteID == id) > 0;
        }
    }
}