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
    public class LecturersController : ApiController
    {
        private CoursesDBEntities db = new CoursesDBEntities();

        // GET: api/Lecturers
        public IQueryable GetLecturers()
        {
            var l = db.Lecturers.Select(i => new { i.LecturerID, i.LecturerName, i.LecturerEmail, i.LecturerJob });
            return l;
        }

        // GET: api/Lecturers/5
        [ResponseType(typeof(Lecturer))]
        public IHttpActionResult GetLecturer(int id)
        {
            Lecturer lecturer = db.Lecturers.Find(id);
            if (lecturer == null)
            {
                return NotFound();
            }

            return Ok(lecturer);
        }

        // PUT: api/Lecturers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLecturer(int id, Lecturer lecturer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != lecturer.LecturerID)
            {
                return BadRequest();
            }

            db.Entry(lecturer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LecturerExists(id))
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

        // POST: api/Lecturers
        [ResponseType(typeof(Lecturer))]
        public IHttpActionResult PostLecturer(Lecturer lecturer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Lecturers.Add(lecturer);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LecturerExists(lecturer.LecturerID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = lecturer.LecturerID }, lecturer);
        }

        // DELETE: api/Lecturers/5
        [ResponseType(typeof(Lecturer))]
        public IHttpActionResult DeleteLecturer(int id)
        {
            Lecturer lecturer = db.Lecturers.Find(id);
            if (lecturer == null)
            {
                return NotFound();
            }

            db.Lecturers.Remove(lecturer);
            db.SaveChanges();

            return Ok(lecturer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LecturerExists(int id)
        {
            return db.Lecturers.Count(e => e.LecturerID == id) > 0;
        }
    }
}