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
    public class PhotosController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();



        // GET: api/Photos/5
        [ResponseType(typeof(IEnumerable<Photo>))]
        [EnableCors(origins: "http://www.traveloggia.pro , https://www.traveloggia.pro, https://traveloggia.pro,  http://traveloggia.pro ,  http://html5.traveloggia.net, http://localhost", headers: "*", methods: "*")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public List <Photo> GetPhoto(int id)
        {
            var photos = db.Photos.Where(pic => pic.SiteID == id);
            List<Photo> plist = null; 
            //if (photos == null)
            //{
            //    return NotFound();
            //}
            try
            {
                plist= photos.ToList();
            }
            catch (Exception ex)
            {

            }
            return plist;
        }

        // PUT: api/Photos/5
        [ResponseType(typeof(void))]
        [EnableCors(origins: "http://www.traveloggia.pro , https://www.traveloggia.pro, https://traveloggia.pro,  http://traveloggia.pro ,  http://html5.traveloggia.net, http://localhost", headers: "*", methods: "*")]
        [AcceptVerbs("PUT")]
        [HttpPut]
        public IHttpActionResult PutPhoto(int id, Photo photo)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (id != photo.PhotoID)
            {
                return BadRequest();
            }

            db.Entry(photo).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
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

        // POST: api/Photos
        [ResponseType(typeof(Photo))]
        [EnableCors(origins: "http://www.traveloggia.pro , https://www.traveloggia.pro, https://traveloggia.pro, http://traveloggia.pro ,  http://html5.traveloggia.net, http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult PostPhoto(Photo photo)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            DateTime validDate = DateTime.Now;
            // string sqlFormattedDate = photoDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            if (photo.DateTaken.HasValue)
              validDate = photo.DateTaken.Value.ToUniversalTime();
            photo.DateTaken = validDate.ToLocalTime();
            photo.DateAdded = DateTime.Now;
            db.Photos.Add(photo);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = photo.PhotoID }, photo);
        }

        // DELETE: api/Photos/5
        [ResponseType(typeof(Photo))]
        [EnableCors(origins: "http://www.traveloggia.pro ,https://www.traveloggia.pro, https://traveloggia.pro, http://traveloggia.pro , http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult DeletePhoto(int id)
        {
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return NotFound();
            }

            db.Photos.Remove(photo);
            db.SaveChanges();

            return Ok(photo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhotoExists(int id)
        {
            return db.Photos.Count(e => e.PhotoID == id) > 0;
        }
    }
}