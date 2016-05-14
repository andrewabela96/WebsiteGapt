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
    public class CourseController : ApiController
    {
        private CoursesDBEntities db = new CoursesDBEntities();

        // GET: api/Course
        public IQueryable GetCourses()
        {

            var p = db.Courses.Select(i => new {i.CourseID, i.CourseName, i.CourseDescription });

            return p;
        }

        // GET: api/Course/5
        //[ResponseType(typeof(Cours))]
        public IQueryable GetCours(string id)
        {
            var q = db.Courses.Select(i => new { i.CourseID, i.CourseName, i.CourseDescription }).Where(x => x.CourseName == id);

            return q;
        }





            /*Cours cours = db.Courses.Find(id);
            if (cours == null)
            {
                return NotFound();
            }

            return Ok(cours);*/
        //}

        // PUT: api/Course/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCours(string id, Cours cours)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cours.CourseID)
            {
                return BadRequest();
            }

            db.Entry(cours).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoursExists(id))
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

        // POST: api/Course
        [ResponseType(typeof(Cours))]
        public IHttpActionResult PostCours(Cours cours)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Courses.Add(cours);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CoursExists(cours.CourseID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cours.CourseID }, cours);
        }

        // DELETE: api/Course/5
        [ResponseType(typeof(Cours))]
        public IHttpActionResult DeleteCours(string id)
        {
            Cours cours = db.Courses.Find(id);
            if (cours == null)
            {
                return NotFound();
            }

            db.Courses.Remove(cours);
            db.SaveChanges();

            return Ok(cours);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CoursExists(string id)
        {
            return db.Courses.Count(e => e.CourseID == id) > 0;
        }
    }
}