using ADYC.Model;
using System;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface IProfessorService
    {
        Professor Get(Guid id);
        IEnumerable<Professor> GetAll();        
        IEnumerable<Professor> FindByFirstName(string firstName);        
        IEnumerable<Professor> FindByLastName(string lastName);
        IEnumerable<Professor> FindByEmail(string email);
        IEnumerable<Professor> FindByCellphoneNumber(string cellphoneNumber);
        IEnumerable<Professor> FindNotTrashedProfessors();
        IEnumerable<Professor> FindTrashedProfessors();

        IEnumerable<Offering> GetProfessorOfferings(Guid professorId);

        void Add(Professor professor);
        void AddRange(IEnumerable<Professor> professors);

        void Update(Professor professor);

        void Remove(Professor professor);
        void RemoveRange(IEnumerable<Professor> professors);
        void Trash(Professor professor);
        void TrashRange(IEnumerable<Professor> professors);
        void Restore(Professor professor);
        void RestoreRange(IEnumerable<Professor> professors);
    }
}
