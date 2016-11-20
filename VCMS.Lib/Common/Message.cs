using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCMS.Lib.Common
{
    public class Message
    {
        public MessageTypes MessageType { get; set; }
        public string MessageContent { get; set; }
    }

    public enum MessageTypes {Info, Success, Danger, Warning}
}
