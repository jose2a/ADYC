﻿using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.IRepository
{
    public interface IPeriodDateRepository : IReadOnlyRepository<PeriodDate>, IWriteOnlyRepository<PeriodDate>
    {
        PeriodDate Get(int periodId, int termId);
    }
}
