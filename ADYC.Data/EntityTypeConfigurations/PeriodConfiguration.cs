using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class PeriodConfiguration : EntityTypeConfiguration<Period>
    {
        public PeriodConfiguration()
        {
            // key
            HasKey(p => p.Id);

            // properties
            Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            // relationships
            //HasMany(p => p.Evaluations)
            //    .WithRequired(ev => ev.Period)
            //    .HasForeignKey(ev => ev.PeriodId)
            //    .WillCascadeOnDelete(false);

            //HasMany(p => p.PeriodDates)
            //    .WithRequired(pd => pd.Period)
            //    .HasForeignKey(pd => pd.PeriodId)
            //    .WillCascadeOnDelete(false);

        }
    }
}
