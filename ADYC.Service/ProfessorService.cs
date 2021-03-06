﻿using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using ADYC.Model;
using ADYC.IRepository;
using ADYC.Util.Exceptions;

namespace ADYC.Service
{
    public class ProfessorService : IProfessorService
    {
        private IProfessorRepository _professorRepository;

        public ProfessorService(IProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }

        public void Add(Professor professor)
        {
            ValidateProfessor(professor);
            ValidateDuplicatedProfessor(professor);

            professor.CreatedAt = DateTime.Today;
            professor.IsDeleted = false;

            _professorRepository.Add(professor);
        }

        public void AddRange(IEnumerable<Professor> professors)
        {
            ValidateProfessorRange(professors);

            foreach (var professor in professors)
            {
                professor.CreatedAt = DateTime.Today;
                professor.IsDeleted = false;
            }

            _professorRepository.AddRange(professors);
        }       

        public IEnumerable<Professor> FindByCellphoneNumber(string cellphoneNumber)
        {
            if (string.IsNullOrEmpty(cellphoneNumber))
            {
                throw new ArgumentNullException("cellphoneNumber");
            }

            return _professorRepository.Find(p => p.CellphoneNumber.Contains(cellphoneNumber), o => o.OrderBy(p => p.CellphoneNumber));
        }

        public IEnumerable<Professor> FindByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("email");
            }

            return _professorRepository.Find(p => p.Email.Contains(email), o => o.OrderBy(p => p.Email));
        }

        public IEnumerable<Professor> FindByFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException("firstName");
            }

            return _professorRepository.Find(p => p.FirstName.Contains(firstName), o => o.OrderBy(p => p.FirstName));
        }

        public IEnumerable<Professor> FindByLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException("lastName");
            }

            return _professorRepository
                .Find(p => p.LastName.Contains(lastName), o => o.OrderBy(c => c.LastName));
        }

        public IEnumerable<Professor> FindNotTrashedProfessors()
        {
            return _professorRepository.Find(p => p.IsDeleted == false, o => o.OrderBy(p => p.Id));
        }

        public IEnumerable<Professor> FindTrashedProfessors()
        {
            return _professorRepository.Find(p => p.IsDeleted == true, o => o.OrderBy(p => p.Id));
        }

        public Professor Get(Guid id)
        {
            return _professorRepository.Get(id);
        }

        public IEnumerable<Professor> GetAll()
        {
            return _professorRepository.GetAll(orderBy: o => o.OrderBy(p => p.LastName));
        }

        public void Remove(Professor professor)
        {
            if (professor == null)
            {
                throw new ArgumentNullException("professor");
            }

            if (professor.Offerings.Count > 0)
            {
                throw new ForeignKeyEntityException("This professor could not be removed. It has one or more offerings.");
            }

            _professorRepository.Remove(professor);
        }

        public void RemoveRange(IEnumerable<Professor> professors)
        {
            if (professors.Count() == 0)
            {
                throw new ArgumentNullException("professors");
            }

            var hasOfferings = professors.Count(c => c.Offerings.Count > 0);

            if (hasOfferings > 0)
            {
                throw new ForeignKeyEntityException("A professor could not be removed. It has one or more offerings.");
            }

            _professorRepository.RemoveRange(professors);
        }

        public void Restore(Professor professor)
        {
            if (professor == null)
            {
                throw new ArgumentNullException("professor");
            }

            professor.IsDeleted = false;
            professor.DeletedAt = null;

            _professorRepository.Update(professor);
        }

        public void RestoreRange(IEnumerable<Professor> professors)
        {
            if (professors == null)
            {
                throw new ArgumentNullException("professors");
            }

            foreach (var professor in professors)
            {
                professor.IsDeleted = false;
                professor.DeletedAt = null;

                _professorRepository.Update(professor);
            }
        }

        public void Trash(Professor professor)
        {
            if (professor == null)
            {
                throw new ArgumentNullException("professor");
            }

            professor.IsDeleted = true;
            professor.DeletedAt = DateTime.Today;

            _professorRepository.Update(professor);
        }

        public void TrashRange(IEnumerable<Professor> professors)
        {
            if (professors.Count() == 0)
            {
                throw new ArgumentNullException("professors");
            }

            foreach (var professor in professors)
            {
                professor.IsDeleted = true;
                professor.DeletedAt = DateTime.Today;

                _professorRepository.Update(professor);
            }
        }

        public void Update(Professor professor)
        {
            ValidateProfessor(professor);

            professor.UpdatedAt = DateTime.Today;

            _professorRepository.Update(professor);
        }

        private void ValidateProfessor(Professor professor)
        {
            if (professor == null)
            {
                throw new ArgumentNullException("professor");
            }
        }

        private void ValidateDuplicatedProfessor(Professor professor)
        {
            var professorExist = (_professorRepository.Find(p => p.FirstName.Equals(professor.FirstName)).Count() > 0)
                            && (_professorRepository.Find(p => p.LastName.Equals(professor.LastName)).Count() > 0);

            if (professorExist)
            {
                throw new PreexistingEntityException("A professor with the same first name and last name already exists.");
            }
        }

        private void ValidateProfessorRange(IEnumerable<Professor> professors)
        {
            foreach (var p in professors)
            {
                ValidateProfessor(p);
                ValidateDuplicatedProfessor(p);
            }
        }
    }
}
