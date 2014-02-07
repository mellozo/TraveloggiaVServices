using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TraveloggiaWebApi.Models;
//using System.Web.Http.Cors;

namespace TraveloggiaWebApi.Controllers
{
       // [EnableCors(origins: "http://localhost/TraveloggiaVClient", headers: "*", methods: "*")]
    public class SiteController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();

        // GET api/Site/5
        public IEnumerable<Site> GetSites(int id)
        {
            return db.Sites.Where(s => s.MapID == id).AsEnumerable();//.Include(s=>s.Photos);

        }


        // GET api/Site
        public IEnumerable<Site> GetSites()
        {
            return db.Sites.AsEnumerable();
        }

        // GET api/Site/5
        //public Site GetSite(int id)
        //{
        //    Site site = db.Sites.Find(id);
        //    if (site == null)
        //    {
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        //    }

        //    return site;
        //}


      

        // PUT api/Site/5
        public HttpResponseMessage PutSite(int id, Site site)
        {
            if (ModelState.IsValid && id == site.SiteID)
            {
                db.Entry(site).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Site
        public HttpResponseMessage PostSite(Site site)
        {
            if (ModelState.IsValid)
            {
                db.Sites.Add(site);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, site);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = site.SiteID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Site/5
        public HttpResponseMessage DeleteSite(int id)
        {
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Sites.Remove(site);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, site);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}