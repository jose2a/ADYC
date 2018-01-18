using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class TermConfiguration : EntityTypeConfiguration<Term>
    {
        public TermConfiguration()
        {
            // key
            HasKey(t => t.Id);

            // properties
            Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(300);

            Property(t => t.StartDate)
                .IsRequired();

            Property(t => t.EndDate)
                .IsRequired();

            Property(t => t.IsCurrentTerm)
                .IsOptional();

            Property(t => t.EnrollmentDeadLine)
                .IsRequired();

            Property(t => t.EnrollmentDropDeadLine)
                .IsRequired();
        }
    }
}
