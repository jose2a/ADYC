using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
