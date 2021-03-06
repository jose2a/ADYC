﻿using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class ScheduleConfiguration : EntityTypeConfiguration<Schedule>
    {
        public ScheduleConfiguration()
        {
            // keys
            HasKey(s => s.Id);

            // properties
            Property(s => s.StartTime)
                .IsOptional();

            Property(s => s.EndTime)
                .IsOptional();

            Property(s => s.Day)
                .IsRequired();

        }
    }
}
