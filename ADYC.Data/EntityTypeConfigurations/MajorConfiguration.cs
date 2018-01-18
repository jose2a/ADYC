using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class MajorConfiguration : EntityTypeConfiguration<Major>
    {
        public MajorConfiguration()
        {
            // key
            HasKey(m => m.Id);

            // properties
            Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(250);

            Property(m => m.IsDeleted)
                .IsRequired();
        }
    }
}
