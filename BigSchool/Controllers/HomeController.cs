using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BigSchoolContext context = new BigSchoolContext();
            var upcommingCourse = context.courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();

            var userID = User.Identity.GetUserId();
            foreach (course i in upcommingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);

                i.Name = user.Name;
                if (user != null)
                {
                    i.isLogin = true;
                    Attendance find = context.Attendances.FirstOrDefault(p => p.CourseId == i.Id && p.Attendee == userID);
                    if (find == null)
                    {
                        i.isShowGoing = true;
                    }

                    Following findfl = context.Followings.FirstOrDefault(p => p.FollowerId == userID && p.FolloweeId == i.LecturerId);
                    if (findfl == null)
                    {
                        i.isShowFollow = true;
                    }

                }

            }
            return View(upcommingCourse);
        }
        //public ActionResult Index()
        //{  BigSchoolContext context = new BigSchoolContext();
        //    var upcomingCourse = context.courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
        //    foreach(course i in upcomingCourse)
        //    {
        //        ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerID);
        //        i.Name = user.Name;

        //    }    
        //    return View(upcomingCourse);
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}