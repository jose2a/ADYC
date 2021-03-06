﻿using ADYC.API.ViewModels;
using ADYC.WebUI.Attributes;
using ADYC.WebUI.Controllers;
using ADYC.WebUI.CustomAttributes;
using ADYC.WebUI.Exceptions;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "AppAdmin")]
    [SelectedTab("majors")]
    public class MajorsController : ADYCBasedController
    {
        private MajorRepository _majorRepository;

        public MajorsController()
        {
            _majorRepository = new MajorRepository();
        }

        // GET: Admin/Majors
        public async Task<ActionResult> Index()
        {
            var majors = await _majorRepository.GetMajors();

            // Add properties to layout
            AddPageHeader("Majors", "List of all majors");

            AddBreadcrumb("Majors", "");

            return View(majors);
        }

        // GET: Admin/Majors/New
        public ActionResult New()
        {
            var viewModel = new MajorFormViewModel
            {
                IsNew = true
            };

            // Add properties to layout
            AddPageHeader(viewModel.Title, "");

            AddBreadcrumb("Majors", Url.Action("Index", "Majors", new { area = "Admin" }));
            AddBreadcrumb(viewModel.Title, "");

            return View("MajorForm", viewModel);
        }

        // GET: Admin/Majors/Edit
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            MajorFormViewModel viewModel = null;

            try
            {
                var major = await _majorRepository.GetMajorById(id.Value);

                viewModel = new MajorFormViewModel(major)
                {
                    IsNew = false
                };
            }
            catch (BadRequestException bre)
            {
                AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
            }

            // Add properties to layout
            AddPageHeader(viewModel.Title, "");

            AddBreadcrumb("Majors", Url.Action("Index", "Majors", new { area = "Admin" }));
            AddBreadcrumb(viewModel.Title, "");

            return View("MajorForm", viewModel);
        }

        // POST: Admin/Majors/Save
        [HttpPost]
        public async Task<ActionResult> Save(MajorFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MajorDto major = (form.IsNew)
                        ? new MajorDto()
                        : await _majorRepository.GetMajorById(form.Id.Value);

                    major.Name = form.Name;

                    if (form.IsNew)
                    {
                        await _majorRepository.PostMajor(major);
                    }
                    else
                    {
                        await _majorRepository.PutMajor(major.Id.Value, major);
                    }

                    AddPageAlerts(ViewHelpers.PageAlertType.Success, "Your changes have been saved succesfully.");

                    return RedirectToAction("Index");
                }
                catch (BadRequestException bre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
                }
            }

            // Add properties to layout
            AddPageHeader(form.Title, "");

            AddBreadcrumb("Majors", Url.Action("Index", "Majors", new { area = "Admin" }));
            AddBreadcrumb(form.Title, "");

            return View("MajorForm", form);
        }

        // GET: Admin/Majors/Delete
        [HttpGet]
        [OnlyAjaxRequest]
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            try
            {
                var statusCode = await _majorRepository.DeleteGrade(id.Value);

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
    }

}