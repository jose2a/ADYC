using ADYC.WebUI.Models;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net.Http;
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
                    CourseTypes = await _courseTypeRepository.GetCourseTypesAsync()
                };
            }
            catch (HttpRequestException hre)
            {
                ModelState.AddModelError("", hre.Message);
            }

            return View("CourseForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (course.Id.Value == 0)
                    {
                        await _courseRepository.PostCourseAsync(course);
                    }
                    else
                    {
                        await _courseRepository.PutCourseAsync(course.Id.Value, course);
                    }

                    return RedirectToAction("Index");
                }
                catch (HttpRequestException hre)
                {
                    ModelState.AddModelError("", hre.Message);
                }
            }

            var viewModel = new CourseFormViewModel(course)
            {
                CourseTypes = await _courseTypeRepository.GetCourseTypesAsync()
            };

            return View("CourseForm", viewModel);
        }
    }
}