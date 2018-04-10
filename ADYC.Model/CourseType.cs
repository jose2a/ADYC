using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class CourseType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }

        public CourseType()
        {
            Courses = new List<Course>();
        }

        public CourseType(string name) : this()
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
