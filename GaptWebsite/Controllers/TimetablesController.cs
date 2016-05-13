using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GaptWebsite.Models;

namespace GaptWebsite.Controllers
{
    public class TimetablesController : Controller
    {
        private CoursesDBEntities db = new CoursesDBEntities();

        // GET: Timetables
        public ActionResult Index()
        {
            var timetables = db.Timetables.Include(t => t.StudyUnit);
            
            return View(timetables.ToList());
        }

        // GET: Timetables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timetable timetable = db.Timetables.Find(id);
            if (timetable == null)
            {
                return HttpNotFound();
            }
            return View(timetable);
        }

        // GET: Timetables/Create
        public ActionResult Create()
        {
            ViewBag.UnitID = new SelectList(db.StudyUnits, "UnitID", "UnitName");
            ViewBag.LocationID = new SelectList(db.Locations, "ID", "RoomFloor");
            return View();
        }

        // POST: Timetables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,tTime,UnitID,tDate,LocationID")] Timetable timetable)
        {
            timetable.DateModified = DateTime.Now;
            if (ModelState.IsValid)
            {
                
                db.Timetables.Add(timetable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UnitID = new SelectList(db.StudyUnits, "UnitID", "UnitName", timetable.UnitID);
            ViewBag.LocationID = new SelectList(db.Locations, "ID", "RoomFloor", timetable.LocationID);
            return View(timetable);
        }

        // GET: Timetables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timetable timetable = db.Timetables.Find(id);
            if (timetable == null)
            {
                return HttpNotFound();
            }
            ViewBag.UnitID = new SelectList(db.StudyUnits, "UnitID", "UnitName", timetable.UnitID);
            return View(timetable);
        }

        // POST: Timetables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,tTime,UnitID,tDate")] Timetable timetable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timetable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UnitID = new SelectList(db.StudyUnits, "UnitID", "UnitName", timetable.UnitID);
            return View(timetable);
        }

        // GET: Timetables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timetable timetable = db.Timetables.Find(id);
            if (timetable == null)
            {
                return HttpNotFound();
            }
            return View(timetable);
        }

        // POST: Timetables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Timetable timetable = db.Timetables.Find(id);
            db.Timetables.Remove(timetable);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}