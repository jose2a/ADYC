﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Model
{
    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public Grade()
        {
            Students = new List<Student>();
        }

        public Grade(string name) : this()
        {
            Name = name;
        }
    }
}
