using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class ProfessorConfiguration : EntityTypeConfiguration<Professor>
    {
        public ProfessorConfiguration()
        {
            // Primary key
            HasKey(p => p.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            // Properties
            Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(60);

            Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(60);

            Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(100);

            Property(p => p.IsDeleted)
                .IsRequired();
        }
    }
}
