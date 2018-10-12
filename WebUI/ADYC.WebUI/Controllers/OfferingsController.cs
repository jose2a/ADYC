using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class OfferingsController : ADYCBasedController
    {
        private TermRepository _termRepository;
        private OfferingRepository _offeringRepository;
        private CourseRepository _courseRepository;
        private ProfessorRepository _professorRepository;
        private ScheduleRepository _scheduleRepository;

        public OfferingsController()
        {
            _termRepository = new TermRepository();
            _offeringRepository = new OfferingRepository();
            _courseRepository = new CourseRepository();
            _professorRepository = new ProfessorRepository();
            _scheduleRepository = new ScheduleRepository();
        }

        // GET: Offerings
        public async Task<ActionResult> Index()
        {
            var terms = await _termRepository.GetTerms();

            return View(terms);
        }

        public async Task<ActionResult> View(int? termId)
        {
            if (!termId.HasValue)
            {
                return HttpNotFound();
            }

            var term = await _termRepository.GetTermById(termId.Value);

            if (term == null)
            {
                return HttpNotFound();
            }

            var offerings = _offeringRepository.GetOfferingsByTermId(termId.Value);

            return View(new OfferingListViewModel
            {
                Term = term,
                Offerings = offerings
            });
        }

        [HttpPost]
        public async Task<ActionResult> New(ShowOfferingFormViewModel form)
        {
            if (!form.TermId.HasValue)
            {
                return HttpNotFound();
            }

            var professors = await _professorRepository.GetNotTrashedProfessors();

            var viewModel = new OfferingFormViewModel
            {
                IsNew = true,
                TermId = form.TermId.Value,
                Courses = await _courseRepository.GetNotTrashedCourses(),
                Professors = professors,
                Terms = await _termRepository.GetTerms()
            };

            return View("OfferingForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ShowOfferingFormViewModel form)
        {
            if (!form.TermId.HasValue)
            {
                return HttpNotFound();
            }

            OfferingFormViewModel viewModel = null;

            try
            {
                var offering = await _offeringRepository.GetOfferingById(form.OfferingId.Value);

                viewModel = new OfferingFormViewModel(offering)
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

            viewModel.Courses = await _courseRepository.GetNotTrashedCourses();
            viewModel.Professors = await _professorRepository.GetNotTrashedProfessors();
            viewModel.Terms = await _termRepository.GetTerms();

            return View("OfferingForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(OfferingFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Offering offering = (form.IsNew)
                        ? new Offering()
                        : await _offeringRepository.GetOfferingById(form.Id.Value);

                    offering.Title = form.Title;
                    offering.Location = form.Location;
                    offering.OfferingDays = form.OfferingDays; // Change later maybe
                    offering.Notes = form.Notes;
                    offering.ProfessorId = form.ProfessorId;
                    offering.CourseId = form.CourseId;
                    offering.TermId = form.TermId;

                    if (form.IsNew)
                    {
                        await _offeringRepository.PostOffering(offering);
                    }
                    else
                    {
                        await _offeringRepository.PutOffering(offering.Id, offering);
                    }

                    return RedirectToAction("View", new { termId = form.TermId });
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            form.Courses = await _courseRepository.GetNotTrashedCourses();
            form.Professors = await _professorRepository.GetNotTrashedProfessors();
            form.Terms = await _termRepository.GetTerms();

            return View("OfferingForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id, bool force)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            try
            {
                var statusCode = await _offeringRepository.DeleteOffering(id.Value, force);

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

        public async Task<ActionResult> Schedules(int? offeringId)
        {
            if (!offeringId.HasValue)
            {
                return HttpNotFound();
            }

            ScheduleListViewModel viewModel = null;

            try
            {
                var scheduleList = await _scheduleRepository.GetSchedulesByOfferingId(offeringId.Value);
                var offering = scheduleList.Offering;
                var days = GetDayEnumViewModelList();

                var schedules = GetScheduleList(offeringId.Value, scheduleList.Schedules, days);

                viewModel = new ScheduleListViewModel(offering, schedules);
                viewModel.IsNew = (scheduleList.Schedules == null || scheduleList.Schedules.Count() == 0);
                viewModel.Days = days;
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

        [HttpPost]
        public async Task<ActionResult> SaveSchedules(ScheduleListViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    form.Schedules = form.Schedules.Where(s => s.StartTime.HasValue && s.EndTime.HasValue).ToList();

                    if (form.IsNew)
                    {
                        await _scheduleRepository.PostScheduleList(form);
                    }
                    else
                    {
                        await _scheduleRepository.PutScheduleList(form.OfferingId, form);
                    }

                    return RedirectToAction("View", new { termId = form.Offering.TermId });
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            var days = GetDayEnumViewModelList();
            var offering = await _offeringRepository.GetOfferingById(form.OfferingId);

            form.Offering = offering;
            form.Schedules = GetScheduleList(offering.Id, form.Schedules, days);
            form.Days = days;

            return View("Schedules", form);
        }

        private static List<DayEnumViewModel> GetDayEnumViewModelList()
        {
            return ((IEnumerable<Day>)Enum.GetValues(typeof(Day))).Select(c => new DayEnumViewModel() { Id = (byte)c, Name = c.ToString() }).ToList();
        }

        private List<ScheduleViewModel> GetScheduleList(int offeringId, List<ScheduleViewModel> scheduleViewModelList, List<DayEnumViewModel> days)
        {
            var scheduleList = new List<ScheduleViewModel>();
                foreach (var d in days)
                {
                    var sch = scheduleViewModelList.SingleOrDefault(s => s.Day == d.Id);

                    if (sch != null)
                    {
                        scheduleList.Add(sch);
                    }
                    else
                    {
                        scheduleList.Add(new ScheduleViewModel
                        {
                            OfferingId = offeringId,
                            Day = d.Id,
                            StartTime = null,
                            EndTime = null
                        });
                    }
                }

            return scheduleList;
        }
    }
}