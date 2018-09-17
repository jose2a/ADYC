using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class CourseTypesController : Controller
    {
        private CourseTypeRepository _courseTypeRepository;

        public CourseTypesController()
        {
            _courseTypeRepository = new CourseTypeRepository();
        }

        // GET: CourseTypes
        public async Task<ActionResult> Index()
        {
            var courseTypes = await _courseTypeRepository.GetCourseTypesAsync();

            return View(courseTypes);
        }

        public ActionResult New()
        {
            var viewModel = new CourseTypeFormViewModel
            {
                IsNew = true
            };

            return View("CourseTypeForm", viewModel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            CourseTypeFormViewModel viewModel = null;

            try
            {
                var courseType = await _courseTypeRepository.GetCourseTypeAsync(id.Value);

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

                foreach (var error in ahre.Errors)
                {
                    ModelState.AddModelError("", ahre.Message);
                }
            }

            return View("CourseTypeForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(CourseTypeFormViewModel form)
        {
            form.IsNew = form.Id == null;

            if (ModelState.IsValid)
            {
                try
                {
                    CourseType courseType;

                    if (form.IsNew)
                    {
                        courseType = new CourseType();
                        courseType.Name = form.Name;
                    }
                    else
                    {
                        courseType = await _courseTypeRepository.GetCourseTypeAsync(form.Id.Value);
                    }

                    if (form.IsNew)
                    {
                        await _courseTypeRepository.PostCourseTypeAsync(courseType);
                    }
                    else
                    {
                        await _courseTypeRepository.PutCourseTypeAsync(courseType.Id, courseType);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    foreach (var error in ahre.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            return View("CourseTypeForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _courseTypeRepository.DeleteCourseTypeAsync(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = "";

                foreach (var error in ahre.Errors)
                {
                    errorString += error;
                }

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }

        }
    }
}