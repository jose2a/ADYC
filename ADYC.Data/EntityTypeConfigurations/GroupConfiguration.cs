using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            // keys
            HasKey(g => g.Id);

            // properties
            Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
