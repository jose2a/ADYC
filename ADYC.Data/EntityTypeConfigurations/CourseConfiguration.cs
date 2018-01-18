using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class CourseConfiguration : EntityTypeConfiguration<Course>
    {
        public CourseConfiguration()
        {
            // Primary key
            HasKey(c => c.Id);

            // Properties
            Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(500);

            Property(c => c.IsDeleted)
                .IsRequired();

            // Relationships
            HasRequired(c => c.CourseType)
                .WithMany(ct => ct.Courses)
                .HasForeignKey(c => c.CourseTypeId)
                .WillCascadeOnDelete(false);
        }
    }
}
