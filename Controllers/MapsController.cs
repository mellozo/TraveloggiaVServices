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
    public class MapsController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();

        [Route("api/Map")]
        [ResponseType(typeof(IEnumerable<Map>))]
        [AcceptVerbs("GET")]
        [HttpGet]
        public IQueryable<Map> GetDefaultMaps()
        {
            var maps = db.Maps.Where(m => m.MemberID == 1 || m.MemberID == 77).OrderByDescending(m => m.CreateDate);
            // .Include(a => a.Children.Select( c=> c.ChildRelationshipType));
            //http://stackoverflow.com/questions/5905716/how-do-i-eagerly-include-the-child-and-grandchild-elements-of-an-entity-in-entit
            return maps.AsQueryable();
        }


        //// GET: api/Maps
        //public IQueryable<Map> GetMaps()
        //{
        //    return db.Maps;
        //}http://traveloggiaauthservice.net.rosebloom.arvixe.com
        //http://localhost:53382/
        // GET: api/Maps/5
        [ResponseType(typeof(IEnumerable<Map>))]
        [EnableCors(origins: "http://traveloggiaauthservice.net.rosebloom.arvixe.com", headers: "*", methods: "*")]
        public IQueryable<Map> GetMaps(int id)
        {
            var maps = db.Maps.Where(m => m.MemberID == id).OrderByDescending(m => m.CreateDate);
            // to do if there are no maps create a map and return it
            if (maps.Count() ==0)
            {
                Map defaultMap = new Map();
                defaultMap.MapName = "DefaultMap_" + DateTime.Now.ToShortDateString().Replace("/", "_");
                defaultMap.MemberID = id;
                defaultMap.CreateDate = DateTime.Now;
                db.Maps.Add(defaultMap);
                try
                {
                    db.SaveChanges();
                    maps = db.Maps.Where(m => m.MemberID == id).OrderByDescending(m => m.CreateDate);
                }
                catch(Exception ex)
                {
                    
                }
              
            }

            return maps;
        }

        // PUT: api/Maps/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMap(int id, Map map)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != map.MapID)
            {
                return BadRequest();
            }

            db.Entry(map).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapExists(id))
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

        // POST: api/Maps
        [ResponseType(typeof(Map))]
        public IHttpActionResult PostMap(Map map)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Maps.Add(map);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = map.MapID }, map);
        }

        // DELETE: api/Maps/5
        [ResponseType(typeof(Map))]
        public IHttpActionResult DeleteMap(int id)
        {
            Map map = db.Maps.Find(id);
            if (map == null)
            {
                return NotFound();
            }

            db.Maps.Remove(map);
            db.SaveChanges();

            return Ok(map);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MapExists(int id)
        {
            return db.Maps.Count(e => e.MapID == id) > 0;
        }
    }
}