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
    [SelectedTab("courseTypes")]
    public class CourseTypesController : ADYCBasedController
    {
        private CourseTypeRepository _courseTypeRepository;

        public CourseTypesController()
        {
            _courseTypeRepository = new CourseTypeRepository();
        }

        // GET: Admin/CourseTypes
        public async Task<ActionResult> Index()
        {
            var courseTypes = await _courseTypeRepository.GetCourseTypes();

            // Add properties to layout
            AddPageHeader("Course Types", "List of all course types");

            AddBreadcrumb("Course Types", "");

            return View(courseTypes);
        }

        // GET: Admin/CourseTypes/New
        public ActionResult New()
        {
            var viewModel = new CourseTypeFormViewModel
            {
                IsNew = true
            };

            // Add properties to layout
            AddPageHeader(viewModel.Title, "");

            AddBreadcrumb("Course Types", Url.Action("Index", "CourseTypes", new { area = "Admin" }));
            AddBreadcrumb(viewModel.Title, "");

            return View("CourseTypeForm", viewModel);
        }

        // GET: Admin/CourseTypes/Edit
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            CourseTypeFormViewModel viewModel = null;

            try
            {
                var courseType = await _courseTypeRepository.GetCourseTypeById(id.Value);

                viewModel = new CourseTypeFormViewModel(courseType)
                {
                    IsNew = false
                };
            }
            catch (BadRequestException bre)
            {
                AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
            }

            // Add properties to layout
            AddPageHeader(viewModel.Title, "");

            AddBreadcrumb("Course Types", Url.Action("Index", "CourseTypes", new { area = "Admin" }));
            AddBreadcrumb(viewModel.Title, "");

            return View("CourseTypeForm", viewModel);
        }

        // POST: Admin/CourseTypes/Save
        [HttpPost]
        public async Task<ActionResult> Save(CourseTypeFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CourseTypeDto courseType = (form.IsNew)
                        ? new CourseTypeDto()
                        : await _courseTypeRepository.GetCourseTypeById(form.Id.Value);

                    courseType.Name = form.Name;

                    if (form.IsNew)
                    {
                        await _courseTypeRepository.PostCourseType(courseType);
                    }
                    else
                    {
                        await _courseTypeRepository.PutCourseType(courseType.Id.Value, courseType);
                    }

                    AddPageAlerts(ViewHelpers.PageAlertType.Success, "Your changes have been saved succesfully.");

                    return RedirectToAction("Index");
                }
                catch (BadRequestException bre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
                }
            }

            // Add properties to layout
            AddPageHeader(form.Title, "");

            AddBreadcrumb("Course Types", Url.Action("Index", "CourseTypes", new { area = "Admin" }));
            AddBreadcrumb(form.Title, "");

            return View("CourseTypeForm", form);
        }

        // GET: Admin/CourseTypes/Delete
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
                var statusCode = await _courseTypeRepository.DeleteCourseType(id.Value);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (BadRequestException ahre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(ahre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }
        }
    }
}