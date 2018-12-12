using ADYC.API.ViewModels;
using ADYC.WebUI.Attributes;
using ADYC.WebUI.Controllers;
using ADYC.WebUI.CustomAttributes;
using ADYC.WebUI.Exceptions;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "AppAdmin")]
    [SelectedTab("grades")]
    public class GradesController : ADYCBasedController
    {
        private GradeRepository _gradeRepository;

        public GradesController()
        {
            _gradeRepository = new GradeRepository();
        }

        // GET: Admin/Grades
        public async Task<ActionResult> Index()
        {
            var grades = await _gradeRepository.GetGrades();

            // Add properties to layout
            AddPageHeader("Grades", "List of all grades");

            AddBreadcrumb("Grades", "");

            return View(grades);
        }

        // GET: Admin/Grades/New
        public ActionResult New()
        {
            var viewModel = new GradeFormViewModel
            {
                IsNew = true
            };

            // Add properties to layout
            AddPageHeader(viewModel.Title, "");

            AddBreadcrumb("Grades", Url.Action("Index", "Grades", new { area = "Admin" }));
            AddBreadcrumb(viewModel.Title, "");

            return View("GradeForm", viewModel);
        }

        // GET: Admin/Grades/Edit
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            GradeFormViewModel viewModel = null;

            try
            {
                var grade = await _gradeRepository.GetGradeById(id.Value);

                viewModel = new GradeFormViewModel(grade)
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

            AddBreadcrumb("Grades", Url.Action("Index", "Grades", new { area = "Admin" }));
            AddBreadcrumb(viewModel.Title, "");

            return View("GradeForm", viewModel);
        }

        // POST: Admin/Grades/Save
        [HttpPost]
        public async Task<ActionResult> Save(GradeFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GradeDto grade = (form.IsNew)
                        ? new GradeDto()
                        : await _gradeRepository.GetGradeById(form.Id.Value);

                    grade.Name = form.Name;

                    if (form.IsNew)
                    {
                        await _gradeRepository.PostGrade(grade);
                    }
                    else
                    {
                        await _gradeRepository.PutGrade(grade.Id.Value, grade);
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

            AddBreadcrumb("Grades", Url.Action("Index", "Grades", new { area = "Admin" }));
            AddBreadcrumb(form.Title, "");

            return View("GradeForm", form);
        }

        // GET: Admin/Grades/Delete
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
                var statusCode = await _gradeRepository.DeleteGrade(id.Value);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (BadRequestException bre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(bre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }
        }
    }
}