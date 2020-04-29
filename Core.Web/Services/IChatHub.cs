using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Services
{
    public interface IChatHub
    {
        Task Receive(string message);
        Task LoginSuccess(long userId);
    }
}
