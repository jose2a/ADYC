using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Util.Exceptions
{
    public class NonexistingEntityException : Exception
    {
        public NonexistingEntityException()
        {

        }

        public NonexistingEntityException(string message)
            : base(message)
        {

        }

        public NonexistingEntityException(string message, Exception innerException) 
            : base(message, innerException)
        {
            
        }
    }
}
