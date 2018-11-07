using ADYC.WebUI.Controllers;
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
                var studentId = SessionHelper.User().UserId; //new Guid("65016919-365A-E811-9B75-B8763FED7266");

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
            catch (AdycHttpRequestException ahre)
            {
                TempData["msg"] = GetErrorsFromAdycHttpExceptionToString(ahre);
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
                await _enrollmentRepository.Withdraw(enrollmentId.Value);

                var enrollment = await _enrollmentRepository.GetById(enrollmentId.Value);

                return View(enrollment.Offering);                
            }
            catch (AdycHttpRequestException ahre)
            {
                if (ahre.StatusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                var errorMsg = GetErrorsFromAdycHttpExceptionToString(ahre);

                TempData["msg"] = errorMsg;
            }

            return RedirectToAction("ViewEnrollment", new { termId = termId });
        }
    }
}