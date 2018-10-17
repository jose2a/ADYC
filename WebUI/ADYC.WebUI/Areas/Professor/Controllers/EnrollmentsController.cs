using ADYC.WebUI.Controllers;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
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

        public EnrollmentsController()
        {
            _termRepository = new TermRepository();
            _offeringRepository = new OfferingRepository();
            _enrollmentRepository = new EnrollmentRepository();
            _evaluationRepository = new EvaluationRepository();
            _periodRepository = new PeriodRepository();
        }


        // GET: Professor/Offerings
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

            var offerings = _offeringRepository.GetOfferingsByProfessorIdAndTermId(new Guid("63016919-365a-e811-9b75-b8763fed7266"), termId.Value);

            return View(new OfferingListViewModel
            {
                Term = term,
                Offerings = offerings
            });
        }

        public async Task<ActionResult> Enrollments(int? offeringId)
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

        public async Task<ActionResult> Evaluations(int? enrollmentId)
        {
            if (!enrollmentId.HasValue)
            {
                return HttpNotFound();
            }

            var enrollmentWithEvaluations = await _evaluationRepository.GetWithEvaluations(enrollmentId.Value);
            enrollmentWithEvaluations.IsCurrentTerm = enrollmentWithEvaluations.Enrollment.Offering.Term.IsCurrentTerm;

            return View(enrollmentWithEvaluations);
        }

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

                    return RedirectToAction("Evaluations", new { enrollmentId = enrollmentWithEvaluations.Enrollment.Id });
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            form.Enrollment = await _enrollmentRepository.GetById(form.Enrollment.Id);
            foreach (var e in form.Evaluations)
            {
                e.Period = await _periodRepository.GetPeriodById(e.PeriodId);
            }

            return View("Evaluations", form);
        }
    }
}