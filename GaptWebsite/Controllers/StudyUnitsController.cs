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
    public class StudyUnitsController : ApiController
    {
        private CoursesDBEntities db = new CoursesDBEntities();

        // GET: api/StudyUnits
        public IQueryable GetStudyUnits()
        {
            var s = db.StudyUnits.Select(i => new { i.UnitID, i.UnitName, i.UnitDescription, i.UnitElective, i.UnitCredits, i.UnitCompensatable, i.UnitSemester, i.UnitYear });
            return s;
        }

        // GET: api/StudyUnits/5
        //[ResponseType(typeof(StudyUnit))]
        public IQueryable GetStudyUnit(int year)
        {
            var s = db.StudyUnits.Where(i => i.UnitYear == year);

            /*var p = from s in db.StudyUnits
                    where s.UnitYear == year && s.Courses.Any(c => c.CourseID == course) 
                    select new { s.UnitID, s.UnitName, s.UnitDescription };*/
                    
                    
            return s;
        }

        // PUT: api/StudyUnits/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStudyUnit(string id, StudyUnit studyUnit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != studyUnit.UnitID)
            {
                return BadRequest();
            }

            db.Entry(studyUnit).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyUnitExists(id))
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

        // POST: api/StudyUnits
        [ResponseType(typeof(StudyUnit))]
        public IHttpActionResult PostStudyUnit(StudyUnit studyUnit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StudyUnits.Add(studyUnit);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (StudyUnitExists(studyUnit.UnitID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = studyUnit.UnitID }, studyUnit);
        }

        // DELETE: api/StudyUnits/5
        [ResponseType(typeof(StudyUnit))]
        public IHttpActionResult DeleteStudyUnit(string id)
        {
            StudyUnit studyUnit = db.StudyUnits.Find(id);
            if (studyUnit == null)
            {
                return NotFound();
            }

            db.StudyUnits.Remove(studyUnit);
            db.SaveChanges();

            return Ok(studyUnit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudyUnitExists(string id)
        {
            return db.StudyUnits.Count(e => e.UnitID == id) > 0;
        }
    }
}