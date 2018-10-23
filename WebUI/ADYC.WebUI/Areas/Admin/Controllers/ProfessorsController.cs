using ADYC.API.ViewModels;
using ADYC.WebUI.Controllers;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Admin.Controllers
{
    public class ProfessorsController : ADYCBasedController
    {
        private ProfessorRepository _professorRepository;

        public ProfessorsController()
        {
            _professorRepository = new ProfessorRepository();
        }

        // GET: Admin/Professors
        public async Task<ActionResult> Index()
        {
            var professors = await _professorRepository.GetProfessors();

            return View(professors);
        }

        // GET: Admin/Professors/New
        public ActionResult New()
        {
            var viewModel = new ProfessorFormViewModel
            {
                IsNew = true
            };

            return View("ProfessorForm", viewModel);
        }

        // GET: Admin/Professors/Edit
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            ProfessorFormViewModel viewModel = null;

            try
            {
                var professor = await _professorRepository.GetProfessorById(id.Value);

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

                AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
            }

            return View("ProfessorForm", viewModel);
        }

        // POST: Admin/Professors/Save
        [HttpPost]
        public async Task<ActionResult> Save(ProfessorFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ProfessorDto professor = (form.IsNew)
                        ? new ProfessorDto()
                        : await _professorRepository.GetProfessorById(form.Id.Value);

                    professor.FirstName = form.FirstName;
                    professor.LastName = form.LastName;
                    professor.Email = form.Email;
                    professor.CellphoneNumber = form.CellphoneNumber;

                    if (form.IsNew)
                    {
                        professor.Id = new Guid(); // Get this from Auth service
                        await _professorRepository.PostProfessor(professor);
                    }
                    else
                    {
                        await _professorRepository.PutProfessor(professor.Id, professor);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            return View("ProfessorForm", form);
        }

        // GET: Admin/Professors/Delete
        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var statusCode = await _professorRepository.DeleteProfessor(id);

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

        // GET: Admin/Professors/Trash
        [HttpGet]
        public async Task<ActionResult> Trash(Guid id)
        {
            try
            {
                var statusCode = await _professorRepository.TrashProfessor(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(ahre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }

            return PartialView("pv_ProfessorRow", await _professorRepository.GetProfessorById(id));
        }

        // GET: Admin/Professors/Restore
        [HttpGet]
        public async Task<ActionResult> Restore(Guid id)
        {
            try
            {
                var statusCode = await _professorRepository.RestoreProfessor(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(ahre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }

            return PartialView("pv_ProfessorRow", await _professorRepository.GetProfessorById(id));
        }
    }
}