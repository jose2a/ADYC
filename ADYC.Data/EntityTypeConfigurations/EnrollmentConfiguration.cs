using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class EnrollmentConfiguration : EntityTypeConfiguration<Enrollment>
    {
        public EnrollmentConfiguration()
        {
            // Key
            HasKey(e => e.Id);

            // Properties
            Property(e => e.FinalGrade)
                .IsOptional();

            Property(e => e.FinalGradeLetter)
                .IsOptional();

            Property(e => e.Notes)
                .IsOptional()
                .HasMaxLength(300);

            Property(e => e.IsCurrentEnrollment)
                .IsRequired();

            Property(e => e.EnrollmentDate)
                .IsRequired();

            Property(e => e.WithdropDate)
                .IsOptional();

            // Relationships
            HasRequired(e => e.Offering)
                .WithMany(o => o.Enrollments)
                .HasForeignKey(e => e.OfferingId)
                .WillCascadeOnDelete(false);
        }
    }
}
