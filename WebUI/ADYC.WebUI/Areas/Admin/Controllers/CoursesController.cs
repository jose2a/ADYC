using ADYC.API.ViewModels;
using ADYC.WebUI.Attributes;
using ADYC.WebUI.Controllers;
using ADYC.WebUI.CustomAttributes;
using ADYC.WebUI.Exceptions;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "AppAdmin")]
    [SelectedTab("courses")]
    public class CoursesController : ADYCBasedController
    {
        private CourseRepository _courseRepository;
        private CourseTypeRepository _courseTypeRepository;

        public CoursesController()
        {
            _courseRepository = new CourseRepository();
            _courseTypeRepository = new CourseTypeRepository();
        }

        // GET: Admin/Courses
        public async Task<ActionResult> Index()
        {
            var courses = await _courseRepository.GetCourses();

            // Add properties to layout
            AddPageHeader("Courses", "List of all courses");

            AddBreadcrumb("Courses", "");

            return View(courses);
        }

        // GET: Admin/Courses/New
        public async Task<ActionResult> New()
        {
            var viewModel = new CourseFormViewModel
            {
                IsNew = true,
                CourseTypes = await _courseTypeRepository.GetCourseTypes()
            };

            // Add properties to layout
            AddPageHeader(viewModel.Title, "");

            AddBreadcrumb("Courses", Url.Action("Index", "Courses", new { area = "Admin" }));
            AddBreadcrumb(viewModel.Title, "");

            return View("CourseForm", viewModel);
        }

        // GET: Admin/Courses/Edit
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
            catch (BadRequestException bre)
            {
                AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
            }

            // Add properties to layout
            AddPageHeader(viewModel.Title, "");

            AddBreadcrumb("Courses", Url.Action("Index", "Courses", new { area = "Admin" }));
            AddBreadcrumb(viewModel.Title, "");

            return View("CourseForm", viewModel);
        }

        // POST: Admin/Courses/Save
        [HttpPost]
        public async Task<ActionResult> Save(CourseFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CourseDto course = (form.IsNew)
                        ? new CourseDto()
                        : await _courseRepository.GetCourseById(form.Id.Value);

                    course.Name = form.Name;
                    course.CourseTypeId = form.CourseTypeId;

                    if (form.IsNew)
                    {
                        await _courseRepository.PostCourse(course);
                    }
                    else
                    {
                        await _courseRepository.PutCourse(course.Id.Value, course);
                    }

                    AddPageAlerts(ViewHelpers.PageAlertType.Success, "Your changes have been saved succesfully.");

                    return RedirectToAction("Index");
                }
                catch (BadRequestException bre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
                }
            }

            form.CourseTypes = await _courseTypeRepository.GetCourseTypes();

            // Add properties to layout
            AddPageHeader(form.Title, "");

            AddBreadcrumb("Courses", Url.Action("Index", "Courses", new { area = "Admin" }));
            AddBreadcrumb(form.Title, "");

            return View("CourseForm", form);
        }

        // GET: Admin/Courses/Delete
        [HttpGet]
        [OnlyAjaxRequest]
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            try
            {
                var statusCode = await _courseRepository.DeleteCourse(id.Value);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (BadRequestException bre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(bre);

                return new HttpStatusCodeResult(bre.StatusCode, errorString);
            }
        }

        // GET: Admin/Courses/Trash
        [HttpGet]
        [OnlyAjaxRequest]
        public async Task<ActionResult> Trash(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            try
            {
                var statusCode = await _courseRepository.TrashCourse(id.Value);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
            }
            catch (BadRequestException bre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(bre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }

            return PartialView("pv_CourseRow", await _courseRepository.GetCourseById(id.Value));
        }

        // GET: Admin/Courses/Restore
        [HttpGet]
        [OnlyAjaxRequest]
        public async Task<ActionResult> Restore(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            try
            {
                var statusCode = await _courseRepository.RestoreCourse(id.Value);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
            }
            catch (BadRequestException bre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(bre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }

            return PartialView("pv_CourseRow", await _courseRepository.GetCourseById(id.Value));
        }
    }
}