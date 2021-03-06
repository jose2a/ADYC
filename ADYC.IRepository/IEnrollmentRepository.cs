﻿using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.IRepository
{
    public interface IEnrollmentRepository : IReadOnlyRepository<Enrollment>, IWriteOnlyRepository<Enrollment>
    {
    }
}
