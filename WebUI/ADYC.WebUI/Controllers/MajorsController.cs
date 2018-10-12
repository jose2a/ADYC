using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class MajorsController : ADYCBasedController
    {
        private MajorRepository _majorRepository;

        public MajorsController()
        {
            _majorRepository = new MajorRepository();
        }

        // GET: CourseTypes
        public async Task<ActionResult> Index()
        {
            var majors = await _majorRepository.GetMajors();

            return View(majors);
        }

        public ActionResult New()
        {
            var viewModel = new MajorFormViewModel
            {
                IsNew = true
            };

            return View("MajorForm", viewModel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            MajorFormViewModel viewModel = null;

            try
            {
                var major = await _majorRepository.GetMajorById(id.Value);

                viewModel = new MajorFormViewModel(major)
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

            return View("MajorForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(MajorFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Major major = (form.IsNew)
                        ? new Major()
                        : await _majorRepository.GetMajorById(form.Id.Value);

                    major.Name = form.Name;

                    if (form.IsNew)
                    {
                        await _majorRepository.PostMajor(major);
                    }
                    else
                    {
                        await _majorRepository.PutMajor(major.Id, major);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            return View("MajorForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _majorRepository.DeleteGrade(id);

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