using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Pushs
{
    public interface Push
    {
        void Send();
        void AddToken(String Token);
    }
}
