using ADYC.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADYC.Model;
using ADYC.IRepository;
using ADYC.Util.Exceptions;

namespace ADYC.Service
{
    public class GradeService : IGradeService
    {
        private IGradeRepository _gradeRepository;

        public GradeService(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        public void Add(Grade grade)
        {
            if (grade == null)
            {
                throw new ArgumentNullException("grade");
            }

            if (_gradeRepository.Find(c => c.Name.Equals(grade.Name)).Count() > 0)
            {
                throw new PreexistingEntityException("A grade with the same name already exists.", null);
            }

            _gradeRepository.Add(grade);
        }

        public IEnumerable<Grade> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name should not be empty.");
            }

            return _gradeRepository.Find(c => c.Name.Contains(name));
        }

        public Grade Get(int id)
        {
            return _gradeRepository.Get(id);
        }

        public IEnumerable<Grade> GetAll()
        {
            return _gradeRepository.GetAll();
        }

        public void Remove(Grade grade)
        {
            if (grade == null)
            {
                throw new ArgumentNullException("grade");
            }

            if (grade.Students.Count > 0)
            {
                throw new ForeignKeyEntityException("A grade could not be removed. It has one or more students associated with it.");
            }

            _gradeRepository.Remove(grade);
        }

        public void RemoveRange(IEnumerable<Grade> grades)
        {
            if (grades.Count() == 0 || grades == null)
            {
                throw new ArgumentNullException("grades");
            }

            var hasStudents = grades.Count(g => g.Students.Count > 0);

            if (hasStudents > 0)
            {
                throw new ForeignKeyEntityException("A grade could not be removed. It has one or more students associated wiht it.");
            }

            _gradeRepository.RemoveRange(grades);
        }

        public void Update(Grade grade)
        {
            if (grade == null)
            {
                throw new ArgumentNullException("grade");
            }

            if (_gradeRepository.Get(grade.Id) == null)
            {
                throw new NonexistingEntityException("Grade does not currently exist.");
            }

            _gradeRepository.Update(grade);
        }
    }
}
