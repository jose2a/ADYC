using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Util.Exceptions
{
    public class ForeignKeyEntityException : Exception
    {
        public ForeignKeyEntityException(string message)
            :base(message)
        {

        }

        public ForeignKeyEntityException(string message, Exception innerException)
            :base(message, innerException)
        {

        }
    }
}
