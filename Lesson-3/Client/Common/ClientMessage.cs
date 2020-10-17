using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;

namespace Common
{
    public class ClientMessage : Message
    {
        public string NameClient { get; set; }
    }
}
