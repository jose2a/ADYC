using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class Enrollment
    {
        public int Id { get; set; }
        public Double? FinalGrade { get; set; }
        public GradeLetter? FinalGradeLetter { get; set; }
        public string Notes { get; set; }
        public bool IsCurrentEnrollment { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public DateTime? WithdropDate { get; set; }

        public Guid StudentId { get; set; }
        public virtual Student Student { get; set; }
        public int OfferingId { get; set; }
        public Offering Offering { get; set; }

        public ICollection<Evaluation> Evaluations { get; set; }

        public Enrollment()
        {
            Evaluations = new List<Evaluation>();
        }

        public Enrollment(Guid studentId, int offeringId) : this()
        {
            StudentId = studentId;
            OfferingId = offeringId;
        }
    }
}
