﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using REST_API.Models;
using System.Web.Http.Cors;

namespace REST_API.Controllers
{
    public class MapsController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();


        // GET: api/Maps/5
        [ResponseType(typeof(Map))]
        [EnableCors(origins: "http://www.traveloggia.pro , https://www.traveloggia.pro , https://traveloggia.pro , http://traveloggia.pro, http://localhost:53382 ", headers: "*", methods: "*")]
        public IHttpActionResult GetMaps(int id)
        {
            Map map = null;
            try
            {
                 Map selectedmap  = db.Maps.Where(m => m.MemberID == id && m.IsDeleted != true).OrderByDescending(m => m.CreateDate).FirstOrDefault();
                 var validSites = db.Sites.Where(s => s.MapID == selectedmap.MapID && s.IsDeleted != true).OrderBy(s => s.RouteIndex).ThenBy(s => s.Arrival).ToList();
                if (selectedmap != null)
                {
                    map = new Map();
                    map.MapID = selectedmap.MapID;
                    map.MapName = selectedmap.MapName;
                    map.CreateDate = selectedmap.CreateDate;
                    map.CrowdSourced = selectedmap.CrowdSourced;
                    map.FromPhone = selectedmap.FromPhone;
                    map.HasLayers = selectedmap.HasLayers;
                    map.IsDeleted = selectedmap.IsDeleted;
                    map.LastRevision = selectedmap.LastRevision;
                    map.MemberID = selectedmap.MemberID;

                    foreach (Site s in validSites)
                    {
                        map.Sites.Add(s);
                    }
                }

            }
            catch (Exception ex)
            {
                var e = ex;
            }
          
            // to do if there are no maps create a map and return it
            if ( map == null)
            {
                Map defaultMap = new Map();
                defaultMap.MapName = DateTime.Now.ToShortDateString().Replace(" ", "_");
                defaultMap.MemberID = id;
                defaultMap.CreateDate = DateTime.Now;
                db.Maps.Add(defaultMap);
                try
                {
                    db.SaveChanges();
                    map = db.Maps.Where(m => m.MemberID == id).OrderByDescending(m => m.CreateDate).FirstOrDefault();
                }
                catch (Exception ex)
                {

                }

            }

            return Ok(map);
       
        }


        // GET: api/MapList/5
        [Route("api/MapList/{id:int}")]
        [ResponseType(typeof(IEnumerable<MapListItem>))]
        [AcceptVerbs("GET")]
        [HttpGet]
        [EnableCors(origins: "http://www.traveloggia.pro ,https://www.traveloggia.pro, https://traveloggia.pro, http://traveloggia.pro ,  http://localhost:53382", headers: " *", methods: "*")]
        public List<MapListItem> GetMapList(int id)
        {
            var maps = db.Maps.Where(m => m.MemberID == id && m.IsDeleted != true).Select(m=> new MapListItem { MapID = m.MapID, MapName = m.MapName, MemberID = m.MemberID, CreateDate = m.CreateDate }).OrderByDescending(m => m.CreateDate);
            return maps.ToList();
        }


        // GET: api/SelectMap/5
        [Route("api/SelectMap/{id:int}")]
        [ResponseType(typeof(Map))]
        [AcceptVerbs("GET")]
        [HttpGet]
        [EnableCors(origins: "http://www.traveloggia.pro ,https://www.traveloggia.pro, https://traveloggia.pro, http://traveloggia.pro , http://localhost:53382", headers: " *", methods: "*")]
        public IHttpActionResult SelectMap(int id)
        {
            try
            {
                Map  selectedmap = db.Maps.Where(m => m.MapID == id).FirstOrDefault();
                var validSites = db.Sites.Where(s => s.MapID == selectedmap.MapID && s.IsDeleted != true).OrderBy(s => s.RouteIndex).ThenBy(s => s.Arrival).ToList();
                Map map = new Map();
                map.MapID = selectedmap.MapID;
                map.MapName = selectedmap.MapName;
                map.CreateDate = selectedmap.CreateDate;
                map.CrowdSourced = selectedmap.CrowdSourced;
                map.FromPhone = selectedmap.FromPhone;
                map.HasLayers = selectedmap.HasLayers;
                map.IsDeleted = selectedmap.IsDeleted;
                map.LastRevision = selectedmap.LastRevision;
                map.MemberID = selectedmap.MemberID;
                
                foreach(Site s in validSites)
                {
                    map.Sites.Add(s);
                }
                return Ok(map);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
          
        }







        [Route("api/Map")]
        [ResponseType(typeof(IEnumerable<Map>))]
        [AcceptVerbs("GET")]
        [HttpGet]
        [EnableCors(origins: "http://html5.traveloggia.net", headers: "*", methods: "*")]
        public IQueryable<Map> GetDefaultMaps()
        {
            var maps = db.Maps.Where(m => m.MemberID == 1 || m.MemberID == 77).Include("Sites").OrderByDescending(m => m.CreateDate);
            return maps.AsQueryable();
        }


        // PUT: api/Maps/5
        [ResponseType(typeof(void))]
        [EnableCors(origins: "http://www.traveloggia.pro ,https://www.traveloggia.pro, https://traveloggia.pro, http://traveloggia.pro , http://localhost:53382", headers: "*", methods: "*")]
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

            if(id <= 6088)
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
        [EnableCors(origins: "http://www.traveloggia.pro , https://www.traveloggia.pro, https://traveloggia.pro, http://traveloggia.pro ,  http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult PostMap(Map map)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            map.CreateDate = DateTime.Now;

            db.Maps.Add(map);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = map.MapID }, map);
        }

        // DELETE: api/Maps/5
        [ResponseType(typeof(Map))]
        [EnableCors(origins: "http://www.traveloggia.pro ,https://www.traveloggia.pro, https://traveloggia.pro, http://traveloggia.pro , http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult DeleteMap(int id)
        {

            var map = db.Maps.Where(m => m.MapID == id).First();
            map.IsDeleted = true;
         //   db.Entry(map).State = EntityState.Modified;

       
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