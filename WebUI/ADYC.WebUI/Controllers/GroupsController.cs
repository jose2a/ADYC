using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class GroupsController : Controller
    {
        private GroupRepository _groupRepository;

        public GroupsController()
        {
            _groupRepository = new GroupRepository();
        }

        // GET: CourseTypes
        public async Task<ActionResult> Index()
        {
            var groups = await _groupRepository.GetGroupAsync();

            return View(groups);
        }

        public ActionResult New()
        {
            var viewModel = new GroupFormViewModel
            {
                IsNew = true
            };

            return View("GroupForm", viewModel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            GroupFormViewModel viewModel = null;

            try
            {
                var group = await _groupRepository.GetGroupAsync(id.Value);

                viewModel = new GroupFormViewModel(group)
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

            return View("GroupForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(GroupFormViewModel form)
        {
            form.IsNew = form.Id == null;

            if (ModelState.IsValid)
            {
                try
                {
                    Group group = (form.IsNew)
                        ? new Group()
                        : await _groupRepository.GetGroupAsync(form.Id.Value);

                    group.Name = form.Name;

                    if (form.IsNew)
                    {
                        await _groupRepository.PostGroupAsync(group);
                    }
                    else
                    {
                        await _groupRepository.PutGroupAsync(group.Id, group);
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

            return View("GroupForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var statusCode = await _groupRepository.DeleteGroupAsync(id);

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