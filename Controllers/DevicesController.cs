﻿using System;
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
    public class DevicesController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();

        // POST: api/Devices
        [ResponseType(typeof(Device))]
        [EnableCors(origins: "http://www.traveloggia.pro ,https://www.traveloggia.pro , http://traveloggia.pro ,https://traveloggia.pro , http://localhost", headers: "*", methods: "*")]
        public IHttpActionResult PostDevice(Device device)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            device.DateRecorded = DateTime.Now;
            db.Devices.Add(device);
            db.SaveChanges();
            return CreatedAtRoute("DefaultApi", new { id = device.id }, device);
        }







        // GET: api/Devices
        public IQueryable<Device> GetDevices()
        {
            return db.Devices;
        }

        // GET: api/Devices/5
        [ResponseType(typeof(Device))]
        public IHttpActionResult GetDevice(int id)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return NotFound();
            }

            return Ok(device);
        }

        // PUT: api/Devices/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDevice(int id, Device device)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != device.id)
            {
                return BadRequest();
            }

            db.Entry(device).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(id))
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

     

        // DELETE: api/Devices/5
        [ResponseType(typeof(Device))]
        public IHttpActionResult DeleteDevice(int id)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return NotFound();
            }

            db.Devices.Remove(device);
            db.SaveChanges();

            return Ok(device);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeviceExists(int id)
        {
            return db.Devices.Count(e => e.id == id) > 0;
        }
    }
}