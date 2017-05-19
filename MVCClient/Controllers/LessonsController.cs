using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MVCClient.Models;
using MVCClient.Properties;
using MVCClient.Util;

namespace MVCClient.Controllers
{
    public class LessonsController : Controller
    {
        // GET: Lessons
        public ActionResult AddLesson()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddLesson(AddLessonViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = Resources.AddLessonError;
                return View(model);
            }
            try
            {
                await HttpClientEngine.Post("api/Lessons/AddLesson", model);
            }
            catch
            {
                ViewBag.Error = Resources.AddLessonError;
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult GetGroupsTable(int subjectId=-1)
        {
            if (subjectId == -1)
            {
                return View();
            }
            var model = HttpClientEngine.Get("api/Lessons/GetGroupsForSubject?subject=" + subjectId, typeof(List<GroupViewModel>));
            return View(model);
        }

        public ActionResult AddMark(int groupId)
        {
            HttpClientEngine.AccessToken = Request.Cookies.Get("token")?.Value;
            ViewBag.GroupId = groupId;
            int lessonId= (int)HttpClientEngine.Get("api/Lessons/GetLastLesson?groupId="+groupId, typeof(int));
            if (lessonId == -1)
            {
                ViewBag.Error = "Wrong Lesson";
                return RedirectToAction("GetGroupsTable", "Lessons");
            }
            var model = new AddMarksViewModel{LessonId=lessonId};
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddMark(AddMarksViewModel model)
        {
            try
            {
                await HttpClientEngine.Post("api/Lessons/AddMark", model);
            }
            catch
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult SubjectsPartial()
        {
            HttpClientEngine.AccessToken = Request.Cookies.Get("token")?.Value;
            var model = HttpClientEngine.Get("api/Subjects/Get", typeof(List<SubjectViewModel>));
            return PartialView(model);
        }

        public ActionResult LessonsPartial(int subjectId)
        {
            HttpClientEngine.AccessToken = Request.Cookies.Get("token")?.Value;
            var model = HttpClientEngine.Get("api/Lessons/Get?groupSubjectId="+subjectId, typeof(List<LessonRegisterViewModel>));
            return PartialView(model);
        }
        public ActionResult StudentsPartial(int groupId)
        {
            HttpClientEngine.AccessToken = Request.Cookies.Get("token")?.Value;
            var model = HttpClientEngine.Get("api/Groups/GetStudents?groupId=" + groupId, typeof(List<StudentViewModel>));
            return View(model);
        }
    }
}