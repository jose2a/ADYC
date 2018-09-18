using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class ProfessorsController : Controller
    {
        private ProfessorRepository _professorRepository;

        public ProfessorsController()
        {
            _professorRepository = new ProfessorRepository();
        }

        // GET: Professors
        public async Task<ActionResult> Index()
        {
            var professors = await _professorRepository.GetProfessorsAsync();

            return View(professors);
        }

        public ActionResult New()
        {
            var viewModel = new ProfessorFormViewModel
            {
                IsNew = true
            };

            return View("ProfessorForm", viewModel);
        }

        public async Task<ActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            ProfessorFormViewModel viewModel = null;

            try
            {
                var professor = await _professorRepository.GetProfessorAsync(id);

                viewModel = new ProfessorFormViewModel(professor)
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

            return View("ProfessorForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(ProfessorFormViewModel form)
        {
            form.IsNew = !form.Id.HasValue;

            if (ModelState.IsValid)
            {
                try
                {
                    Professor professor = (form.IsNew)
                        ? new Professor()
                        : await _professorRepository.GetProfessorAsync(form.Id.Value);

                    professor.FirstName = form.FirstName;
                    professor.LastName = form.LastName;
                    professor.Email = form.Email;
                    professor.CellphoneNumber = form.CellphoneNumber;

                    if (form.IsNew)
                    {
                        professor.Id = new System.Guid(); // Get this from Auth service
                        await _professorRepository.PostProfessorAsync(professor);
                    }
                    else
                    {
                        await _professorRepository.PutProfessorAsync(professor.Id, professor);
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

            return View("ProfessorForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var statusCode = await _professorRepository.DeleteProfessorAsync(id);

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
        public async Task<ActionResult> Trash(Guid id)
        {
            try
            {
                var statusCode = await _professorRepository.TrashProfessorAsync(id);

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

            return PartialView("pv_ProfessorRow", await _professorRepository.GetProfessorAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> Restore(Guid id)
        {
            try
            {
                var statusCode = await _professorRepository.RestoreProfessorAsync(id);

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

            return PartialView("pv_ProfessorRow", await _professorRepository.GetProfessorAsync(id));
        }
    }
}