using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;

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


        public ActionResult Edit(int id)
        {
            course cs = context.courses.FirstOrDefault(p => p.Id == id);
            if (cs == null)
            {
                return HttpNotFound();
            }
            return View(cs);

        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(course cs)
        {
            course update = context.courses.FirstOrDefault(p => p.Id == cs.Id);
            if(update != null)
            {
                context.courses.AddOrUpdate(cs);
                context.SaveChanges();
            }
            return RedirectToAction("Mine");
        }

        public ActionResult Delete(int id)
        {
            course cs = context.courses.FirstOrDefault(p => p.Id == id);
            if (cs == null)
            {
                return HttpNotFound();
            }
            return View(cs);

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(course cs)
        {
            course dele = context.courses.FirstOrDefault(p => p.Id == cs.Id);
            if (dele != null)
            {
                context.courses.Remove(dele);
                context.SaveChanges();
            }
            return RedirectToAction("Mine");
        }
 


        public ActionResult LectureImGoing()
        {
            ApplicationUser curentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var listFollowee = context.Followings.Where(p => p.FollowerId == curentUser.Id).ToList();
            var listAttendances = context.Attendances.Where(p => p.Attendee == curentUser.Id).ToList();
            var csr = new List<course>();
            foreach (var crse in listAttendances)
            {
                foreach (var item in listFollowee)
                {
                    if (item.FolloweeId == crse.course.LecturerId)
                    {
                        course objCourse = crse.course;
                        objCourse.LectureName =
                       System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                        .FindById(objCourse.LecturerId).Name;
                        csr.Add(objCourse);
                    }
                }
            }
            return View(csr);
        }
        public ActionResult details(int id)
        {
            BigSchoolContext context = new BigSchoolContext();

            return View();
        }
    }
}