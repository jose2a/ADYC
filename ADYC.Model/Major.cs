using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class Major
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public Major()
        {
            Students = new List<Student>();
        }
    }
}
