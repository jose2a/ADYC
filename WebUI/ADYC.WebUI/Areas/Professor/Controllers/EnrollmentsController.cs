using ADYC.WebUI.Controllers;
using ADYC.WebUI.CustomAttributes;
using ADYC.WebUI.Exceptions;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Professor.Controllers
{
    [Authorize(Roles = "AppProfessor")]
    [SelectedTab("enrollments")]
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

        // GET: Professor/Enrollments/ViewOfferings
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

            var professorId = SessionHelper.User.UserId;

            var offerings = _offeringRepository.GetOfferingsByProfessorIdAndTermId(professorId, termId.Value);

            return View(new OfferingListViewModel
            {
                Term = term,
                Offerings = offerings
            });
        }

        // GET: Professor/Enrollments/ViewEnrollments
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
                    Enrollments = enrollments
                });
        }

        // GET: Professor/Enrollments/ViewEvaluations
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

            var viewModel = new EnrollmentWithEvaluationsViewModel(enrollmentWithEvaluations);

            return View(viewModel);
        }

        // POST: Professor/Enrollments/SaveEvaluations
        [HttpPost, ValidateInput(false)]
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

                    TempData["msg"] = "Your changes have been saved sucessfully.";

                    return RedirectToAction("ViewEvaluations", new { enrollmentId = enrollmentWithEvaluations.Enrollment.Id });
                }
                catch (BadRequestException bre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
                }
            }

            form.Enrollment = await _enrollmentRepository.GetById(form.Enrollment.Id);

            foreach (var evaluation in form.Evaluations)
            {
                evaluation.Period = await _periodRepository.GetPeriodById(evaluation.PeriodId);
            }

            return View("ViewEvaluations", form);
        }

        // GET: Professor/Enrollments/ViewSchedules
        public async Task<ActionResult> ViewSchedules(int? offeringId)
        {
            if (!offeringId.HasValue)
            {
                return HttpNotFound();
            }

            ScheduleListViewModel viewModel = null;

            try
            {
                var days = GetDayEnumViewModelList();

                var scheduleList = await _scheduleRepository.GetSchedulesByOfferingId(offeringId.Value);
                scheduleList.Schedules = GetScheduleList(offeringId.Value, scheduleList.Schedules.ToList(), days);

                viewModel = new ScheduleListViewModel(scheduleList);
                viewModel.Days = days;
            }
            catch (BadRequestException bre)
            {
                AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
            }

            return View(viewModel);
        }
    }
}