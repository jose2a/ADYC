using ADYC.API.Auth.Models;
using ADYC.API.ViewModels;
using ADYC.WebUI.Controllers;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "AppAdmin")]
    public class StudentsController : ADYCBasedController
    {
        private StudentRepository _studentRepository;
        private GradeRepository _gradeRepository;
        private GroupRepository _groupRepository;
        private MajorRepository _majorRepository;
        private AccountRepository _accountRepository;

        public StudentsController()
        {
            _studentRepository = new StudentRepository();
            _gradeRepository = new GradeRepository();
            _groupRepository = new GroupRepository();
            _majorRepository = new MajorRepository();
            _accountRepository = new AccountRepository();
        }

        // GET: Admin/Students
        public async Task<ActionResult> Index()
        {
            var students = await _studentRepository.GetStudents();

            return View(students);
        }

        // GET: Admin/Students/New
        public async Task<ActionResult> New()
        {
            var viewModel = new StudentFormViewModel
            {
                IsNew = true
            };

            await SetStudentViewModelListsProperties(viewModel);

            return View("StudentForm", viewModel);
        }

        // GET: Admin/Students/Edit
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            StudentFormViewModel viewModel = null;

            try
            {
                var student = await _studentRepository.GetStudentById(id.Value);

                viewModel = new StudentFormViewModel(student)
                {
                    IsNew = false
                };

                await SetStudentViewModelListsProperties(viewModel);
            }
            catch (AdycHttpRequestException ahre)
            {
                if (ahre.StatusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);
            }

            return View("StudentForm", viewModel);
        }

        // POST: Admin/Students/Save
        [HttpPost]
        public async Task<ActionResult> Save(StudentFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                StudentDto newStudent = null;

                try
                {
                    StudentDto student = (form.IsNew)
                        ? new StudentDto()
                        : await _studentRepository.GetStudentById(form.Id.Value);

                    student.FirstName = form.FirstName;
                    student.LastName = form.LastName;
                    student.Email = form.Email;
                    student.CellphoneNumber = form.CellphoneNumber;
                    student.GradeId = form.GradeId;
                    student.GroupId = form.GroupId;
                    student.MajorId = form.MajorId;

                    if (form.IsNew)
                    {
                        //student.Id = Guid.NewGuid(); // Get this from Auth service
                        newStudent = await _studentRepository.PostStudent(student);

                        var registerBindingModel = new RegisterBindingModel
                        {
                            Email = student.Email,
                            Password = "ChangeItAsap123!",
                            UserId = newStudent.Id,
                            UserRole = "AppStudent"
                        };

                        await _accountRepository.RegisterAccount(registerBindingModel);
                    }
                    else
                    {
                        await _studentRepository.PutStudent(student.Id, student);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    AddErrorsFromAdycHttpExceptionToModelState(ahre, ModelState);

                    if (newStudent != null)
                    {
                        await _accountRepository.DeleteAccount(newStudent.Email);
                    }
                }
            }

            await SetStudentViewModelListsProperties(form);

            return View("StudentForm", form);
        }

        // GET: Admin/Students/Delete
        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var student = await _studentRepository.GetStudentById(id);

                var statusCode = await _accountRepository.DeleteAccount(student.Email);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                statusCode = await _studentRepository.DeleteStudent(id);

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

        // GET: Admin/Students/Trash
        [HttpGet]
        public async Task<ActionResult> Trash(Guid id)
        {
            try
            {
                var statusCode = await _studentRepository.TrashStudent(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(ahre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }

            return PartialView("pv_StudentRow", await _studentRepository.GetStudentById(id));
        }

        // GET: Admin/Students/Restore
        [HttpGet]
        public async Task<ActionResult> Restore(Guid id)
        {
            try
            {
                var statusCode = await _studentRepository.RestoreStudent(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
            }
            catch (AdycHttpRequestException ahre)
            {
                var errorString = GetErrorsFromAdycHttpExceptionToString(ahre);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorString);
            }

            return PartialView("pv_StudentRow", await _studentRepository.GetStudentById(id));
        }

        private async Task SetStudentViewModelListsProperties(StudentFormViewModel viewModel)
        {
            viewModel.Grades = await _gradeRepository.GetGrades();
            viewModel.Groups = await _groupRepository.GetGroups();
            viewModel.Majors = await _majorRepository.GetMajors();
        }
    }
}