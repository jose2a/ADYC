using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class OfferingConfiguration : EntityTypeConfiguration<Offering>
    {
        public OfferingConfiguration()
        {
            // keys
            HasKey(o => o.Id);

            // properties
            Property(o => o.Title)
                .HasMaxLength(300)
                .IsRequired();

            Property(o => o.Location)
                .HasMaxLength(500);

            Property(o => o.OfferingDays)
                .IsRequired();

            Property(o => o.Notes)
                .HasMaxLength(500);

            // relationships
            HasRequired(o => o.Professor)
                .WithMany(p => p.Offerings)
                .HasForeignKey(o => o.ProfessorId)
                .WillCascadeOnDelete(false);

            HasRequired(o => o.Term)
                .WithMany(t => t.Offerings)
                .HasForeignKey(o => o.TermId)
                .WillCascadeOnDelete(false);

            HasRequired(o => o.Course)
                .WithMany(c => c.Offerings)
                .HasForeignKey(o => o.CourseId)
                .WillCascadeOnDelete(false);

            HasMany(o => o.Schedules)
                .WithRequired(s => s.Offering)
                .HasForeignKey(s => s.OfferingId);
        }
    }
}
