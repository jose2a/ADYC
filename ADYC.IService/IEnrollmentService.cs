using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.IService
{
    public interface IEnrollmentService
    {
        Enrollment Get(int id);

        void Add(Enrollment enrollment);

    }
}
