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
using GaptWebsite.Models;

namespace GaptWebsite.Controllers
{
    public class TimetableAppController : ApiController
    {
        private CoursesDBEntities db = new CoursesDBEntities();

        // GET: api/TimetableApp
        public IQueryable GetTimetable()
        {

            var r = db.Timetables.Select(i => new { i.UnitID, i.tTime, i.tDate, i.LocationID });
            return r;
        }

        // GET: api/TimetableApp/5
        [ResponseType(typeof(Timetable))]
        public IHttpActionResult GetTimetable(int id)
        {
            Timetable timetable = db.Timetables.Find(id);
            if (timetable == null)
            {
                return NotFound();
            }

            return Ok(timetable);
        }

        // PUT: api/TimetableApp/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTimetable(int id, Timetable timetable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != timetable.ID)
            {
                return BadRequest();
            }

            db.Entry(timetable).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimetableExists(id))
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

        // POST: api/TimetableApp
        [ResponseType(typeof(Timetable))]
        public IHttpActionResult PostTimetable(Timetable timetable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Timetables.Add(timetable);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = timetable.ID }, timetable);
        }

        // DELETE: api/TimetableApp/5
        [ResponseType(typeof(Timetable))]
        public IHttpActionResult DeleteTimetable(int id)
        {
            Timetable timetable = db.Timetables.Find(id);
            if (timetable == null)
            {
                return NotFound();
            }

            db.Timetables.Remove(timetable);
            db.SaveChanges();

            return Ok(timetable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TimetableExists(int id)
        {
            return db.Timetables.Count(e => e.ID == id) > 0;
        }
    }
}