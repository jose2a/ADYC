using ADYC.Model;
using ADYC.WebUI.Infrastructure;
using ADYC.WebUI.Repositories;
using ADYC.WebUI.ViewModels;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ADYC.WebUI.Controllers
{
    public class StudentsController : ADYCBasedController
    {
        private StudentRepository _studentRepository;
        private GradeRepository _gradeRepository;
        private GroupRepository _groupRepository;
        private MajorRepository _majorRepository;

        public StudentsController()
        {
            _studentRepository = new StudentRepository();
            _gradeRepository = new GradeRepository();
            _groupRepository = new GroupRepository();
            _majorRepository = new MajorRepository();
        }

        // GET: Professors
        public async Task<ActionResult> Index()
        {
            var students = await _studentRepository.GetStudentsAsync();

            return View(students);
        }

        public async Task<ActionResult> New()
        {
            var viewModel = new StudentFormViewModel
            {
                IsNew = true,
                Grades = await _gradeRepository.GetGradesAsync(),
                Groups = await _groupRepository.GetGroupsAsync(),
                Majors = await _majorRepository.GetMajorsAsync()
            };

            return View("StudentForm", viewModel);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (!id.HasValue)
            {
                return HttpNotFound();
            }

            StudentFormViewModel viewModel = null;

            try
            {
                var student = await _studentRepository.GetStudentAsync(id.Value);

                viewModel = new StudentFormViewModel(student)
                {
                    IsNew = false,
                    Grades = await _gradeRepository.GetGradesAsync(),
                    Groups = await _groupRepository.GetGroupsAsync(),
                    Majors = await _majorRepository.GetMajorsAsync()
                };
            }
            catch (AdycHttpRequestException ahre)
            {
                if (ahre.StatusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

                ProcessAdycHttpException(ahre, ModelState);
            }

            return View("StudentForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(StudentFormViewModel form)
        {
            form.IsNew = !form.Id.HasValue;

            if (ModelState.IsValid)
            {
                try
                {
                    Student student = (form.IsNew)
                        ? new Student()
                        : await _studentRepository.GetStudentAsync(form.Id.Value);

                    student.FirstName = form.FirstName;
                    student.LastName = form.LastName;
                    student.Email = form.Email;
                    student.CellphoneNumber = form.CellphoneNumber;
                    student.GradeId = form.GradeId;
                    student.GroupId = form.GroupId;
                    student.MajorId = form.MajorId;

                    if (form.IsNew)
                    {
                        student.Id = Guid.NewGuid(); // Get this from Auth service
                        await _studentRepository.PostStudentAsync(student);
                    }
                    else
                    {
                        await _studentRepository.PutStudentAsync(student.Id, student);
                    }

                    return RedirectToAction("Index");
                }
                catch (AdycHttpRequestException ahre)
                {
                    ProcessAdycHttpException(ahre, ModelState);
                }
            }

            form.Grades = await _gradeRepository.GetGradesAsync();
            form.Groups = await _groupRepository.GetGroupsAsync();
            form.Majors = await _majorRepository.GetMajorsAsync();

            return View("StudentForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var statusCode = await _studentRepository.DeleteStudentAsync(id);

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

        [HttpGet]
        public async Task<ActionResult> Trash(Guid id)
        {
            try
            {
                var statusCode = await _studentRepository.TrashStudentAsync(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }
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

            return PartialView("pv_StudentRow", await _studentRepository.GetStudentAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult> Restore(Guid id)
        {
            try
            {
                var statusCode = await _studentRepository.RestoreStudentAsync(id);

                if (statusCode == HttpStatusCode.NotFound)
                {
                    return HttpNotFound();
                }

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

            return PartialView("pv_StudentRow", await _studentRepository.GetStudentAsync(id));
        }
    }
}