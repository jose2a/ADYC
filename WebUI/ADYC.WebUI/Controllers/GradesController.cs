using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class GradesController : ADYCBasedController
    {
        private GradeRepository _gradeRepository;

        public GradesController()
        {
            _gradeRepository = new GradeRepository();
        }

        // GET: CourseTypes
        public async Task<ActionResult> Index()
        {
            var grades = await _gradeRepository.GetGrades();

            return View(grades);
        }

        public ActionResult New()
        {
            var viewModel = new GradeFormViewModel
            {
                IsNew = true
            };

            return View("GradeForm", viewModel);
        }

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
            catch (AdycHttpRequestException ahre)
            {
                if (ahre.StatusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
            }

            return View("GradeForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(GradeFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Grade grade = (form.IsNew)
                        ? new Grade()
                        : await _gradeRepository.GetGradeById(form.Id.Value);

                    grade.Name = form.Name;

                    if (form.IsNew)
                    {
                        await _gradeRepository.PostGrade(grade);
                    }
                    else
                    {
                        await _gradeRepository.PutGrade(grade.Id, grade);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            return View("GradeForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _gradeRepository.DeleteGrade(id);

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
    }
}