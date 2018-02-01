using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Util.Exceptions
{
    public class PreexistingEntityException : Exception
    {
        public PreexistingEntityException(string message) 
            : base(message)
        {

        }

        public PreexistingEntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
