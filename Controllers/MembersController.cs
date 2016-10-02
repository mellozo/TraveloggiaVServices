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
    public class MembersController : ApiController
    {
        private traveloggiaDBEntities db = new traveloggiaDBEntities();


        [Route("api/Members/Validate")]
        [ResponseType(typeof(Member))]
        [AcceptVerbs("POST")]
        [HttpPost]
        [EnableCors(origins: "http://www.traveloggia.pro , http://traveloggia.pro , http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult ValidateMember(Member param)
        {
            Member member = null;
            try
            {
                var mel = db.Members.Where(membr => membr.Email == param.Email).FirstOrDefault();
                member = mel;
            }
            catch (Exception e)
            {
                var x = e;
            }

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }


        // POST: api/Members
        [ResponseType(typeof(Member))]
        [EnableCors(origins: "http://www.traveloggia.pro , http://traveloggia.pro ,  http://localhost:53382", headers: "*", methods: "*")]
        public IHttpActionResult PostMember(Member member)
        {
            member.AccountCreateDate = DateTime.Now;
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors);
            //    return BadRequest(ModelState);
            //}
            Member memberAlready = db.Members.Where(membr => membr.Email == member.Email).FirstOrDefault();
            if (memberAlready != null)
            {
                return BadRequest("member exists already");
            }
            db.Members.Add(member);
            db.SaveChanges();

            Map defaultMap = new Map();
            defaultMap.MemberID = member.MemberID;
            defaultMap.CreateDate = DateTime.Now;
            defaultMap.MapName = "DefaultMap " + DateTime.Now.ToShortDateString();
            try
            {
                db.Maps.Add(defaultMap);
                db.SaveChanges();
            }
            catch (Exception e)
            {
            }
            return CreatedAtRoute("DefaultApi", new { id = member.MemberID }, member);
        }










        // PUT: api/Members/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMember(int id, Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != member.MemberID)
            {
                return BadRequest();
            }

            db.Entry(member).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
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


        // DELETE: api/Members/5
        [ResponseType(typeof(Member))]
        public IHttpActionResult DeleteMember(int id)
        {
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }

            db.Members.Remove(member);
            db.SaveChanges();

            return Ok(member);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberExists(int id)
        {
            return db.Members.Count(e => e.MemberID == id) > 0;
        }
    }
}