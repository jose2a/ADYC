using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class MajorsController : Controller
    {
        private MajorRepository _majorRepository;

        public MajorsController()
        {
            _majorRepository = new MajorRepository();
        }

        // GET: CourseTypes
        public async Task<ActionResult> Index()
        {
            var majors = await _majorRepository.GetMajorAsync();

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
                var major = await _majorRepository.GetMajorAsync(id.Value);

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

                foreach (var error in ahre.Errors)
                {
                    ModelState.AddModelError("", ahre.Message);
                }
            }

            return View("MajorForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(MajorFormViewModel form)
        {
            form.IsNew = form.Id == null;

            if (ModelState.IsValid)
            {
                try
                {
                    Major major = (form.IsNew) 
                        ? new Major()
                        : await _majorRepository.GetMajorAsync(form.Id.Value);

                    major.Name = form.Name;

                    if (form.IsNew)
                    {
                        await _majorRepository.PostMajorAsync(major);
                    }
                    else
                    {
                        await _majorRepository.PutMajorAsync(major.Id, major);
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

            return View("MajorForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _majorRepository.DeleteGradeAsync(id);

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