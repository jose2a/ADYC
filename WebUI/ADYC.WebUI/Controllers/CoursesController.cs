using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class CoursesController : Controller
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
            var courses = await _courseRepository.GetCoursesAsync();

            return View(courses);
        }

        public async Task<ActionResult> New()
        {
            var viewModel = new CourseFormViewModel
            {
                IsNew = true,
                CourseTypes = await _courseTypeRepository.GetCourseTypesAsync()
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
                var course = await _courseRepository.GetCourseAsync(id.Value);

                viewModel = new CourseFormViewModel(course)
                {
                    IsNew = false,
                    CourseTypes = await _courseTypeRepository.GetCourseTypesAsync()
                };
            }
            catch (AdycHttpRequestException ahre)
            {
                if (ahre.StatusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                foreach (var error in ahre.Errors)
                {
                    ModelState.AddModelError("", ahre.Message); 
                }
            }

            return View("CourseForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(CourseFormViewModel form)
        {
            form.IsNew = form.Id == null;

            if (ModelState.IsValid)
            {
                try
                {
                    Course course = (form.IsNew)
                        ? new Course()
                        : await _courseRepository.GetCourseAsync(form.Id.Value);

                    course.Name = form.Name;
                    course.CourseTypeId = form.CourseTypeId;

                    if (form.IsNew)
                    {
                        await _courseRepository.PostCourseAsync(course);
                    }
                    else
                    {
                        await _courseRepository.PutCourseAsync(course.Id, course);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    foreach (var error in ahre.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            form.CourseTypes = await _courseTypeRepository.GetCourseTypesAsync();

            return View("CourseForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _courseRepository.DeleteCourseAsync(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = "";

                foreach (var error in ahre.Errors)
                {
                    errorString += error;
                }

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }
            
        }

        [HttpGet]
        public async Task<ActionResult> Trash(int id)
        {
            try
            {
                var statusCode = await _courseRepository.TrashCourseAsync(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = "";

                foreach (var error in ahre.Errors)
                {
                    errorString += error;
                }

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }

            return PartialView("pv_CourseRow", await _courseRepository.GetCourseAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> Restore(int id)
        {
            try
            {
                var statusCode = await _courseRepository.RestoreCourseAsync(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = "";

                foreach (var error in ahre.Errors)
                {
                    errorString += error;
                }

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }

            return PartialView("pv_CourseRow", await _courseRepository.GetCourseAsync(id));
        }
    }
}