using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Util.Exceptions
{
    public class ForeignKeyException : Exception
    {
        public ForeignKeyException(string message)
            :base(message)
        {

        }

        public ForeignKeyException(string message, Exception innerException)
            :base(message, innerException)
        {

        }
    }
}
