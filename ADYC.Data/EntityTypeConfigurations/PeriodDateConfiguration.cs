using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class PeriodDateConfiguration : EntityTypeConfiguration<PeriodDate>
    {
        public PeriodDateConfiguration()
        {
            // keys
            HasKey(pd => new { pd.TermId, pd.PeriodId });

            Property(pd => pd.TermId)
                .IsRequired()
                .HasColumnOrder(0);

            Property(pd => pd.PeriodId)
                .IsRequired()
                .HasColumnOrder(1);

            // properties
            Property(pd => pd.StartDate)
                .IsRequired();

            Property(pd => pd.EndDate)
                .IsRequired();

            // relationships
            HasRequired(pd => pd.Term)
                .WithMany(t => t.PeriodDates)
                .WillCascadeOnDelete(false);

            HasRequired(pd => pd.Period)
                .WithMany(p => p.PeriodDates)
                .WillCascadeOnDelete(false);
        }
    }
}
