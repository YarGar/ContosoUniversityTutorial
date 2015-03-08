using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;

namespace ContosoUniversity.Controllers
{
    public class StudentController : Controller
    {
        // Instantiates a db context object
        private SchoolContext db = new SchoolContext();

        // Gets a list of students from the Students entity set by reading 
        // the Students property of the database context instance
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /*dex*/)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        /* The commented out code of the following edit method is no longer recommended because the Bind 
         * attribute clears out any pre-existing data in fields not listed in the Include parameter. 
        * The new code reads the existing entity and calls TryUpdateModel to update fields from user 
        * input in the posted form data. It then sets a flag on the entity indicating it has been changed.
        * When the SaveChanges method is called, the Modified flag causes the Entity Framework to create SQL 
        * statements to update the database row. Concurrency conflicts are ignored, and all columns of the 
        * database row are updated, including those that the user didn't change. 
        * As a best practice to prevent overposting, the fields that you want to be updateable by the Edit
        * page are whitelisted in the TryUpdateModel parameters. If fields are added to the data model in the
        * future, they're automatically protected until they are explicitly added here.
        */
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)/*Edit([Bind(Include = "ID,LastName,FirstMidName,EnrollmentDate")] Student student)*/
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentToUpdate = db.Students.Find(id);
            if (TryUpdateModel(studentToUpdate, "",
               new string[] { "LastName", "FirstMidName", "EnrollmentDate" }))
            {
                try
                {
                    db.Entry(studentToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    
                    /*Wanted to redirect to the details page of modified student instead of index. 
                    * Learned I needed to built the solution before it would redirect properly.
                    * Im not sure why I need the "new" keyward but it seems to be common practice
                    */
                    return RedirectToAction("Details", new { id = id });
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(studentToUpdate);
            //if (ModelState.IsValid)
            //{
            //    db.Entry(student).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(student);
        }

        /* A try-catch block was added the HttpPost Delete method to handle any errors that might 
         * occur when the database is updated. If an error occurs, the HttpPost Delete method calls
         * the HttpGet Delete method, passing it a parameter that indicates that an error has occurred.
         * The HttpGet Delete method then redisplays the confirmation page along with the error message,
         * giving the user an opportunity to cancel or try again.
         */
        public ActionResult Delete(int? id, bool? saveChangesError=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again and if the problem persists, see your System Administrator.";
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                db.Students.Remove(student);
                db.SaveChanges();
            }
            catch(DataException /*dex*/)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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
