using ADYC.WebUI.Controllers;
using ADYC.WebUI.CustomAttributes;
using ADYC.WebUI.Exceptions;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Student.Controllers
{
    [Authorize(Roles = "AppStudent")]
    [SelectedTab("enrollments")]
    public class EnrollmentsController : ADYCBasedController
    {
        private TermRepository _termRepository;
        private EnrollmentRepository _enrollmentRepository;
        private EvaluationRepository _evaluationRepository;
        private OfferingRepository _offeringRepository;
        private ScheduleRepository _scheduleRepository;

        public EnrollmentsController()
        {
            _termRepository = new TermRepository();
            _enrollmentRepository = new EnrollmentRepository();
            _evaluationRepository = new EvaluationRepository();
            _offeringRepository = new OfferingRepository();
            _scheduleRepository = new ScheduleRepository();
        }

        // GET: Student/Enrollments
        public async Task<ActionResult> Index()
        {
            var terms = await _termRepository.GetTerms();

            // Add properties to layout
            AddPageHeader("Enrollments (Terms)", "");

            AddBreadcrumb("Enrollments (Terms)", "");

            return View(terms);
        }

        // GET: Student/Enrollments/ViewEnrollment
        public async Task<ActionResult> ViewEnrollment(int? termId)
        {
            if (!termId.HasValue)
            {
                return HttpNotFound();
            }

            try
            {
                var studentId = SessionHelper.User.UserId;

                var enrollments = await _enrollmentRepository.GetEnrollmentsByStudentIdAndTermId(studentId, termId.Value);

                var term = await _termRepository.GetTermById(termId.Value);

                // Add properties to layout
                AddPageHeader("Enrollment", "");

                AddBreadcrumb("Enrollments (Terms)", UrlHelper.Action("Index", "Enrollments", new { area = "Student" }));
                AddBreadcrumb("Enrollment", "");

                return View(new EnrollmentDetailListViewModel
                {
                    TermId = termId.Value,
                    Term = term,
                    IsCurrentTerm = term.IsCurrentTerm,
                    Enrollments = enrollments
                });
            }
            catch (BadRequestException bre)
            {
                AddPageAlerts(ViewHelpers.PageAlertType.Error, GetErrorsFromAdycHttpExceptionToString(bre));

                TempData["errorMsg"] = GetErrorsFromAdycHttpExceptionToString(bre);
            }

            return RedirectToAction("Index");
        }

        // GET: Student/Enrollments/ViewEvaluations
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
            AddPageHeader("Your schedule", "Your schedule for this offering.");

            AddBreadcrumb("Enrollments (Terms)", UrlHelper.Action("Index", "Enrollments", new { area = "Student" }));
            AddBreadcrumb("Enrollment", UrlHelper.Action("ViewEnrollment", "Enrollments", new { area = "Student", termId = viewModel.Enrollment.Offering.TermId }));
            AddBreadcrumb("Evaluations", "");

            return View(viewModel);
        }

        // GET: Student/Enrollments/Withdraw
        public async Task<ActionResult> Withdraw(int? enrollmentId, int? termId)
        {
            if (!enrollmentId.HasValue)
            {
                return HttpNotFound();
            }

            if (!termId.HasValue)
            {
                return HttpNotFound();
            }

            try
            {
                var term = await _termRepository.GetTermById(termId.Value);

                if (DateTime.Now < term.EnrollmentDropDeadLine.AddDays(1))
                {
                    TempData["warningMsg"] = "The period to withdraw has already ended. You cannot withdraw at this moment.";

                    return RedirectToAction("ViewEnrollment", new { termId = termId.Value });
                }
                await _enrollmentRepository.Withdraw(enrollmentId.Value);

                var enrollment = await _enrollmentRepository.GetById(enrollmentId.Value);

                // Add properties to layout
                AddPageHeader("Withdraw confirmed", "");

                AddBreadcrumb("Enrollments (Terms)", UrlHelper.Action("Index", "Enrollments", new { area = "Student" }));
                AddBreadcrumb("Enrollment", UrlHelper.Action("ViewEnrollment", "Enrollments", new { area = "Student", termId = termId.Value }));
                AddBreadcrumb("Withdraw confirmed", "");

                return View(enrollment.Offering);                
            }
            catch (BadRequestException bre)
            {
                TempData["errorMsg"] = GetErrorsFromAdycHttpExceptionToString(bre);
            }

            return RedirectToAction("ViewEnrollment", new { termId = termId });
        }

        // GET: Student/Enrollments/ViewSchedules
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

                // Add properties to layout
                AddPageHeader("Your schedule", "Your schedule for this offering.");

                AddBreadcrumb("Enrollments (Terms)", UrlHelper.Action("Index", "Enrollments", new { area = "Student" }));
                AddBreadcrumb("Enrollment", UrlHelper.Action("ViewEnrollment", "Enrollments", new { area = "Student", termId = viewModel.Offering.TermId }));//$"/Student/Enrollments/ViewEnrollment?termId={viewModel.Offering.TermId}");
                AddBreadcrumb("Your schedule", "");
            }
            catch (BadRequestException bre)
            {
                AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
            }

            return View(viewModel);
        }
    }
}