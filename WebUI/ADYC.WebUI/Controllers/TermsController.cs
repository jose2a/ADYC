using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class TermsController : ADYCBasedController
    {
        private TermRepository _termRepository;
        private PeriodRepository _periodRepository;
        private PeriodDateRepository _periodDateRepository;

        public TermsController()
        {
            _termRepository = new TermRepository();
            _periodRepository = new PeriodRepository();
            _periodDateRepository = new PeriodDateRepository();
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

        public async Task<ActionResult> PeriodDates(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            PeriodDateListViewModel viewModel = null;

            try
            {
                var periodDatesList = await _periodDateRepository.GetPeriodDatesForTerm(id.Value);
                var term = periodDatesList.Term;
                var periods = await _periodRepository.GetPeriodAsync();

                List<PeriodDateViewModel> periodDates = null;
                bool isNew = false;

                if (periodDatesList.PeriodDates == null || periodDatesList.PeriodDates.Count == 0)
                {
                    isNew = true;
                    periodDates = new List<PeriodDateViewModel>();

                    foreach (var p in periods)
                    {
                        periodDates.Add(new PeriodDateViewModel
                        {
                            PeriodId = p.Id,
                            TermId = term.Id,
                            StartDate = null,
                            EndDate = null
                        });
                    }
                }
                else
                {
                    isNew = false;
                    periodDates = periodDatesList.PeriodDates;
                }

                viewModel = new PeriodDateListViewModel(id.Value, term, periodDates)
                {
                    Periods = await _periodRepository.GetPeriodAsync(),
                    IsNew = isNew
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

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SavePeriodDates(PeriodDateListViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (form.IsNew)
                    {
                        await _periodDateRepository.PostPeriodDateAsync(form);
                    }
                    else
                    {
                        await _periodDateRepository.PutPeriodDateAsync(form.TermId, form);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    ProcessAdycHttpException(ahre, ModelState);
                }
            }

            form.Term = await _termRepository.GetTermAsync(form.TermId);
            form.Periods = await _periodRepository.GetPeriodAsync();

            return View("PeriodDates", form);
        }

    }
}