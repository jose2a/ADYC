using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class CourseTypeConfiguration : EntityTypeConfiguration<CourseType>
    {
        public CourseTypeConfiguration()
        {
            // Primary key
            HasKey(c => c.Id);

            // Properties
            Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
