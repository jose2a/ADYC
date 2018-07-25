using ADYC.Model;
using System.Collections.Generic;

namespace ADYC.IService
{
    public interface ICourseTypeService
    {
        CourseType Get(int id);

        IEnumerable<CourseType> GetAll();
        IEnumerable<CourseType> FindByName(string name);

        void Add(CourseType courseType);

        void Update(CourseType courseType);

        void Remove(CourseType courseType);
        void RemoveRange(IEnumerable<CourseType> courseTypes);
    }
}
