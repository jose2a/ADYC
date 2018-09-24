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
    public class TermsController : ADYCBasedController
    {
        private TermRepository _termRepository;

        public TermsController()
        {
            _termRepository = new TermRepository();
        }

        // GET: Terms
        public async Task<ActionResult> Index()
        {
            var terms = await _termRepository.GetTermsAsync();

            return View(terms);
        }

        public ActionResult New()
        {
            var viewModel = new TermFormViewModel
            {
                IsNew = true,
                StartDate = null,
                EndDate = null,
                EnrollmentDeadLine = null,
                EnrollmentDropDeadLine = null
            };

            return View("TermForm", viewModel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            TermFormViewModel viewModel = null;

            try
            {
                var term = await _termRepository.GetTermAsync(id.Value);

                viewModel = new TermFormViewModel(term)
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

                ProcessAdycHttpException(ahre, ModelState);
            }

            return View("TermForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(TermFormViewModel form)
        {
            form.IsNew = form.Id == null;

            if (ModelState.IsValid)
            {
                try
                {
                    Term term = (form.IsNew)
                        ? new Term()
                        : await _termRepository.GetTermAsync(form.Id.Value);

                    term.Name = form.Name;
                    term.StartDate = form.StartDate.Value;
                    term.EndDate = form.EndDate.Value;
                    term.EnrollmentDeadLine = form.EnrollmentDeadLine.Value;
                    term.EnrollmentDropDeadLine = form.EnrollmentDropDeadLine.Value;
                    term.IsCurrentTerm = form.IsCurrentTerm;

                    if (form.IsNew)
                    {
                        await _termRepository.PostTermAsync(term);
                    }
                    else
                    {
                        await _termRepository.PutTermAsync(term.Id, term);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    ProcessAdycHttpException(ahre, ModelState);
                }
            }

            return View("TermForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _termRepository.DeleteTermAsync(id);

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