using ADYC.Model;
using System;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface IProfessorService
    {
        Professor Get(int id);
        IEnumerable<Professor> GetAll();        
        IEnumerable<Professor> FindByFirstName(string firstName);        
        IEnumerable<Professor> FindByLastName(string lastName);
        IEnumerable<Professor> FindByEmail(string email);
        IEnumerable<Professor> FindByCellphoneNumber(string cellphoneNumber);
        IEnumerable<Professor> FindNotSoftDeletedProfessors();
        IEnumerable<Professor> FindSoftDeletedProfessors();

        IEnumerable<Offering> GetProfessorOfferings(Guid professorId);

        void Add(Professor professor);
        void AddRange(IEnumerable<Professor> professors);

        void Update(Professor professor);

        void Remove(Professor professor);
        void RemoveRange(IEnumerable<Professor> professors);
        void SoftDelete(Professor professor);
        void SoftDeleteRange(IEnumerable<Professor> professors);
    }
}
