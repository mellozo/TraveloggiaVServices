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
    public class PhotosController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();

        // GET: api/Photos/5
        [ResponseType(typeof(Photo))]
        [EnableCors(origins: "http://traveloggia.pro.rosebloom.arvixe.com/, http://html5.traveloggia.net, http://localhost:53382", headers: "*", methods: "*")]
        public IQueryable<Photo> GetPhoto(int id)
        {
            IQueryable<Photo> photos = db.Photos.Where(pic => pic.SiteID == id).AsQueryable();
            //if (photos == null)
            //{
            //    return NotFound();
            //}
            return photos.AsQueryable();
        }

        // POST: api/Photos
        [ResponseType(typeof(Photo))]
        [EnableCors(origins: "http://traveloggia.pro.rosebloom.arvixe.com/, http://html5.traveloggia.net, http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult PostPhoto(Photo photo)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            photo.DateAdded = DateTime.Now;
            db.Photos.Add(photo);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = photo.PhotoID }, photo);
        }


        // PUT: api/Photos/5
        [ResponseType(typeof(void))]
        [EnableCors(origins: "http://traveloggia.pro.rosebloom.arvixe.com/, http://html5.traveloggia.net, http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult PutPhoto(int id, Photo photo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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





        // DELETE: api/Photos/5
        [ResponseType(typeof(Photo))]
        [EnableCors(origins: "http://traveloggia.pro.rosebloom.arvixe.com/, http://html5.traveloggia.net, http://localhost:53382", headers: "*", methods: "*")]
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