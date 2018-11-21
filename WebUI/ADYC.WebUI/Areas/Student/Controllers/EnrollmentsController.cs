using ADYC.WebUI.Controllers;
using ADYC.WebUI.CustomAttributes;
using ADYC.WebUI.Exceptions;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
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

        public EnrollmentsController()
        {
            _termRepository = new TermRepository();
            _enrollmentRepository = new EnrollmentRepository();
            _evaluationRepository = new EvaluationRepository();
            _offeringRepository = new OfferingRepository();
        }

        // GET: Student/Enrollments
        public async Task<ActionResult> Index()
        {
            var terms = await _termRepository.GetTerms();

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

                return View(enrollment.Offering);                
            }
            catch (BadRequestException bre)
            {
                TempData["errorMsg"] = GetErrorsFromAdycHttpExceptionToString(bre);
            }

            return RedirectToAction("ViewEnrollment", new { termId = termId });
        }
    }
}