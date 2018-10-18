using ADYC.WebUI.Controllers;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Professor.Controllers
{
    public class EnrollmentsController : ADYCBasedController
    {
        private TermRepository _termRepository;
        private OfferingRepository _offeringRepository;
        private EnrollmentRepository _enrollmentRepository;
        private EvaluationRepository _evaluationRepository;
        private PeriodRepository _periodRepository;
        private ScheduleRepository _scheduleRepository;

        public EnrollmentsController()
        {
            _termRepository = new TermRepository();
            _offeringRepository = new OfferingRepository();
            _enrollmentRepository = new EnrollmentRepository();
            _evaluationRepository = new EvaluationRepository();
            _periodRepository = new PeriodRepository();
            _scheduleRepository = new ScheduleRepository();
        }

        // GET: Professor/Enrollments/
        public async Task<ActionResult> Index()
        {
            var terms = await _termRepository.GetTerms();

            return View(terms);
        }

        // GET: Professor/Enrollments/View
        public async Task<ActionResult> ViewOfferings(int? termId)
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

            var studentId = new Guid("63016919-365a-e811-9b75-b8763fed7266");

            var offerings = _offeringRepository.GetOfferingsByProfessorIdAndTermId(studentId, termId.Value);

            return View(new OfferingListViewModel
            {
                Term = term,
                Offerings = offerings
            });
        }

        // GET: Professor/Enrollments/Enrollments
        public async Task<ActionResult> ViewEnrollments(int? offeringId)
        {
            if (!offeringId.HasValue)
            {
                return HttpNotFound();
            }

            var offering = await _offeringRepository.GetOfferingById(offeringId.Value);

            if (offering == null)
            {
                return HttpNotFound();
            }

            var enrollments = await _enrollmentRepository.GetEnrollmentsByOfferingId(offeringId.Value);

            return View(
                new EnrollmentListViewModel
                {
                    Offering = offering,
                    Enrollments = enrollments,
                    IsCurrentTerm = offering.Term.IsCurrentTerm
                });
        }

        // GET: Professor/Enrollments/Evaluations
        public async Task<ActionResult> ViewEvaluations(int? enrollmentId)
        {
            if (!enrollmentId.HasValue)
            {
                return HttpNotFound();
            }

            var enrollmentWithEvaluations = await _evaluationRepository.GetWithEvaluations(enrollmentId.Value);

            if (enrollmentWithEvaluations.Enrollment == null)
            {
                return HttpNotFound();
            }

            enrollmentWithEvaluations.IsCurrentTerm = enrollmentWithEvaluations.Enrollment.Offering.Term.IsCurrentTerm;

            return View(enrollmentWithEvaluations);
        }

        // POST: Professor/Enrollments/SaveEvaluations
        [HttpPost]
        public async Task<ActionResult> SaveEvaluations(EnrollmentWithEvaluationsViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var enrollmentWithEvaluations = await _evaluationRepository.GetWithEvaluations(form.Enrollment.Id);
                    enrollmentWithEvaluations.Enrollment.Notes = form.Enrollment.Notes;
                    enrollmentWithEvaluations.Evaluations = form.Evaluations;

                    // Update enrollment
                    await _evaluationRepository.PutEnrollmentWithEvaluations(form.Enrollment.Id, enrollmentWithEvaluations);

                    TempData["msg"] = "Your changes were saved sucessfully";

                    return RedirectToAction("ViewEvaluations", new { enrollmentId = enrollmentWithEvaluations.Enrollment.Id });
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            form.Enrollment = await _enrollmentRepository.GetById(form.Enrollment.Id);

            foreach (var evaluation in form.Evaluations)
            {
                evaluation.Period = await _periodRepository.GetPeriodById(evaluation.PeriodId);
            }

            return View("ViewEvaluations", form);
        }

        public async Task<ActionResult> ViewSchedules(int? offeringId)
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
                viewModel.IsCurrentTerm = offering.Term.IsCurrentTerm;
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

        //private List<ScheduleViewModel> GetScheduleList(int offeringId, List<ScheduleViewModel> scheduleViewModelList, List<DayEnumViewModel> days)
        //{
        //    var scheduleList = new List<ScheduleViewModel>();
        //    foreach (var d in days)
        //    {
        //        var sch = scheduleViewModelList.SingleOrDefault(s => s.Day == d.Id);

        //        if (sch != null)
        //        {
        //            scheduleList.Add(sch);
        //        }
        //        else
        //        {
        //            scheduleList.Add(new ScheduleViewModel
        //            {
        //                OfferingId = offeringId,
        //                Day = d.Id,
        //                StartTime = null,
        //                EndTime = null
        //            });
        //        }
        //    }

        //    return scheduleList;
        //}

        //private static List<DayEnumViewModel> GetDayEnumViewModelList()
        //{
        //    return ((IEnumerable<Day>)Enum.GetValues(typeof(Day))).Select(c => new DayEnumViewModel() { Id = (byte)c, Name = c.ToString() }).ToList();
        //}
    }
}