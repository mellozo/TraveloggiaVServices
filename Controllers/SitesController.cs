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
using System.Web.Http.Cors;

namespace disaster.Controllers
{
    public class SitesController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();

        // POST: api/Sites
        [ResponseType(typeof(Site))]
        [EnableCors(origins: "http://traveloggiaauthservice.net.rosebloom.arvixe.com,  http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult PostSite(Site site)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            site.DateAdded = DateTime.Now;
            db.Sites.Add(site);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = site.SiteID }, site);
        }



        [Route("api/Site/6")]
        [ResponseType(typeof(IEnumerable<Site>))]
        [AcceptVerbs("GET")]
        [HttpGet]
        [EnableCors(origins: "http://traveloggiaauthservice.net.rosebloom.arvixe.com, http://html5.traveloggia.net, http://localhost:53382", headers: "*", methods: "*")]
        public IEnumerable<Site> GetSitesByMapId(int id)
        {
            return db.Sites.Where(s => s.MapID == id).AsEnumerable();//.Include(s=>s.Photos);

        }

        // PUT: api/Sites/5
        [ResponseType(typeof(void))]
        [EnableCors(origins: "http://traveloggiaauthservice.net.rosebloom.arvixe.com, http://html5.traveloggia.net, http://localhost:53382", headers: "*", methods: "*")]
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

   

        // DELETE: api/Sites/5
        [ResponseType(typeof(Site))]
        [EnableCors(origins: "http://traveloggiaauthservice.net.rosebloom.arvixe.com, http://html5.traveloggia.net, http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult DeleteSite(int id)
        {
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return NotFound();
            }

            db.Sites.Remove(site);
            db.SaveChanges();

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