using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class CourcesController : Controller
    {
        BigSchoolContext context = new BigSchoolContext();
        // GET: Cources
        public ActionResult Create()
        {
            course objCourse = new course();
            objCourse.ListCategory = context.Categories.ToList();
            return View(objCourse);
            
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(course objCourse)
        {
            ModelState.Remove("LecturerId");

            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = context.Categories.ToList();
                return View("Create", objCourse);
            }
            ApplicationUser user= System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;
            context.courses.Add(objCourse);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendances.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<course>();
            foreach(Attendance temp in listAttendances)
            {
                course objCourse = temp.course;
                objCourse.LectureName = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
.FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }
        public ActionResult Mine()
        {
            ApplicationUser curentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.courses.Where(c => c.LecturerId ==curentUser.Id && c.DateTime > DateTime.Now).ToList();
       foreach(course i in courses)
            {
                i.LectureName = curentUser.Name;
            }
            return View(courses);
        }
    }
}