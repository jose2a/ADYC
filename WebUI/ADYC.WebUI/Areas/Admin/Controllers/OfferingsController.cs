using ADYC.API.ViewModels;
using ADYC.WebUI.Attributes;
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

namespace ADYC.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "AppAdmin")]
    [SelectedTab("offerings")]
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

        // GET: Admin/Offerings
        public async Task<ActionResult> Index()
        {
            var terms = await _termRepository.GetTerms();

            // Add properties to layout
            AddPageHeader("Offerings (Terms)", "List of terms");

            AddBreadcrumb("Offerings (Terms)", "");

            return View(terms);
        }

        // GET: Admin/Offerings/View
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

            // Add properties to layout
            AddPageHeader("Offerings (Terms)", "List of terms");

            AddBreadcrumb("Offerings (Terms)", Url.Action("Index"));
            AddBreadcrumb("Offerings (List)", "");

            return View(new OfferingListViewModel
            {
                Term = term,
                Offerings = offerings
            });
        }

        // POST: Admin/Offerings/New
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
                TermId = form.TermId.Value
            };

            await SetOfferingListProperties(viewModel);

            // Add properties to layout
            AddPageHeader(viewModel.FormTitle, "");

            AddBreadcrumb("Offerings (Terms)", Url.Action("Index"));
            AddBreadcrumb("Offerings (List)", Url.Action("View", new { TermId = form.TermId.Value }));
            AddBreadcrumb(viewModel.FormTitle, "");

            return View("OfferingForm", viewModel);
        }

        // POST: Admin/Offerings/Edit
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
            catch (BadRequestException bre)
            {
                AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
            }

            await SetOfferingListProperties(viewModel);

            // Add properties to layout
            AddPageHeader(viewModel.FormTitle, "");

            AddBreadcrumb("Offerings (Terms)", Url.Action("Index"));
            AddBreadcrumb("Offerings (List)", Url.Action("View", new { TermId = form.TermId.Value }));
            AddBreadcrumb(viewModel.FormTitle, "");

            return View("OfferingForm", viewModel);
        }

        // POST: Admin/Offerings/Save
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> Save(OfferingFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    OfferingDto offering = (form.IsNew)
                        ? new OfferingDto()
                        : await _offeringRepository.GetOfferingById(form.Id.Value);

                    offering.Title = form.Title;
                    offering.Location = form.Location;
                    offering.OfferingDays = form.OfferingDays;
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

                    AddPageAlerts(ViewHelpers.PageAlertType.Success, "Your changes have been saved succesfully.");

                    return RedirectToAction("View", new { termId = form.TermId });
                }
                catch (BadRequestException bre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
                }
            }

            await SetOfferingListProperties(form);

            // Add properties to layout
            AddPageHeader(form.FormTitle, "");

            AddBreadcrumb("Offerings (Terms)", Url.Action("Index"));
            AddBreadcrumb("Offerings (List)", Url.Action("View", new { TermId = form.TermId }));
            AddBreadcrumb(form.FormTitle, "");

            return View("OfferingForm", form);
        }

        // GET: Admin/Offerings/Delete
        [HttpGet]
        [OnlyAjaxRequest]
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
            catch (BadRequestException bre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(bre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }
        }

        // GET: Admin/Offerings/Schedules
        public async Task<ActionResult> Schedules(int? offeringId)
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
            AddPageHeader("Schedules", "Schedules for this offering");

            AddBreadcrumb("Offerings (Terms)", Url.Action("Index"));
            AddBreadcrumb("Offerings (List)", Url.Action("View", new { TermId = viewModel.Offering.TermId }));
            AddBreadcrumb("Schedules", "");

            return View(viewModel);
        }

        // POST: Admin/Offerings/SaveSchedules
        [HttpPost]
        public async Task<ActionResult> SaveSchedules(ScheduleListViewModel form)
        {
            ModelState.Remove("Offering.Title");
            ModelState.Remove("Offering.Location");
            ModelState.Remove("Offering.Term");            

            if (ModelState.IsValid)
            {
                try
                {
                    form.Schedules = form.Schedules.Where(s => s.StartTime.HasValue && s.EndTime.HasValue).ToList();

                    var scheduleList = new ScheduleListDto
                    {
                        Schedules = form.Schedules
                    };                    

                    if (form.IsNew)
                    {
                        scheduleList.Offering = await _offeringRepository.GetOfferingById(form.OfferingId);
                        await _scheduleRepository.PostScheduleList(scheduleList);
                    }
                    else
                    {
                        await _scheduleRepository.PutScheduleList(form.OfferingId, scheduleList);
                    }

                    AddPageAlerts(ViewHelpers.PageAlertType.Success, "Your changes have been saved succesfully.");

                    return RedirectToAction("Schedules", new { offeringId = form.OfferingId });
                }
                catch (BadRequestException bre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
                }
            }

            var days = GetDayEnumViewModelList();

            form.Offering = await _offeringRepository.GetOfferingById(form.OfferingId);
            form.Schedules = GetScheduleList(form.OfferingId, form.Schedules.ToList(), days);
            form.Days = days;

            // Add properties to layout
            AddPageHeader("Schedules", "Schedules for this offering");

            AddBreadcrumb("Offerings (Terms)", Url.Action("Index"));
            AddBreadcrumb("Offerings (List)", Url.Action("View", new { TermId = form.Offering.TermId }));
            AddBreadcrumb("Schedules", "");

            return View("Schedules", form);
        }

        private async Task SetOfferingListProperties(OfferingFormViewModel viewModel)
        {
            viewModel.Courses = await _courseRepository.GetNotTrashedCourses();
            viewModel.Professors = await _professorRepository.GetNotTrashedProfessors();
            viewModel.Terms = await _termRepository.GetTerms();
        }
    }
}