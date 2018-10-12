using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class CourseTypesController : ADYCBasedController
    {
        private CourseTypeRepository _courseTypeRepository;

        public CourseTypesController()
        {
            _courseTypeRepository = new CourseTypeRepository();
        }

        // GET: CourseTypes
        public async Task<ActionResult> Index()
        {
            var courseTypes = await _courseTypeRepository.GetCourseTypes();

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

        [HttpPost]
        public async Task<ActionResult> Save(CourseTypeFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CourseType courseType = (form.IsNew)
                        ? new CourseType()
                        : await _courseTypeRepository.GetCourseTypeById(form.Id.Value);

                    courseType.Name = form.Name;

                    if (form.IsNew)
                    {
                        await _courseTypeRepository.PostCourseType(courseType);
                    }
                    else
                    {
                        await _courseTypeRepository.PutCourseType(courseType.Id, courseType);
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