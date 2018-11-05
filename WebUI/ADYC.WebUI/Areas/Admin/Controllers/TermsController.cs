using ADYC.API.ViewModels;
using ADYC.WebUI.Controllers;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "AppAdmin")]
    public class TermsController : ADYCBasedController
    {
        private TermRepository _termRepository;
        private PeriodRepository _periodRepository;
        private PeriodDateRepository _periodDateRepository;

        public TermsController()
        {
            var user = ((CustomPrincipal)User);

            _termRepository = new TermRepository();
            _periodRepository = new PeriodRepository();
            _periodDateRepository = new PeriodDateRepository();
        }

        // GET: Admin/Terms
        public async Task<ActionResult> Index()
        {
            var terms = await _termRepository.GetTerms();

            return View(terms);
        }

        // GET: Admin/Terms/New
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

        // GET: Admin/Terms/Edit
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            TermFormViewModel viewModel = null;

            try
            {
                var term = await _termRepository.GetTermById(id.Value);

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

                AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
            }

            return View("TermForm", viewModel);
        }

        // POST: Admin/Terms/Save
        [HttpPost]
        public async Task<ActionResult> Save(TermFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TermDto term = (form.IsNew)
                        ? new TermDto()
                        : await _termRepository.GetTermById(form.Id.Value);

                    term.Name = form.Name;
                    term.StartDate = form.StartDate.Value;
                    term.EndDate = form.EndDate.Value;
                    term.EnrollmentDeadLine = form.EnrollmentDeadLine.Value;
                    term.EnrollmentDropDeadLine = form.EnrollmentDropDeadLine.Value;
                    term.IsCurrentTerm = form.IsCurrentTerm;

                    if (form.IsNew)
                    {
                        await _termRepository.PostTerm(term);
                    }
                    else
                    {
                        await _termRepository.PutTerm(term.Id.Value, term);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            return View("TermForm", form);
        }

        // GET: Admin/Terms/Delete
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _termRepository.DeleteTerm(id);

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

        // GET: Admin/Terms/PeriodDates
        public async Task<ActionResult> PeriodDates(int? termId)
        {
            if (!termId.HasValue)
            {
                return HttpNotFound();
            }

            PeriodDateListViewModel viewModel = null;

            try
            {
                var periodDateList = await _periodDateRepository.GetPeriodDatesByTermId(termId.Value);
                var periods = await _periodRepository.GetPeriods();

                List<PeriodDateDto> periodDates = null;
                bool isNew = false;

                if (periodDateList.PeriodDates == null || periodDateList.PeriodDates.ToList().Count == 0)
                {
                    isNew = true;
                    periodDates = new List<PeriodDateDto>();

                    foreach (var p in periods)
                    {
                        periodDates.Add(new PeriodDateDto
                        {
                            PeriodId = p.Id.Value,
                            TermId = periodDateList.Term.Id.Value,
                            StartDate = null,
                            EndDate = null
                        });
                    }
                }
                else
                {
                    isNew = false;
                    periodDates = periodDateList.PeriodDates.ToList();
                }

                viewModel = new PeriodDateListViewModel(periodDateList)
                {
                    Periods = periods,
                    IsNew = isNew
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

            return View(viewModel);
        }

        // POST: Admin/Terms/SavePeriodDates
        [HttpPost]
        public async Task<ActionResult> SavePeriodDates(PeriodDateListViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var periodDateList = new PeriodDateListDto
                    {
                        PeriodDates = form.PeriodDates
                    };

                    if (form.IsNew)
                    {
                        await _periodDateRepository.PostPeriodDateList(periodDateList);
                    }
                    else
                    {
                        await _periodDateRepository.PutPeriodDateList(form.TermId, periodDateList);
                    }

                    TempData["msg"] = "Your changes have been saved succesfully.";

                    return RedirectToAction("PeriodDates", new { termId = form.TermId });
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            form.Term = await _termRepository.GetTermById(form.TermId);
            form.Periods = await _periodRepository.GetPeriods();

            return View("PeriodDates", form);
        }

    }
}