using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public int CourseTypeId { get; set; }
        public CourseType CourseType { get; set; }

        public virtual ICollection<Offering> Offerings { get; set; }

        public Course()
        {
            IsDeleted = false;
            Offerings = new List<Offering>();
        }
    }
}
