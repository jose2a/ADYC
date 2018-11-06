using ADYC.API.ViewModels;
using ADYC.WebUI.Controllers;
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
    public class OfferingEnrollmentsController : ADYCBasedController
    {
        private OfferingRepository _offeringRepository;
        private TermRepository _termRepository;
        private ScheduleRepository _scheduleRepository;
        private EnrollmentRepository _enrollmentRepository;
        private StudentRepository _studentRepository;

        public OfferingEnrollmentsController()
        {
            _offeringRepository = new OfferingRepository();
            _termRepository = new TermRepository();
            _scheduleRepository = new ScheduleRepository();
            _enrollmentRepository = new EnrollmentRepository();
            _studentRepository = new StudentRepository();
        }

        // GET: Student/OferringEnrollments
        public async Task<ActionResult> Index()
        {
            var currentTerm = await _termRepository.GetCurrentTerm();

            if (currentTerm == null)
            {
                return RedirectToAction("NotCurrentTerm");
            }

            var studentId = SessionHelper.GetUser().UserId; //new Guid("65016919-365A-E811-9B75-B8763FED7266");

            var offeringsForCurrentTerm = _offeringRepository
                .GetOfferingsByTermId(currentTerm.Id.Value);

            var currentEnrollments = await _enrollmentRepository.
                GetEnrollmentsByStudentIdAndTermId(studentId, currentTerm.Id.Value);

            var isCurrentlyEnrolled = currentEnrollments.Count(e => !e.WithdropDate.HasValue) > 0;

            return View(new OfferingEnrollmentListViewModel
            {
                Term = currentTerm,
                Offerings = offeringsForCurrentTerm,
                IsStudentCurrentlyEnrolled = isCurrentlyEnrolled
            });
        }

        // GET: Student/OfferingEnrollments/ViewSchedules
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
            catch (AdycHttpRequestException ahre)
            {
                if (ahre.StatusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
            }

            return PartialView("pv_ViewSchedules", viewModel);
        }

        // GET: Student/OfferingEnrollments/Enrollment
        public async Task<ActionResult> Enrollment(int? offeringId)
        {
            if (!offeringId.HasValue)
            {
                return HttpNotFound();
            }

            try
            {
                var studentId = SessionHelper.GetUser().UserId; //new Guid("65016919-365A-E811-9B75-B8763FED7266");
                //var student = await _studentRepository.GetStudentById(studentId);

                var enrollment = new EnrollmentDto
                {
                    StudentId = studentId,
                    OfferingId = offeringId.Value
                };

                await _enrollmentRepository.PostEnrollment(enrollment);

                var offering = await _offeringRepository.GetOfferingById(offeringId.Value);

                return View(offering);
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

            return RedirectToAction("Index");
        }
    }
}