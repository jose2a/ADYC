using ADYC.API.ViewModels;
using ADYC.WebUI.Controllers;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Admin.Controllers
{
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

            return View(majors);
        }

        // GET: Admin/Majors/New
        public ActionResult New()
        {
            var viewModel = new MajorFormViewModel
            {
                IsNew = true
            };

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
            catch (AdycHttpRequestException ahre)
            {
                if (ahre.StatusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
            }

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

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
                }
            }

            return View("MajorForm", form);
        }

        // GET: Admin/Majors/Delete
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _majorRepository.DeleteGrade(id);

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