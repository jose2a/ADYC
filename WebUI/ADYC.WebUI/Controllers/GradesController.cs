using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class GradesController : Controller
    {
        private GradeRepository _gradeRepository;

        public GradesController()
        {
            _gradeRepository = new GradeRepository();
        }

        // GET: CourseTypes
        public async Task<ActionResult> Index()
        {
            var grades = await _gradeRepository.GetGradesAsync();

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
                var grade = await _gradeRepository.GetGradeAsync(id.Value);

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

                foreach (var error in ahre.Errors)
                {
                    ModelState.AddModelError("", ahre.Message);
                }
            }

            return View("GradeForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(GradeFormViewModel form)
        {
            form.IsNew = form.Id == null;

            if (ModelState.IsValid)
            {
                try
                {
                    Grade grade = (form.IsNew) 
                        ? new Grade() 
                        : await _gradeRepository.GetGradeAsync(form.Id.Value);

                    grade.Name = form.Name;

                    if (form.IsNew)
                    {
                        await _gradeRepository.PostGradeAsync(grade);
                    }
                    else
                    {
                        await _gradeRepository.PutGradeAsync(grade.Id, grade);
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

            return View("GradeForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _gradeRepository.DeleteGradeAsync(id);

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
    }
}