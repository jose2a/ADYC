﻿using ADYC.API.ViewModels;
using ADYC.WebUI.Controllers;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "AppAdmin")]
    public class CourseTypesController : ADYCBasedController
    {
        private CourseTypeRepository _courseTypeRepository;

        public CourseTypesController()
        {
            _courseTypeRepository = new CourseTypeRepository();
        }

        // GET: Admin/CourseTypes
        public async Task<ActionResult> Index()
        {
            var courseTypes = await _courseTypeRepository.GetCourseTypes();

            return View(courseTypes);
        }

        // GET: Admin/CourseTypes/New
        public ActionResult New()
        {
            var viewModel = new CourseTypeFormViewModel
            {
                IsNew = true
            };

            return View("CourseTypeForm", viewModel);
        }

        // GET: Admin/CourseTypes/Edit
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            CourseTypeFormViewModel viewModel = null;

            try
            {
                var courseType = await _courseTypeRepository.GetCourseTypeById(id.Value);

                viewModel = new CourseTypeFormViewModel(courseType)
                {
                    IsNew = false
                };
            }
            catch (AdycHttpRequestException ahre)
            {
                if (ahre.StatusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
            }

            return View("CourseTypeForm", viewModel);
        }

        // POST: Admin/CourseTypes/Save
        [HttpPost]
        public async Task<ActionResult> Save(CourseTypeFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CourseTypeDto courseType = (form.IsNew)
                        ? new CourseTypeDto()
                        : await _courseTypeRepository.GetCourseTypeById(form.Id.Value);

                    courseType.Name = form.Name;

                    if (form.IsNew)
                    {
                        await _courseTypeRepository.PostCourseType(courseType);
                    }
                    else
                    {
                        await _courseTypeRepository.PutCourseType(courseType.Id.Value, courseType);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            return View("CourseTypeForm", form);
        }

        // GET: Admin/CourseTypes/Delete
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _courseTypeRepository.DeleteCourseType(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(ahre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }
        }
    }
}