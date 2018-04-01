using ADYC.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Data.EntityTypeConfigurations
{
    public class StudentConfiguration : EntityTypeConfiguration<Student>
    {
        public StudentConfiguration()
        {
            // Primary key
            HasKey(s => s.Id);
            Property(s => s.Id)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            // Properties
            Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(60);

            Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(60);

            Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            Property(s => s.CellphoneNumber)
                .IsRequired().
                HasMaxLength(30);

            Property(s => s.IsDeleted)
                .IsRequired();

            Property(s => s.CreatedAt)
                .IsRequired();

            Property(s => s.UpdatedAt)
                .IsOptional();

            Property(s => s.DeletedAt)
                .IsOptional();

            // Relationships
            HasRequired(s => s.Grade)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GradeId)
                .WillCascadeOnDelete(false);

            HasRequired(s => s.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(s => s.GroupId)
                .WillCascadeOnDelete(false);

            HasRequired(s => s.Major)
                .WithMany(m => m.Students)
                .HasForeignKey(s => s.MajorId)
                .WillCascadeOnDelete(false);

            HasMany(s => s.Enrollments)
                .WithRequired(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .WillCascadeOnDelete(false);
        }
    }
}
