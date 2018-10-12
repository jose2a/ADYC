using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class CoursesController : ADYCBasedController
    {
        private CourseRepository _courseRepository;
        private CourseTypeRepository _courseTypeRepository;

        public CoursesController()
        {
            _courseRepository = new CourseRepository();
            _courseTypeRepository = new CourseTypeRepository();
        }

        // GET: Courses
        public async Task<ActionResult> Index()
        {
            var courses = await _courseRepository.GetCourses();

            return View(courses);
        }

        public async Task<ActionResult> New()
        {
            var viewModel = new CourseFormViewModel
            {
                IsNew = true,
                CourseTypes = await _courseTypeRepository.GetCourseTypes()
            };

            return View("CourseForm", viewModel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            CourseFormViewModel viewModel = null;

            try
            {
                var course = await _courseRepository.GetCourseById(id.Value);

                viewModel = new CourseFormViewModel(course)
                {
                    IsNew = false,
                    CourseTypes = await _courseTypeRepository.GetCourseTypes()
                };
            }
            catch (AdycHttpRequestException ahre)
            {
                if (ahre.StatusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
            }

            return View("CourseForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(CourseFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Course course = (form.IsNew)
                        ? new Course()
                        : await _courseRepository.GetCourseById(form.Id.Value);

                    course.Name = form.Name;
                    course.CourseTypeId = form.CourseTypeId;

                    if (form.IsNew)
                    {
                        await _courseRepository.PostCourse(course);
                    }
                    else
                    {
                        await _courseRepository.PutCourse(course.Id, course);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            form.CourseTypes = await _courseTypeRepository.GetCourseTypes();

            return View("CourseForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _courseRepository.DeleteCourse(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(ahre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Trash(int id)
        {
            try
            {
                var statusCode = await _courseRepository.TrashCourse(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(ahre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }

            return PartialView("pv_CourseRow", await _courseRepository.GetCourseById(id));
        }

        [HttpGet]
        public async Task<ActionResult> Restore(int id)
        {
            try
            {
                var statusCode = await _courseRepository.RestoreCourse(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(ahre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }

            return PartialView("pv_CourseRow", await _courseRepository.GetCourseById(id));
        }
    }
}