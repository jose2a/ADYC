using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class Professor
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CellphoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Offering> Offerings { get; set; }

        public Professor()
        {
            Offerings = new List<Offering>();
        }

        public Professor(Guid id, string firstName, string lastName, string email, string cellphoneNumber)
            : this()
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CellphoneNumber = cellphoneNumber;
        }

    }
}
