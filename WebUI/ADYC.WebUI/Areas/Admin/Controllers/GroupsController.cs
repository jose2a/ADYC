using ADYC.API.ViewModels;
using ADYC.WebUI.Controllers;
using ADYC.WebUI.CustomAttributes;
using ADYC.WebUI.Exceptions;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "AppAdmin")]
    [SelectedTab("groups")]
    public class GroupsController : ADYCBasedController
    {
        private GroupRepository _groupRepository;

        public GroupsController()
        {
            _groupRepository = new GroupRepository();
        }

        // GET: Admin/Groups
        public async Task<ActionResult> Index()
        {
            var groups = await _groupRepository.GetGroups();

            return View(groups);
        }

        // GET: Admin/Groups/New
        public ActionResult New()
        {
            var viewModel = new GroupFormViewModel
            {
                IsNew = true
            };

            return View("GroupForm", viewModel);
        }

        // GET: Admin/Groups/Edit
        public async Task<ActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            GroupFormViewModel viewModel = null;

            try
            {
                var group = await _groupRepository.GetGroupById(id.Value);

                viewModel = new GroupFormViewModel(group)
                {
                    IsNew = false
                };
            }
            catch (BadRequestException bre)
            {
                AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
            }

            return View("GroupForm", viewModel);
        }

        // POST: Admin/Groups/Save
        [HttpPost]
        public async Task<ActionResult> Save(GroupFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GroupDto group = (form.IsNew)
                        ? new GroupDto()
                        : await _groupRepository.GetGroupById(form.Id.Value);

                    group.Name = form.Name;

                    if (form.IsNew)
                    {
                        await _groupRepository.PostGroup(group);
                    }
                    else
                    {
                        await _groupRepository.PutGroup(group.Id.Value, group);
                    }

                    TempData["successMsg"] = "Your changes have been saved succesfully.";

                    return RedirectToAction("Index");
                }
                catch (BadRequestException bre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(bre, ModelState);
                }
            }

            return View("GroupForm", form);
        }

        // GET: Admin/Groups/Delete
        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            try
            {
                var statusCode = await _groupRepository.DeleteGroup(id.Value);

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