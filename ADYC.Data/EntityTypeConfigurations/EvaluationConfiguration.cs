using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class EvaluationConfiguration : EntityTypeConfiguration<Evaluation>
    {
        public EvaluationConfiguration()
        {
            // keys
            HasKey(e => new { e.EnrollmentId, e.PeriodId });

            Property(e => e.EnrollmentId)
                .IsRequired()
                .HasColumnOrder(0);

            Property(e => e.PeriodId)
                .IsRequired()
                .HasColumnOrder(1);

            // properties
            Property(e => e.PeriodGrade)
                .IsRequired();

            Property(e => e.PeriodGradeLetter)
                .IsRequired();

            Property(e => e.Notes)
                .IsOptional()
                .HasMaxLength(300);

            // relationships
            HasRequired(e => e.Enrollment)
                .WithMany(er => er.Evaluations)
                .WillCascadeOnDelete(false);

            HasRequired(e => e.Period)
                .WithMany(p => p.Evaluations)
                .WillCascadeOnDelete(false);
        }
    }
}
