using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Util.Exceptions
{
    public class NonexistenEntityException : Exception
    {
        public NonexistenEntityException(string message, Exception innerException) 
            : base(message, innerException)
        {
            
        }
    }
}
