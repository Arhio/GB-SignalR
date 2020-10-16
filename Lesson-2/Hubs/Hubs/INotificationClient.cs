using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hubs
{
    public interface INotificationClient
    {
        Task Send(Message message);
    }
}
