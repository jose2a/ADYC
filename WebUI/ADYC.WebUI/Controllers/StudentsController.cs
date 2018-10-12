﻿using ADYC.Model;
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

        public async Task<ActionResult> Index()
        {
            var students = await _studentRepository.GetStudents();

            return View(students);
        }

        public async Task<ActionResult> New()
        {
            var viewModel = new StudentFormViewModel
            {
                IsNew = true,
                Grades = await _gradeRepository.GetGrades(),
                Groups = await _groupRepository.GetGroups(),
                Majors = await _majorRepository.GetMajors()
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
                var student = await _studentRepository.GetStudentById(id.Value);

                viewModel = new StudentFormViewModel(student)
                {
                    IsNew = false,
                    Grades = await _gradeRepository.GetGrades(),
                    Groups = await _groupRepository.GetGroups(),
                    Majors = await _majorRepository.GetMajors()
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

            return View("StudentForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(StudentFormViewModel form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Student student = (form.IsNew)
                        ? new Student()
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
                        student.Id = Guid.NewGuid(); // Get this from Auth service
                        await _studentRepository.PostStudent(student);
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
                }
            }

            form.Grades = await _gradeRepository.GetGrades();
            form.Groups = await _groupRepository.GetGroups();
            form.Majors = await _majorRepository.GetMajors();

            return View("StudentForm", form);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var statusCode = await _studentRepository.DeleteStudent(id);

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
    }
}