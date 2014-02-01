using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AngularServices.Models;

namespace AngularServices.Controllers
{
    /*
     * this is the version that works 
     * PM> Install-Package Microsoft.AspNet.WebApi.Cors -Version 5.0.0
     * 
     * */


    [EnableCors(origins: "http://localhost/TraveloggiaV", headers: "*", methods: "*")]
    public class JournalController : ApiController
    {


        private traveloggiaDBEntities DB = new traveloggiaDBEntities();
     

        // GET api/journal/5
        public IQueryable<Journal> Get(int id)
        {
            return DB.Journals.Where(j => j.SiteID == id).AsQueryable();
        }

        // POST api/journal
        public void Post(Journal j)
        {

            DB.Journals.Add(j);
            DB.SaveChanges();
        }

        // PUT api/journal/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/journal/5
        public void Delete(int id)
        {
        }
    }
}
