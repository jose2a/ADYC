﻿using System;
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
        public bool IsDeleted { get; set; }

        public ICollection<Offering> Offerings { get; set; }

        public Professor()
        {
            Offerings = new List<Offering>();
        }

    }
}
