using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adastra
{
    public class InvalidRecordException : System.Exception
    {
        public override string Message
        {
            get
            {
                return "Adastra has detected that your record is invalid! Usually this is a result of corrupted data coming from slow performing OpenVibe.\r\n";
            }
        }
    }
}
