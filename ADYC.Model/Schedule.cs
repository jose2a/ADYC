﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class Schedule
    {
        public int Id { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public int OfferingId { get; set; }
        public virtual Offering Offering { get; set; }

        public Day Day { get; set; }

        public Schedule(int offeringId, Day day)
        {
            OfferingId = offeringId;
            Day = day;
        }
    }
}
