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
            foreach (course i in upcommingCourse)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LecturerId);
                // quản lý user, khi đăng nhập, lấy ID của đứa nào đăng nhập, system.web là những hàm trong thư viện để gọi id tương ứng, findbyID lấy id dựa vào id của user, dựa vào get identity
                i.Name = user.Name;

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