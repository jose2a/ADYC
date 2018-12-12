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

            // Add properties to layout
            AddPageHeader("Offerings (Terms)", "List of terms");

            AddBreadcrumb("Offerings (Terms)", "");

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


            // Add properties to layout
            AddPageHeader("Offerings (List)", "List of terms");

            AddBreadcrumb("Offerings (Terms)", Url.Action("Index"));
            AddBreadcrumb("Offerings (List)", "");

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

            // Add properties to layout
            AddPageHeader("Enrollments", "");

            AddBreadcrumb("Offerings (Terms)", Url.Action("Index"));
            AddBreadcrumb("Offerings (List)", Url.Action("View", new { TermId = offering.TermId }));
            AddBreadcrumb("Enrollments", "");

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

            // Add properties to layout
            AddPageHeader("Evaluations", "");

            AddBreadcrumb("Offerings (Terms)", Url.Action("Index"));
            AddBreadcrumb("Offerings (List)", Url.Action("View", new { TermId = viewModel.Enrollment.Offering.TermId }));
            AddBreadcrumb("Enrollments", Url.Action("ViewEnrollments", new { offeringId = viewModel.Enrollment.OfferingId }));
            AddBreadcrumb("Evaluations", "");

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

                    AddPageAlerts(ViewHelpers.PageAlertType.Success, "Your changes have been saved succesfully.");

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

            // Add properties to layout
            AddPageHeader("Evaluations", "");

            AddBreadcrumb("Offerings (Terms)", Url.Action("Index"));
            AddBreadcrumb("Offerings (List)", Url.Action("View", new { TermId = form.Enrollment.Offering.TermId }));
            AddBreadcrumb("Enrollments", Url.Action("ViewEnrollments", new { offeringId = form.Enrollment.OfferingId }));
            AddBreadcrumb("Evaluations", "");

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

            // Add properties to layout
            AddPageHeader(viewModel.Title, "");

            AddBreadcrumb("Offerings (Terms)", Url.Action("Index"));
            AddBreadcrumb("Offerings (List)", Url.Action("View", new { TermId = viewModel.Offering.TermId }));
            AddBreadcrumb("Enrollments", Url.Action("ViewEnrollments", new { offeringId = viewModel.OfferingId }));
            AddBreadcrumb(viewModel.Title, "");

            return View(viewModel);
        }
    }
}