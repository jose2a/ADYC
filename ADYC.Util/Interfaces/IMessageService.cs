using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADYC.Util.Interfaces
{
    public interface IMessageService
    {
        bool Sent(string to, string subject, string body);
    }
}
