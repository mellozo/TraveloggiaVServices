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

   //  [EnableCors(origins: "http://html5.traveloggia.net/Default.html", headers: "*", methods: "*")]
    public class MapController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();

        // GET api/Map
        public IQueryable<Map> GetMaps()
        {
            var maps = db.Maps.Where(m=>m.MemberID==1 || m.MemberID ==77).OrderByDescending(m=>m.CreateDate);
            return maps.AsQueryable();
        }

         //GET api/Map/5
        //public Map GetMap()
        //{
        //    Map map = db.Maps.OrderByDescending(m=>m.CreateDate).FirstOrDefault();
        //    if (map == null)
        //    {
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        //    }

        //    return map;
        //}

        // PUT api/Map/5
        public HttpResponseMessage PutMap(int id, Map map)
        {
            if (ModelState.IsValid && id == map.MapID)
            {
                db.Entry(map).State = EntityState.Modified;

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

        // POST api/Map
        public HttpResponseMessage PostMap(Map map)
        {
            if (ModelState.IsValid)
            {
                db.Maps.Add(map);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, map);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = map.MapID }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Map/5
        public HttpResponseMessage DeleteMap(int id)
        {
            Map map = db.Maps.Find(id);
            if (map == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Maps.Remove(map);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, map);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}