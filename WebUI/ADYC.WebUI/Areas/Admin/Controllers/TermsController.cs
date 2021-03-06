﻿using ADYC.API.ViewModels;
using ADYC.WebUI.Attributes;
using ADYC.WebUI.Controllers;
using ADYC.WebUI.CustomAttributes;
using ADYC.WebUI.Exceptions;
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
    [SelectedTab("terms")]
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

        // GET: Admin/Terms
        public async Task<ActionResult> Index()
        {
            var terms = await _termRepository.GetTerms();

            // Add properties to layout
            AddPageHeader("Terms", "List of all terms");

            AddBreadcrumb("Terms", "");

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

            // Add properties to layout
            AddPageHeader(viewModel.Title, "");

            AddBreadcrumb("Terms", Url.Action("Index", "Terms", new { area = "Admin" }));
            AddBreadcrumb(viewModel.Title, "");

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
            catch (BadRequestException bre)
            {
                AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
            }

            // Add properties to layout
            AddPageHeader(viewModel.Title, "");

            AddBreadcrumb("Terms", Url.Action("Index", "Terms", new { area = "Admin" }));
            AddBreadcrumb(viewModel.Title, "");

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

            AddBreadcrumb("Terms", Url.Action("Index", "Terms", new { area = "Admin" }));
            AddBreadcrumb(form.Title, "");

            return View("TermForm", form);
        }

        // GET: Admin/Terms/Delete
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
                var statusCode = await _termRepository.DeleteTerm(id.Value);

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
            catch (BadRequestException bre)
            {
                AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
            }

            // Add properties to layout
            AddPageHeader(viewModel.Title, "");

            AddBreadcrumb("Terms", Url.Action("Index", "Terms", new { area = "Admin" }));
            AddBreadcrumb(viewModel.Title, "");

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

                    AddPageAlerts(ViewHelpers.PageAlertType.Success, "Your changes have been saved succesfully.");

                    return RedirectToAction("PeriodDates", new { termId = form.TermId });
                }
                catch (BadRequestException bre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
                }
            }

            form.Term = await _termRepository.GetTermById(form.TermId);
            form.Periods = await _periodRepository.GetPeriods();

            // Add properties to layout
            AddPageHeader(form.Title, "");

            AddBreadcrumb("Terms", Url.Action("Index", "Terms", new { area = "Admin" }));
            AddBreadcrumb(form.Title, "");

            return View("PeriodDates", form);
        }

    }
}